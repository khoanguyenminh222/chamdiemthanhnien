//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class dm_donVi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dm_donVi()
        {
            this.taiKhoans = new HashSet<taiKhoan>();
            this.giaoChiTieuchoDVs = new HashSet<giaoChiTieuchoDV>();
            this.giaoChiTieuchoDVs1 = new HashSet<giaoChiTieuchoDV>();
            this.giaoChiTieuchoDVs2 = new HashSet<giaoChiTieuchoDV>();
            this.quanHeDonVis = new HashSet<quanHeDonVi>();
            this.quanHeDonVis1 = new HashSet<quanHeDonVi>();
            this.quanHeDonVis2 = new HashSet<quanHeDonVi>();
        }
    
        public int iD { get; set; }
        public int fk_nguoiQuanLy { get; set; }
        public Nullable<bool> cumTruong { get; set; }
        public string dienThoai { get; set; }
        public string diaChi { get; set; }
    
        public virtual nguoiDung nguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<taiKhoan> taiKhoans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<giaoChiTieuchoDV> giaoChiTieuchoDVs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<giaoChiTieuchoDV> giaoChiTieuchoDVs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<giaoChiTieuchoDV> giaoChiTieuchoDVs2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<quanHeDonVi> quanHeDonVis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<quanHeDonVi> quanHeDonVis1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<quanHeDonVi> quanHeDonVis2 { get; set; }
    }
}
