using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index ()
        {
          var employees = await _leaveAllocationsService.GetEmployees();

            if(employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        public async Task<IActionResult> AllocateLeave(string? userId)
        {
            await _leaveAllocationsService.AllocateLeave(userId);
            return RedirectToAction(nameof(Details), new { userId });
        }

        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVM = await _leaveAllocationsService.GetEmployeeAllocations(userId);

            if (employeeVM == null)
            {
                return NotFound(); 
            }

            return View(employeeVM); 
        }
    }
}
