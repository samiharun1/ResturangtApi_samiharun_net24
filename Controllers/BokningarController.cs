using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models;
using ResturangtApi_samiharun_net24.Models.Data;
using ResturangtApi_samiharun_net24.Models.Dtos;
using ResturangtApi_samiharun_net24.Models.Services;
using static ResturangtApi_samiharun_net24.Models.Dtos.BokningDtos;

namespace ResturangtApi_samiharun_net24.Controllers
{

    // Route: api/Bokningar
    // Denna controller hanterar alla bokningar i systemet
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BokningarController : ControllerBase
    {
        // Databaskontext för att kommunicera med databasen
        private readonly ResturangDbContext _db;
        // Service för att hantera bokningslogik, t.ex. kontrollera tillgänglighet
        private readonly BokningService _bokningService;

        // Dependency Injection  vi får DbContext och Service automatiskt
        public BokningarController(ResturangDbContext db, BokningService bokningService)
        {
            _db = db;
            _bokningService = bokningService;
        }

        // GET: api/Bokningar
        // Hämtar alla bokningar inklusive bord och kund
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bokningar = await _db.Bokningar
                .Include(b => b.Bord)
                .Include(b => b.Kunder)
                .Select(b => new BokningReadDto
                {
                    Id = b.Id,
                    BordId = b.BordId,
                    BordNummer = b.Bord.BordNummer.ToString(),
                    KundId = b.KundId,
                    KundNamn = b.Kunder.Name,
                    KundTelefon = b.Kunder.Phone,
                    StartTime = b.StartTime,
                    AntalGaster = b.AntalGaster
                })
                .ToListAsync();

            return Ok(bokningar);
        }

        // GET: api/Bokningar/{id}
        // Hämtar en specifik bokning via id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBokning(int id)
        {
            var b = await _db.Bokningar
                .Include(b => b.Bord)
                .Include(b => b.Kunder)
                .Where(b => b.Id == id)
                .Select(b => new BokningReadDto
                {
                    Id = b.Id,
                    BordId = b.BordId,
                    BordNummer = b.Bord.BordNummer.ToString(),
                    KundId = b.KundId,
                    KundNamn = b.Kunder.Name,
                    KundTelefon = b.Kunder.Phone,
                    StartTime = b.StartTime,
                    AntalGaster = b.AntalGaster
                })
                .FirstOrDefaultAsync();

            // Om bokningen inte finns
            if (b == null) return NotFound();
            return Ok(b);
        }

        // POST: api/Bokningar
        // Skapar en ny bokning
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            // 1. Kontrollera att bordet finns
            var bord = await _db.Bord.FindAsync(dto.BordId);
            if (bord == null) return NotFound("Bord finns inte");

            // 2. Kontrollera att antalet gäster får plats
            if (dto.AntalGaster > bord.Kapacitet) return BadRequest("För många gäster för bordet");

            // 3. Kontrollera att bordet är ledigt (2-timmarsregeln)
            var available = await _bokningService.IsTableAvailableAsync(dto.BordId, dto.StartTime);
            if (!available) return Conflict("Bordet är upptaget");

            int kundId;

            // Hantera kund
            if (dto.KundId.HasValue)
            {
                var exists = await _db.Kunder.AnyAsync(k => k.Id == dto.KundId.Value);
                if (!exists) return BadRequest("KundId ogiltigt");
                kundId = dto.KundId.Value;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dto.KundNamn) || string.IsNullOrWhiteSpace(dto.KundTelefon))
                    return BadRequest("Ange kundnamn och telefon eller KundId");

                var nyKund = new Kunder
                {
                    Name = dto.KundNamn!,
                    Phone = dto.KundTelefon!
                };
                _db.Kunder.Add(nyKund);
                await _db.SaveChangesAsync();
                kundId = nyKund.Id;
            }

            // Skapa bokning
            var bokning = new Bokning
            {
                BordId = dto.BordId,
                KundId = kundId,
                StartTime = dto.StartTime,
                AntalGaster = dto.AntalGaster
            };

            _db.Bokningar.Add(bokning);
            await _db.SaveChangesAsync();

            // Returnera objekt som MVC kan läsa
            return CreatedAtAction(nameof(GetBokning), new { id = bokning.Id }, new
            {
                bokning.Id,
                bokning.BordId,
                BordNummer = bord.BordNummer,
                bokning.KundId,
                KundNamn = dto.KundNamn ?? (await _db.Kunder.FindAsync(kundId))!.Name,
                KundTelefon = dto.KundTelefon ?? (await _db.Kunder.FindAsync(kundId))!.Phone,
                bokning.StartTime,
                bokning.AntalGaster
            });
        }

        // DELETE ta bort bokning
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Bokningar.FindAsync(id);
            if (existing == null) return NotFound();

            _db.Bokningar.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
