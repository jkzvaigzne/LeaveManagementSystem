using AutoMapper;
using LeaveManagement.Application.Models.LeaveRequests;

namespace LeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestAutoMapperProfile : Profile
    {
        public LeaveRequestAutoMapperProfile()
        {
            CreateMap<LeaveRequestCreateVM, LeaveRequest>();
        }
    }
}
