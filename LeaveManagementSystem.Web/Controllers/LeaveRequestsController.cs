using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService) : Controller
    {
        // Employee View Request
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var leaveTypes = await _leaveTypesService.GetAll();
            var leaveTypesList = new SelectList(leaveTypes, "Id","Name");
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
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
            return View();
        }
        // Employee Cancel Request
        [HttpPost]
        public async Task<IActionResult> Cancel(int leaveRequestId)
        {
            return View();
        }

        // Admin/Supe review request
        public async Task<IActionResult> ListRequest()
        {
            return View();
        }

        public async Task<IActionResult> Review(int leaveRequestId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Review()
        {
            return View();
        }
    }
}
