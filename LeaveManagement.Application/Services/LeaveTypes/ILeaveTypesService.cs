using LeaveManagement.Application.Models.LeaveTypes;

namespace LeaveManagement.Application.Services.LeaveTypes;

public interface ILeaveTypesService
{
    Task<bool> CheckIfLeaveTypeNameExists(string name);
    Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit);
    Task Create(LeaveTypeCreateVM model);
    Task<bool> DaysExceedMaximumForEdit(int leaveTypeId, int days);
    Task Edit(LeaveTypeEditVM model);
    Task<T?> Get<T>(int id) where T : class;
    Task<List<LeaveTypeReadOnlyVM>> GetAll();
    bool LeaveTypeExists(int id);
    Task Remove(int id);
}