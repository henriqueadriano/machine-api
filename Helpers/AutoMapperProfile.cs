using AutoMapper;
using machine_api.Models.User;

namespace machine_api.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>();
        }
    }
}
