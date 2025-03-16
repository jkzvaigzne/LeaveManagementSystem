using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        public IActionResult Index()
        {
            var employeeId = "";
            _leaveAllocationsService.GetAllocations(employeeId);
            return View();
        }
    }
}
