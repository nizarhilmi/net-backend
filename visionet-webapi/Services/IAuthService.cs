using visionet_webapi.Common.Command;
using visionet_webapi.Common.Dto;

namespace visionet_webapi.Services
{
    public interface IAuthService
    {
        LoginDto Login(AuthCommand command);
    }
}
