namespace LeaveManagement.Application.Services.Periods
{
    public interface IPeriodsService
    {
        Task<Period> GetCurrentPeriod();
    }
}
