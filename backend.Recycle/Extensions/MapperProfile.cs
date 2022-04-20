using AutoMapper;
using backend.Recycle.Data.Models;
using backend.Recycle.Data.ViewModels;

namespace backend.Recycle.Extensions
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<RequestEntity, UserRequestModel>().ReverseMap();
        }
    }
}
