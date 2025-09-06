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
    public class BordController : ControllerBase
    {
        private readonly ResturangDbContext _db;

        public BordController(ResturangDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Bord.AsNoTracking().ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bord = await _db.Bord.FindAsync(id);
            if (bord == null) return NotFound();
            return Ok(bord);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bord bord)
        {
            _db.Bord.Add(bord);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = bord.Id }, bord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Bord bord)
        {
            var existing = await _db.Bord.FindAsync(id);
            if (existing == null) return NotFound();
            existing.BordNummer = bord.BordNummer;
            existing.Kapacitet = bord.Kapacitet;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bord = await _db.Bord.FindAsync(id);
            if (bord == null) return NotFound();
            _db.Bord.Remove(bord);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

