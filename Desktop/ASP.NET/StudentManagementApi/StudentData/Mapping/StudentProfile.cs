using AutoMapper;
using StudentData.Models;
using StudentData.DTOs;

namespace StudentData.Mapping
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<CreateStudentDTO, Student>();

            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => DateTime.Now.Year - src.BirthDate.Year));
        }
    }
}