using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHS.Models;

namespace QLHS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewFeedsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public NewFeedsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/NewFeeds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewFeed>>> GetNewFeeds()
        {
            return await _context.NewFeeds.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        // GET: api/NewFeeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewFeed>> GetNewFeed(int id)
        {
            var newFeed = await _context.NewFeeds.FindAsync(id);

            if (newFeed == null)
            {
                return NotFound();
            }

            return newFeed;
        }

        // PUT: api/NewFeeds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewFeed(int id, NewFeed newFeed)
        {
            if (id != newFeed.Id)
            {
                return BadRequest();
            }

            _context.Entry(newFeed).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewFeedExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNewFeed", new { id = newFeed.Id }, newFeed);
        }

        // POST: api/NewFeeds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewFeed>> PostNewFeed(NewFeed newFeed)
        {
            _context.NewFeeds.Add(newFeed);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNewFeed", new { id = newFeed.Id }, newFeed);
        }

        // DELETE: api/NewFeeds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewFeed(int id)
        {
            var newFeed = await _context.NewFeeds.FindAsync(id);
            if (newFeed == null)
            {
                return NotFound();
            }

            _context.NewFeeds.Remove(newFeed);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewFeedExists(int id)
        {
            return _context.NewFeeds.Any(e => e.Id == id);
        }
    }
}
