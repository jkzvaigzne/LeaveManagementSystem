using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var leaveAllocations = await _leaveAllocationsService.GetAllocations();
            return View();
        }
    }
}
