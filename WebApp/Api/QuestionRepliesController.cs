using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Api
{
    [Produces("application/json")]
    [Route("api/QuestionReplies")]
    public class QuestionRepliesController : Controller
    {
        private readonly AACCContext _context;

        public QuestionRepliesController(AACCContext context)
        {
            _context = context;
        }

        // GET: api/QuestionReplies
        [HttpGet]
        public IEnumerable<QuestionReply> GetQuestionReplies()
        {
            return _context.QuestionReplies;
        }

        // GET: api/QuestionReplies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionReply([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionReply = await _context.QuestionReplies.SingleOrDefaultAsync(m => m.QuestionReplyId == id);

            if (questionReply == null)
            {
                return NotFound();
            }

            return Ok(questionReply);
        }

        // PUT: api/QuestionReplies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionReply([FromRoute] int id, [FromBody] QuestionReply questionReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionReply.QuestionReplyId)
            {
                return BadRequest();
            }

            _context.Entry(questionReply).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionReplyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/QuestionReplies
        [HttpPost]
        public async Task<IActionResult> PostQuestionReply([FromBody] QuestionReply questionReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.QuestionReplies.Add(questionReply);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionReply", new { id = questionReply.QuestionReplyId }, questionReply);
        }

        // DELETE: api/QuestionReplies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionReply([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionReply = await _context.QuestionReplies.SingleOrDefaultAsync(m => m.QuestionReplyId == id);
            if (questionReply == null)
            {
                return NotFound();
            }

            _context.QuestionReplies.Remove(questionReply);
            await _context.SaveChangesAsync();

            return Ok(questionReply);
        }

        private bool QuestionReplyExists(int id)
        {
            return _context.QuestionReplies.Any(e => e.QuestionReplyId == id);
        }
    }
}