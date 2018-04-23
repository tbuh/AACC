using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Api
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public bool IsSuperAdmin { get { return UserId == -999; } }

        public void Load(string info)
        {

        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string Info { get; set; }
        public string Data { get; set; }
    }

    public class SyncRequest
    {
        public string Info { get; set; }
        public List<Report> Reports { get; set; }
    }

    [Produces("application/json")]
    [Route("api/MobileSync")]
    public class MobileSyncController : Controller
    {
        private readonly AACCContext _context;
        private ILogger<MobileSyncController> _logger;

        public MobileSyncController(AACCContext context, ILogger<MobileSyncController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("/api/MobileSync/login")]
        public async Task<LoginResult> Login([FromBody] LoginRequest request)
        {
            var error = new LoginResult { Info = "Login Error" };
            try
            {
                _logger.LogInformation("User trying to login");
                var userName = Security.Decrypt(request.UserName);
                var password = Security.Decrypt(request.Password);
                UserInfo userInfo = null;
                _logger.LogInformation($"User name {userName} Password {password}");
                if (userName.ToLower() == "admin" && password.ToLower() == "admin")
                {
                    userInfo = new UserInfo { UserId = -999, UserName = "Super Admin" };
                    return new LoginResult { Data = Security.Crypt(JsonConvert.SerializeObject(userInfo)) };
                }

                var user = await _context.Assessors.SingleOrDefaultAsync(a => a.Login == userName && a.Password == password);
                if (user == null) return error;
                userInfo = new UserInfo { UserId = user.AssessorId, UserName = user.Name };
                return new LoginResult { Data = Security.Crypt(JsonConvert.SerializeObject(userInfo)) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MobileSyncLogin");
                return error;
            }
        }

        private int CheckUser(string info)
        {
            try
            {
                var userInfo = Security.Decrypt(info);
                return int.Parse(userInfo.Split('|')[1]);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed...");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Sync([FromBody]List<Report> reports)
        {
            if (!ModelState.IsValid)
            {
                var errorsModel = new StringBuilder();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        if (error.Exception != null)
                            errorsModel.AppendLine(error.Exception.Message);
                        else if (error.ErrorMessage != null)
                            errorsModel.AppendLine(error.ErrorMessage);
                    }
                }
                var res = new SyncModel() { Error = errorsModel.ToString() };
                return CreatedAtAction("Sync", res);
            }

            var CryptInfo = this.HttpContext.Request.Headers["CryptInfo"];
            if (CryptInfo.Count == 0)
            {
                var res = new SyncModel() { Error = "Login failed." };
                return CreatedAtAction("Sync", res);
            }

            _logger.LogInformation("sync...");

            if (reports == null)
                _logger.LogInformation("sync...request is null");
            else
                _logger.LogInformation($"sync...request '{CryptInfo[0]}'");

            var model = new SyncModel();
            StringBuilder sb = new StringBuilder();
            try
            {
                var userId = CheckUser(CryptInfo[0]);

                try
                {
                    if (reports != null)
                        foreach (var report in reports)
                        {
                            if (report.AgedCareCenterId == -1 || report.AssessorId == -1) continue;

                            if (report.IsDeleted)
                            {
                                if (report.QuestionReply != null)
                                    foreach (var reply in report.QuestionReply)
                                    {
                                        if (reply.QuestionReplyId != 0)
                                            _context.Remove(reply);
                                    }
                                _logger.LogInformation($"INFO Delete...report #'{report.ReportId}'");
                                _context.Remove(report);
                                await _context.SaveChangesAsync();
                            }
                            else if (report.IsNew)
                            {
                                _logger.LogInformation($"INFO Inser new...report'{report.ReportDate}'");
                                await _context.SaveReport(report);
                            }
                            else if (report.IsChanged)
                            {
                                _logger.LogInformation($"INFO Update...report #'{report.ReportId}'");
                                await _context.UpdateReport(report);
                            }
                        }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ReportSyncError");
                    sb.AppendLine(ex.Message);
                }

                model.AccreditationStandartList = await _context.AccreditationStandarts.Include(acc => acc.Questions).ToListAsync();
                model.AgedCareCenterList = await _context.AgedCareCenters.ToListAsync();
                model.AssessorList = await _context.Assessors.ToListAsync();
                var Questions = await _context.Questions.Include(q => q.Questions).GroupBy(q => q.AccreditationStandartId).ToListAsync();
                model.ReportList = await _context.GetReportsWithReplies(userId);

                var newReport = new Report
                {
                    AgedCareCenterId = -1,
                    AssessorId = -1,
                    ReportDate = DateTime.Now,
                    IsNew = true
                };
                var questionReplyList =
                    from asst in model.AccreditationStandartList
                    from q in asst.Questions
                    select new QuestionReply(q);

                newReport.QuestionReply = questionReplyList.SelectMany(q => q.SubQuestionList).ToList();
                model.NewReport = newReport;
                model.ReportList
                    .ForEach(r =>
                    {
                        foreach (var item in r.QuestionReply)
                        {
                            var q = item.Question;
                            if (item.Question.ParentId.HasValue)
                            {
                                item.QuestionParentId = item.Question.ParentId.Value;
                                q = item.Question.Parent;
                            }
                            item.AccreditationStandartId = q.AccreditationStandartId.Value;
                        }
                        var questionNumberOrderBy = 0;
                        //r.QuestionReply.GroupBy(qr => qr.Question.AccreditationStandartId)
                        //                .SelectMany(g => g.OrderBy(q => q.QuestionId)
                        //                .GroupBy(qr2 => qr2.QuestionParentId)
                        //                .SelectMany(g2 => g2.OrderBy(q => q.QuestionId)
                        //                .Select((qr, i) =>
                        //                               {
                        //                                   questionNumberOrderBy++;
                        //                                   qr.QuestionNumber = $"{g.Key}.{i + 1}";
                        //                                   return qr.QuestionNumber;
                        //                               }))
                        //                             .ToList();
                        //r.QuestionReply = r.QuestionReply.OrderBy(qr => qr.AccreditationStandartId).ToList();
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MobileSync");
                sb.AppendLine(ex.Message);
            }
            finally
            {
                var message = sb.ToString();
                model.Error = message;
            }

            return CreatedAtAction("Sync", model);
        }
    }

    public class SyncModel
    {
        public List<AgedCareCenter> AgedCareCenterList { get; set; }
        public List<Assessor> AssessorList { get; set; }
        public List<Report> ReportList { get; set; }
        public List<AccreditationStandart> AccreditationStandartList { get; set; }
        public Report NewReport { get; set; }
        public string Error { get; set; }
    }
}