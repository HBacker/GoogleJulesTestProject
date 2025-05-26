using AutoMapper;
using EDMN_EgitimPortali_WebAPI.DTOs;
using EDMN_EgitimPortali_WebAPI.DTOs.Authentication;
using EDMN_EgitimPortali_WebAPI.DTOs.Comments;
using EDMN_EgitimPortali_WebAPI.DTOs.Lessons;
using EDMN_EgitimPortali_WebAPI.DTOs.WatchedContents;
using EDMN_EgitimPortali_WebAPI.Models;

namespace EDMN_EgitimPortali_WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                    src.CourseCategories.Select(cc => cc.CategoryId).ToList()))
                .ReverseMap();

            CreateMap<Lesson, LessonDTO>().ReverseMap();
            CreateMap<Course, CourseCreateDTO>().ReverseMap();
            CreateMap<Course, CourseUpdateDTO>().ReverseMap();
            CreateMap<Lesson, LessonCreateDTO>().ReverseMap();

            CreateMap<ApplicationUser, CurrentUserResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CommentCreateDTO, Comment>();

            CreateMap<WatchedContent, WatchedContentDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();

            CreateMap<CourseCreateDTO, Course>()
                .ForMember(dest => dest.CourseCategories, opt => opt.MapFrom(src =>
                    src.CategoryIds.Select(id => new CourseCategory
                    {
                        CategoryId = id
                    }).ToList()));

            CreateMap<CourseUpdateDTO, Course>()
                .ForMember(dest => dest.CourseCategories, opt => opt.MapFrom(src =>
                    src.CategoryIds.Select(id => new CourseCategory
                    {
                        CategoryId = id
                    }).ToList()));
        }
    }
}
