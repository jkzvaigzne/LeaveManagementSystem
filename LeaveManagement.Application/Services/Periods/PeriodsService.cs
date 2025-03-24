using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Application.Services.Periods
{
    public class PeriodsService(ApplicationDbContext _context) : IPeriodsService
    {
        public async Task<Period> GetCurrentPeriod()
        {
            var currentDate = DateTime.Now;
            var period = await _context.Periods
                .Where(q => q.StartDate.Year <= currentDate.Year && q.EndDate.Year >= currentDate.Year)
                .OrderByDescending(q => q.EndDate)
                .FirstOrDefaultAsync();

            return period;
        }
    }
}
