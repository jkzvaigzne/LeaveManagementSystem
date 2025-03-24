using LeaveManagement.Application.Models.LeaveRequests;
using LeaveManagement.Application.Services.LeaveRequests;
using LeaveManagement.Application.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService, ILeaveRequestService _leaveRequestService) : Controller
    {
        // Employee View Request
        public async Task<IActionResult> Index()
        {
            var model = await _leaveRequestService.GetEmployeeLeaveRequests();
            return View(model);
        }

        public async Task<IActionResult> Create(int? leaveTypeId)
        {
            var leaveTypes = await _leaveTypesService.GetAll();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList
            };

            return View(model);
        }
        // Employee Create Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
            if (await _leaveRequestService.RequestDatesExceedAllocation(model))
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation");
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested is invalid.");
            }

            if (ModelState.IsValid)
            {
                await _leaveRequestService.CreateLeaveRequest(model);
                return RedirectToAction(nameof(Index));
            }

            var leaveTypes = await _leaveTypesService.GetAll();
            model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
            return View(model);
        }
        // Employee Cancel Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _leaveRequestService.CancelLeaveRequest(id);
            return RedirectToAction(nameof(Index));
        }

        // Admin/Supe review request
        [Authorize(Policy = "AdminSupervisorOnly")]
        public async Task<IActionResult> ListRequest()
        {
            var model = await _leaveRequestService.AdminGetAllLeaveRequests();
            return View(model);
        }
        // Admin/Supe review request
        public async Task<IActionResult> Review(int id)
        {
            var model = await _leaveRequestService.GetLeaveRequestForReview(id);
            return View(model);
        }

        // Admin/Supe review request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, bool approved)
        {
            await _leaveRequestService.ReviewLeaveRequest(id, approved);
            return RedirectToAction(nameof(ListRequest));
        }
    }
}
