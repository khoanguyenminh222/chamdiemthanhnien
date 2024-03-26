using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class tieuChi_nguoiDung
    {
        public List<dm_donVi> dm_DonVi { get; set; }
        public nguoiDung nguoiDung { get; set; }
        public donVi donVi { get; set; }
        public loaiTieuChi loaiTieuChi { get; set; }
    }
}