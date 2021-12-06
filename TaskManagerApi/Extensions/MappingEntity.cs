using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.HelperModels;
using TaskManagerApi.DTO.ResponseModels;

namespace TaskManagerApi.Extensions
{
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            CreateMap<Domain.Models.Task, TaskVM>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                .ForMember(dest => dest.Deadline, opts => opts.MapFrom(src => src.Deadline))
                .ForMember(dest => dest.StatusId, opts => opts.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.UserToTasks.Select(x => new UserShortInfo
                {
                    Id = x.UserId,
                    Name = x.User.Name,
                    Surname = x.User.Surname
                }).ToList()));

            CreateMap<User, UserOrganization>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opts => opts.MapFrom(src => src.Surname))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.OrganizationName, opts => opts.MapFrom(src => src.Organization.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Organization.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Organization.Address));
        }
    }
}
