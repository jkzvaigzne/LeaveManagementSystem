﻿using AutoMapper;
using LeaveManagement.Application.Models.LeaveAllocations;
using LeaveManagement.Application.Models.LeaveRequests;
using LeaveManagement.Application.Services.LeaveAllocations;
using LeaveManagement.Application.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Application.Services.LeaveRequests
{
    public class LeaveRequestService(IMapper _mapper,
    IUserService _userService,
    ApplicationDbContext _context,
    ILeaveAllocationsService _leaveAllocationService) : ILeaveRequestService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;

            await UpdateAllocationDays(leaveRequest, false);
            await _context.SaveChangesAsync();
        }
        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .ToListAsync();

            var aprovedLeaveRequestsCount = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved);
            var pendingLeaveRequestsCount = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending);
            var declinedLeaveRequestsCount = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined);

            var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType?.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = aprovedLeaveRequestsCount,
                PendingRequests = pendingLeaveRequestsCount,
                DeclinedRequests = declinedLeaveRequestsCount,
                TotalRequests = leaveRequests.Count,
                LeaveRequests = leaveRequestModels
            };

            return model;
        }
        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;

            _context.Add(leaveRequest);

            await UpdateAllocationDays(leaveRequest, true);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser();
            var leaveRequests = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType?.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
            }).ToList();

            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userService.GetLoggedInUser();
            var currentDate = DateTime.Now;
            var period = await _context.Periods
                .Where(q => q.EndDate.Year == currentDate.Year)
                .OrderByDescending(q => q.EndDate)
                .FirstOrDefaultAsync();

            if (period == null)
            {
                throw new Exception("No period found for the current year.");
            }
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            var allocation = await _context.LeaveAllocation.FirstOrDefaultAsync(q => q.LeaveTypeId
            == model.LeaveTypeId
            && q.EmployeeId == user.Id
            && q.PeriodId == period.Id
            );

            if (allocation == null)
            {
                return false;
            }
            return allocation.Days < numberOfDays;
        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userService.GetLoggedInUser();
            var currentDate = DateTime.Now;

            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = approved
                ? (int)LeaveRequestStatusEnum.Approved
                : (int)LeaveRequestStatusEnum.Declined;

            leaveRequest.ReviewerId = user.Id;

            if (!approved)
            {
                await UpdateAllocationDays(leaveRequest, false);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == id);

            var user = await _userService.GetUserById(leaveRequest.Employee.Id);

            var model = new ReviewLeaveRequestVM
            {
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.Employee.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };

            return model;
        }

        public async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationService.GetCurrentAllocation(
                leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);

            if (allocation == null)
            {
                throw new InvalidOperationException(InvalidOperationExceptionHelper.NoLeaveAllocationFoundForGivenTypeEmployee);
            }

            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

            if (deductDays)
            {
                allocation.Days -= numberOfDays;
            }
            else
            {
                allocation.Days += numberOfDays;
            }

            _context.Entry(allocation).State = EntityState.Modified;
        }


        public int CalculateDays(DateOnly start, DateOnly end)
        {
            return end.DayNumber - start.DayNumber;
        }
    }
}

