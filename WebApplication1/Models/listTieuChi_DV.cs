using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class listTieuChi_DV
    {
        public List<loaiTieuChi> loaiTieuChis { get; set; }
        public List<dm_donVi> dm_DonVis { get; set; }
    }
}