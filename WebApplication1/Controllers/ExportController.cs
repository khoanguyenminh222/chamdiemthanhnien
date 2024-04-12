using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WebApplication1.Controllers
{
    public class ExportController : Controller
    {
        private chamdiemEntities db = new chamdiemEntities();
        public ActionResult ExportToExcel()
        {
            int tinhDoanId = (int)Session["dm_DonVi"]; // Lấy giá trị tinhDoanId từ Session
            IEnumerable<dynamic> totalScores = CalculateTotalScoresByChiDoan(tinhDoanId); // Gọi hàm CalculateTotalScoresByChiDoan
            Console.WriteLine(totalScores);
            
            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Total Scores");

            // Đặt tiêu đề cho các cột
            worksheet.Cells[1, 1].Value = "Chi Đoàn";
            worksheet.Cells[1, 2].Value = "Tổng Điểm Chi Đoàn";
            worksheet.Cells[1, 3].Value = "Tổng Điểm Thành Đoàn";
            worksheet.Cells[1, 4].Value = "Tổng Điểm Tỉnh Đoàn";

            // Đổ dữ liệu từ totalScores vào file Excel
            int row = 2;
            foreach (var item in totalScores)
            {
                worksheet.Cells[row, 1].Value = item.tenChiDoan;
                worksheet.Cells[row, 2].Value = item.TotalScoreChiDoan;
                worksheet.Cells[row, 3].Value = item.TotalScoreThanhDoan;
                worksheet.Cells[row, 4].Value = item.TotalScoreTinhDoan;
                row++;
            }

            // Định dạng lại cột để hiển thị đẹp hơn
            worksheet.Cells["A1:D1"].Style.Font.Bold = true;
            worksheet.Cells["A:D"].AutoFitColumns();
            worksheet.DefaultColWidth = 25;

            // Lưu file Excel và trả về cho client
            byte[] excelBytes = excelPackage.GetAsByteArray();
            string fileName = "TotalScores.xlsx";
            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(excelBytes, mimeType, fileName);
        }
        public IEnumerable<dynamic> CalculateTotalScoresByChiDoan(int tinhDoanId)
        {
            int year = 0;
            if (TempData["year"] != null)
            {
                year = (int)TempData["year"];
                Console.WriteLine(year);
            }
            else
            {
                year = DateTime.Now.Year;
            }
            Console.WriteLine(year);
            var dataDiem = (from chiTieu in db.chiTieux
                            join giaoChiTieu in db.giaoChiTieuchoDVs
                                on chiTieu.iD equals giaoChiTieu.fk_chiTieu
                            join bangdiem in db.bangDiems
                                on giaoChiTieu.id equals bangdiem.fk_giaoChiTieu
                            join nhomChiTeu in db.nhomChiTieux
                                on chiTieu.fk_loaiChiTieu equals nhomChiTeu.iD
                            join loaiTieuChi in db.loaiTieuChis
                                on nhomChiTeu.fk_loaiTieuChi equals loaiTieuChi.iD
                            join dm_donVi in db.dm_donVi
                                on giaoChiTieu.fk_dmDonViChiDoan equals dm_donVi.iD
                            join nguoiDung in db.nguoiDungs
                                on dm_donVi.fk_nguoiQuanLy equals nguoiDung.iD
                            join donVi in db.donVis
                                on nguoiDung.fk_donVi equals donVi.iD
                            select new dataBangDiem()
                            {
                                bangDiem = bangdiem,
                                giaoChiTieuchoDV = giaoChiTieu,
                                loaiTieuChi = loaiTieuChi,
                                nhomChiTieu = nhomChiTeu,
                                chiTieu = chiTieu,
                                dm_DonVi = dm_donVi,
                                nguoiDung = nguoiDung,
                                donVi = donVi,
                            })
                .Where(l => l.loaiTieuChi.nam == year)
                .Where(g => g.giaoChiTieuchoDV.fk_dmDonViTinhDoan == tinhDoanId) // Lọc các chi đoàn thuộc tỉnh đoàn
                .OrderBy(o => o.nhomChiTieu.fk_loaiTieuChi)
                .ThenBy(o => o.chiTieu.iD)
                .ThenBy(b => b.bangDiem.fk_giaoChiTieu)
                .ToList();

            var scores = dataDiem
                                    .GroupBy(item => new { item.giaoChiTieuchoDV.dm_donVi.iD, item.nguoiDung.ten }) // Nhóm các bản ghi theo ID của từng chi đoàn
                                    .Select(group => new
                                    {
                                        ChiDoan = group.Key,
                                        tenChiDoan = group.Key.ten,
                                        TotalScoreChiDoan = group.Sum(item => item.bangDiem.diemChiDoan), // Tính tổng điểm cho mỗi chi đoàn
                                        TotalScoreThanhDoan = group.Sum(item => item.bangDiem.diemThanhDoan),
                                        TotalScoreTinhDoan = group.Sum(item => item.bangDiem.diemTinhDoan)
                                    })
                                    .ToList();

            // Trả về kết quả cho view để hiển thị
            return scores;
        }
    }
}