using AutoMapper;
using machine_api.Models.User;

namespace machine_api.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, RegisterModel>();
            CreateMap<RegisterModel, User>();

            CreateMap<User, LoggedUser>();
            CreateMap<LoggedUser, User>();

            CreateMap<User, UpdateModel>();
            CreateMap<UpdateModel, User>();
        }
    }
}
