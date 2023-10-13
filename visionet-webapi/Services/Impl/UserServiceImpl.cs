using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using visionet_webapi.Common.Command;
using visionet_webapi.Common.Dto;
using visionet_webapi.Common.Enum;
using visionet_webapi.Exceptions;
using visionet_webapi.Models;
using visionet_webapi.Repository;

namespace visionet_webapi.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly DataContext db;
        private const string fontName = "Tahoma";

        public UserServiceImpl(DataContext db)
        {
            this.db = db;
        }

        public void CreateUser(UserCommand command)
        {
            db.User.Add(new User()
            {
                Username = command.Username,
                Password = command.Password,
                Name = command.Name,
                Email = command.Email,
                Gender = command.Gender,
                DayOfBirthday = command.DayOfBirthday,
                RoleId = command.RoleId,
            });

            db.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = db.User.Find(id);
            if (user == null) 
            {
                throw new NotFoundException("user not found");
            }
            db.User.Remove(user);
            db.SaveChanges();
        }

        public Task<Stream> GetUserReportExcel()
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Activity Report");
            int rowIdx = 0;
            int detailStartRow = 0;

            #region New Style Manage
            IFont headerFont = workbook.CreateFont();
            headerFont.IsBold = true;
            headerFont.FontHeightInPoints = 20;
            headerFont.FontName = fontName;
            IFont normalFont = workbook.CreateFont();
            normalFont.FontHeightInPoints = 12;
            normalFont.FontName = fontName;
            IFont normalFontBold = workbook.CreateFont();
            normalFontBold.IsBold = true;
            normalFontBold.FontHeightInPoints = 12;
            normalFontBold.FontName = fontName;

            ICellStyle borderAllCellStyle = workbook.CreateCellStyle();
            borderAllCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
            borderAllCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            borderAllCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            borderAllCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            borderAllCellStyle.Alignment = HorizontalAlignment.Center;
            borderAllCellStyle.SetFont(normalFont);

            ICellStyle borderSideCellStyle = workbook.CreateCellStyle();
            borderSideCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            borderSideCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            borderSideCellStyle.SetFont(normalFont);
            #endregion

            #region Header Section
            IRow row = sheet.CreateRow(rowIdx);
            ICell cell = row.CreateCell(0);
            row = sheet.CreateRow(rowIdx);
            cell = row.CreateCell(0);
            cell.SetCellValue("No");
            cell.CellStyle = borderAllCellStyle;
            cell = row.CreateCell(1);
            cell.SetCellValue("Username");
            cell.CellStyle = borderAllCellStyle;
            cell = row.CreateCell(2);
            cell.SetCellValue("Name");
            cell.CellStyle = borderAllCellStyle;
            cell = row.CreateCell(3);
            cell.SetCellValue("Email");
            cell.CellStyle = borderAllCellStyle;
            cell = row.CreateCell(4);
            cell.SetCellValue("Gender");
            cell.CellStyle = borderAllCellStyle;
            cell = row.CreateCell(5);
            cell.SetCellValue("DOB");
            cell.CellStyle = borderAllCellStyle;
            rowIdx++;
            #endregion

            var datas = GetUsers();
            foreach (var data in datas)
            {
                row = sheet.CreateRow(rowIdx);
                cell = row.CreateCell(0);
                cell.SetCellValue($"{(rowIdx - 1) + 1}");
                cell.CellStyle = borderSideCellStyle;
                cell = row.CreateCell(1);
                cell.SetCellValue($"{data.Username ?? null}");
                cell.CellStyle = borderSideCellStyle;
                cell = row.CreateCell(2);
                cell.SetCellValue($"{data.Name ?? null}");
                cell.CellStyle = borderSideCellStyle;
                cell = row.CreateCell(3);
                cell.SetCellValue($"{data.Email ?? null}");
                cell.CellStyle = borderSideCellStyle;
                cell = row.CreateCell(4);
                cell.SetCellValue($"{data.Gender ?? null}");
                cell.CellStyle = borderSideCellStyle;
                cell = row.CreateCell(5);
                cell.SetCellValue($"{data.DayOfBirthday ?? null}");
                cell.CellStyle = borderSideCellStyle;
                rowIdx++;
            }

            #region Misc & Gimmic
            for (int i = 0; i <= 6; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            #endregion

            Stream s = new MemoryStream();

            using (MemoryStream tempStream = new MemoryStream())
            {
                workbook.Write(tempStream);
                var byteArray = tempStream.ToArray();
                s.Write(byteArray, 0, byteArray.Length);
            }

            return Task.FromResult(s);
        }
        

        public IList<UserDto> GetUsers()
        {
            IList<UserDto> users = new List<UserDto>();
            var datas = db.User.Include(x => x.Role).ToList();
            foreach ( var data in datas )
            {
                users.Add(new UserDto()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Username = data.Username,
                    Email = data.Email,
                    Gender = data.Gender,
                    DayOfBirthday= data.DayOfBirthday.ToString("yyyy-MM-dd"),
                });
            }
            return users;
        }

        public byte[] GetUserReportPDF()
        {
            Document doc = new Document();
            MemoryStream stream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            Paragraph title = new Paragraph("User Report");
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title); 

            PdfPTable table = new PdfPTable(6);

            float[] columnWidths = { 4f, 8f, 10f, 15f, 8f, 10f };
            table.SetTotalWidth(columnWidths);
            table.SetWidths(columnWidths);

            table.AddCell("No");
            table.AddCell("Username");
            table.AddCell("Name");
            table.AddCell("Email");
            table.AddCell("Gender");
            table.AddCell("DOB");

            var datas = GetUsers();
            int rowNumber = 1;

            foreach (var data in datas)
            {
                table.AddCell(rowNumber.ToString());
                table.AddCell(data.Username ?? "");
                table.AddCell(data.Name ?? "");
                table.AddCell(data.Email ?? "");
                table.AddCell(data.Gender ?? "");
                table.AddCell(data.DayOfBirthday ?? "");
                table.SpacingAfter = 45f;
                table.SpacingBefore = 10f;
                rowNumber++;
            }

            doc.Add(table);

            doc.Close();

            return stream.ToArray();
        }
    }
}
