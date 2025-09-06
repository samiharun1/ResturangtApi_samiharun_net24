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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BokningarController : ControllerBase
    {
        private readonly ResturangDbContext _db;
        private readonly BokningService _bokningService;

        public BokningarController(ResturangDbContext db, BokningService bokningService)
        {
            _db = db;
            _bokningService = bokningService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var bord = await _db.Bord.FindAsync(dto.BordId);
            if (bord == null) return NotFound("Bord finns inte");
            if (dto.AntalGaster > bord.Kapacitet) return BadRequest("För många gäster för bordet");

            var available = await _bokningService.IsTableAvailableAsync(dto.BordId, dto.StartTime);
            if (!available) return Conflict("Bordet är upptaget");

            int kundId;
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

                var nyKund = new Kunder { Name = dto.KundNamn!, Phone = dto.KundTelefon! };
                _db.Kunder.Add(nyKund);
                await _db.SaveChangesAsync();
                kundId = nyKund.Id;
            }

            var bokning = new Bokning
            {
                BordId = dto.BordId,
                KundId = kundId,
                StartTime = dto.StartTime,
                AntalGaster = dto.AntalGaster
            };
            _db.Bokningar.Add(bokning);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBokning), new { id = bokning.Id }, bokning);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBokning(int id)
        {
            var bokning = await _db.Bokningar
                .Include(b => b.Bord)
                .Include(b => b.Kunder)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bokning == null) return NotFound();
            return Ok(bokning);
        }
    }
}


