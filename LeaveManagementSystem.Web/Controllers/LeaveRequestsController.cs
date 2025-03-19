using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        // Employee View Request
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
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
