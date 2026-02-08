using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models;
using ResturangtApi_samiharun_net24.Models.Data;


// Syftet med en API‑controller:
// Ta emot inkommande HTTP-anrop (GET, POST, PUT, DELETE)
// Validera inkommande data
// Anropa tjänster (t.ex. BokningService) eller databasen
// Returnera rätt JSON‑svar eller felmeddelanden
//
// Exempel: BokningarController:
// GET hämta alla bokningar eller en specifik bokning
// POST skapa en ny bokning
// PUT uppdatera en befintlig bokning
// DELETE ta bort en bokning
//
// Controller fungerar som länken mellan frontend och databasen**.
//
// Viktigt att förstå:
// [HttpGet], [HttpPost], [HttpPut], [HttpDelete] anger 
// vilken typ av HTTP-anrop metoden hanterar.
// IActionResult används för att returnera olika typer av 
// svar: t.ex. Ok, NotFound, BadRequest, Conflict.
// DTOs används för att skicka/ta emot data från klienten.

namespace ResturangtApi_samiharun_net24.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenyController : ControllerBase
    {
        private readonly ResturangDbContext _db;
        public MenyController(ResturangDbContext db) => _db = db;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _db.Meny.AsNoTracking().ToListAsync());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _db.Meny.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Meny item)
        {
            _db.Meny.Add(item);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Meny item)
        {
            var existing = await _db.Meny.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = item.Name;
            existing.Price = item.Price;
            existing.Description = item.Description;
            existing.IsPopular = item.IsPopular;
            existing.BildUrl = item.BildUrl;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Meny.FindAsync(id);
            if (item == null) return NotFound();
            _db.Meny.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
