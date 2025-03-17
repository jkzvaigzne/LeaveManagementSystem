using LeaveManagementSystem.Web.Models.LeaveAllocations;
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

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? id)
        {
            await _leaveAllocationsService.AllocateLeave(id);
            return RedirectToAction(nameof(Details), new {userId = id});
        }

        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _leaveAllocationsService.GetEmployeeAllocations(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }

            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocationEditVm)
        {
            await _leaveAllocationsService.EditAllocation(allocationEditVm);
            return RedirectToAction(nameof(Details), new { userId = allocationEditVm.Employee.Id });
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
