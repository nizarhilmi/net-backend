using visionet_webapi.Common.Command;
using visionet_webapi.Common.Dto;
using visionet_webapi.Common.Enum;

namespace visionet_webapi.Services
{
    public interface IUserService
    {
        void CreateUser(UserCommand command);
        IList<UserDto> GetUsers();
        void DeleteUser(int id);
        Task<Stream> GetUserReportExcel();
        byte[] GetUserReportPDF();
    }
}
