using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class bangDiemManager
    {
        private readonly List<dataBangDiem> _dataBangDiems;

        public bangDiemManager(List<dataBangDiem> dataBangDiems)
        {
            _dataBangDiems = dataBangDiems;
        }

        // Hàm tính tổng điểm của các chi đoàn thuộc tỉnh đoàn
        public IEnumerable<dynamic> CalculateTotalScoresByTinhDoan(int tinhDoanId)
        {
            return _dataBangDiems.Where(item => item.giaoChiTieuchoDV.dm_donVi.quanHeDonVis.Any(qh => qh.tinhDoan == tinhDoanId)) // Lọc các chi đoàn thuộc tỉnh đoàn
                                 .GroupBy(item => item.giaoChiTieuchoDV.dm_donVi.iD) // Group theo ID của từng chi đoàn
                                 .Select(group => new
                                 {
                                     ChiDoan = group.Key,
                                     TotalScore = group.Sum(item => item.bangDiem.diemChiDoan)
                                 });
        }
    }
}