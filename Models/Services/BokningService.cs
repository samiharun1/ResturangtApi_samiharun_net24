using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models.Data;

namespace ResturangtApi_samiharun_net24.Models.Services
{
    public class BokningService
    {
        private readonly ResturangDbContext _db;
        public BokningService(ResturangDbContext db) => _db = db;

        // Kollar om ett bord är ledigt för en starttid (2h)
        public async Task<bool> IsTableAvailableAsync(int bordId, DateTime start)
        {
            var end = start.AddHours(2);
            return !await _db.Bokningar
                .AsNoTracking()
                .AnyAsync(b => b.BordId == bordId && b.StartTime < end && start < b.StartTime.AddHours(2));
        }

        // Hämta lediga bord för starttid och antal gäster
        public async Task<List<int>> GetAvailableTableIdsAsync(DateTime start, int antalGaster)
        {
            var end = start.AddHours(2);
            var q = _db.Bord
                .AsNoTracking()
                .Where(t => t.Kapacitet >= antalGaster)
                .Where(t => !_db.Bokningar.Any(b => b.BordId == t.Id && b.StartTime < end && start < b.StartTime.AddHours(2)))
                .OrderBy(t => t.Kapacitet);

            return await q.Select(t => t.Id).ToListAsync();
        }
    }
}
    
