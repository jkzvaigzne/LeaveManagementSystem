namespace LeaveManagementSystem.Web.Data
{
    public class Period : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
