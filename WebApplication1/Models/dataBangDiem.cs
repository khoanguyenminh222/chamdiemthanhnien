using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class dataBangDiem
    {
        public bangDiem bangDiem { get; set; }
        public giaoChiTieuchoDV giaoChiTieuchoDV { get; set; }
        public loaiTieuChi loaiTieuChi { get; set; }
        public nhomChiTieu nhomChiTieu { get; set; }
        public chiTieu chiTieu { get; set; }
        public chiTietChiTieu chiTietChiTieu { get; set; }
        public dm_donVi dm_DonVi { get; set; }
        public nguoiDung nguoiDung { get; set; }
        public donVi donVi { get; set; }

    }
}