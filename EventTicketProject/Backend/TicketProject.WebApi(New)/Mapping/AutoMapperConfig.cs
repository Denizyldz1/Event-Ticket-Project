using AutoMapper;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.CategoryDto;
using TicketProject.DtoLayer.Dtos.CityDto;
using TicketProject.DtoLayer.Dtos.EventDto;
using TicketProject.DtoLayer.Dtos.TicketUser;

namespace TicketProject.WebApi.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AddCategoryDto,Category>().ReverseMap();
            CreateMap<UpdateCategoryDto,Category>().ReverseMap();
            CreateMap<AddCityDto,City>().ReverseMap();
            CreateMap<UpdateCityDto,City>().ReverseMap();
        }
    }
}
