using Microsoft.AspNetCore.Mvc;
using visionet_webapi.Common.Command;
using visionet_webapi.Common.Dto;
using visionet_webapi.Common.Enum;
using visionet_webapi.Controllers.Attributes;
using visionet_webapi.Services;

namespace visionet_webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [AuthorizeRole(PrivilegeType.CreateUser)]
        [HttpPost]
        public void CreateUser([FromBody] UserCommand command)
        {
            userService.CreateUser(command);
        }

        [AuthorizeRole(PrivilegeType.ReadUser)]
        [HttpGet]
        public IList<UserDto> GetUsers()
        {
            return userService.GetUsers();
        }

        [AuthorizeRole(PrivilegeType.DeleteUser)]
        [HttpDelete]
        [Route("{id}")]
        public void DeleteUser(int id)
        {
            userService.DeleteUser(id);
        }

        [AuthorizeRole(PrivilegeType.Report)]
        [HttpGet]
        [Route("report/excel")]
        public IActionResult GetUserReportExcel()
        {
            Stream stream = userService.GetUserReportExcel().GetAwaiter().GetResult();
            string dateTime = DateTime.Now.ToString("ddMMyyyy");
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"User_Report_{dateTime}.xlsx");
        }

        [AuthorizeRole(PrivilegeType.Report)]
        [HttpGet]
        [Route("report/pdf")]
        public IActionResult GetUserReportPDF()
        {
            byte[] pdfData = userService.GetUserReportPDF();
            string dateTime = DateTime.Now.ToString("ddMMyyyy");
            return File(pdfData, "application/pdf", $"User_Report_{dateTime}.pdf");
        }
    }
}
