using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.DataAccess.Entities;
using AutoMapper;

namespace AcademyLMS.BusinessLogic.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();

        CreateMap<StudentDto, Student>()
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        CreateMap<Student, StudentCreateDto>();

        CreateMap<Course, CourseDto>();

        CreateMap<CourseDto, Course>()
            .ForMember(dest => dest.Teacher, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        CreateMap<CourseCreateDto, Course>()
            .ForMember(dest => dest.CourseId, opt => opt.Ignore())
            .ForMember(dest => dest.Teacher, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());

        CreateMap<Course, CourseCreateDto>();
    }
}
