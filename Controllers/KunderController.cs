using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models;
using ResturangtApi_samiharun_net24.Models.Data;

namespace ResturangtApi_samiharun_net24.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KunderController : ControllerBase
    {
        private readonly ResturangDbContext _db;
        public KunderController(ResturangDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Kunder.AsNoTracking().ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var kund = await _db.Kunder.FindAsync(id);
            if (kund == null) return NotFound();
            return Ok(kund);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kunder kund)
        {
            _db.Kunder.Add(kund);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = kund.Id }, kund);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Kunder kund)
        {
            var existing = await _db.Kunder.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = kund.Name;
            existing.Phone = kund.Phone;
            existing.Email = kund.Email;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kund = await _db.Kunder.FindAsync(id);
            if (kund == null) return NotFound();
            _db.Kunder.Remove(kund);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
