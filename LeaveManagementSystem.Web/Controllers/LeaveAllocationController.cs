using LeaveManagementSystem.Web.Services.LeaveAllocations;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        public async Task<IActionResult> Details()
        {
            var employeeVM = await _leaveAllocationsService.GetEmployeeAllocations();

            if (employeeVM == null)
            {
                return NotFound(); 
            }

            return View(employeeVM); 
        }
    }
}
