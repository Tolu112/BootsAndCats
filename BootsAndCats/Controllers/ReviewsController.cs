using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BootsAndCats;
using BootsAndCats.DTOs;

namespace BootsAndCats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly BootsAndCatsContext _context;

        public ReviewsController(BootsAndCatsContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            //if (_context.Reviews == null)
            //{
            //    return Problem("Entity set 'BootsAndCatsContext.Reviews'  is null.");
            //}
            //  _context.Reviews.Add(review);
            //  await _context.SaveChangesAsync();

            //  return CreatedAtAction("GetReview", new { id = review.Id }, review);

                var rev = new Review
                {
                    UserId = review.UserId,
                    AlbumId = review.AlbumId,
                    Rating = review.Rating,
                    ReviewContent = review.ReviewContent,
                    Recommendation = review.Recommendation,
                    Tag = review.Tag,
                    DateSubmitted = DateTime.Now,
                };

            var result = _context.Users.Include("Reviews")
                                    .FirstOrDefault(u => u.Id == rev.UserId);
            result.Reviews.Add(rev);
            //_context.Reviews.Add(rev);
            if (await _context.SaveChangesAsync() == 1) return Ok();

            return StatusCode(500);
            
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
