use master
go
create database chamdiem
go
use chamdiem
go
create table loaiTieuChi
(
	iD int not null IDENTITY(1,1) primary key,
	ten nvarchar(100) not null,
	tongDiem int not null,
	nam int default Year(getDate())
)
go
ALTER TABLE loaiTieuChi
add nam int default Year(getDate())
create table nhomChiTieu
(
	iD int not null IDENTITY(1,1) primary key,
	ten nvarchar(200) not null,
	tongDiem int not null,
	fk_loaiTieuChi int,
	foreign key (fk_loaiTieuChi) references loaiTieuChi(iD)
)
go
create table chiTieu
(
	iD int not null IDENTITY(1,1) primary key,
	ten nvarchar(4000) not null,
	ycDanhGiaKQ nvarchar(1000),
	fk_loaiChiTieu int,
	foreign key (fk_loaiChiTieu) references nhomChiTieu(iD)
)
go
create table chiTietChiTieu
(
	iD int not null IDENTITY(1,1) primary key,
	noiDung nvarchar(4000) not null,
	diem int not null,
	fk_loaiChiTieu int,
	foreign key (fk_loaiChiTieu) references chiTieu(iD)
)
go
create table donVi
(
	iD int not null IDENTITY(1,1) primary key,
	ten nvarchar(100) not null,
)
go
create table nguoiDung
(
	iD int not null IDENTITY(1,1) primary key,
	ten nvarchar(100) not null,
	fk_donVi int not null,
	chucVu nvarchar(100) not null,
	dienThoai varchar(12),
	diaChi nvarchar(200),
	foreign key (fk_donVi) references donVi(iD)
)
go
create table dm_donVi
(
	iD int not null IDENTITY(1,1) primary key,
	fk_nguoiQuanLy int not null,
	cumTruong bit,
	dienThoai varchar(12),
	diaChi nvarchar(200),
	foreign key (fk_nguoiQuanLy) references nguoiDung(iD)
)
go
create table taiKhoan 
(
	iD int not null IDENTITY(1,1) primary key,
	ten varchar(50) not null,
	matKhau varchar(50) not null,
	fk_dmDonVi int,
	foreign key (fk_dmDonVi) references dm_donVi(iD)
)
go
create table giaoChiTieuchoDV
(
	id int not null IDENTITY(1,1) primary key,
	fk_chiTieu int,
	fk_dmDonViChiDoan int,
	fk_dmDonViThanhDoan int,
	fk_dmDonViTinhDoan int,
	foreign key (fk_chiTieu) references chiTieu(iD),
	foreign key (fk_dmDonViChiDoan) references dm_donVi(iD),
	foreign key (fk_dmDonViThanhDoan) references dm_donVi(iD),
	foreign key (fk_dmDonViTinhDoan) references dm_donVi(iD)
)
go
create table bangDiem
(
	id int not null IDENTITY(1,1) primary key,
	fk_giaoChiTieu int,
	diemCoDinh int,
	diemChiDoan int default 0,
	diemThanhDoan int default 0,
	diemTinhDoan int default 0,
	ycMinhChung nvarchar(1000),
	thoiGian Datetime,
	yKienPhanHoi nvarchar(1000),
	banPhuTrach nvarchar(50),
	hinhAnh image,
	trangThai int default 0,--0: chưa đánh giá, 1: chi đoàn đã chấm, 2: thành đoàn đã chấm, 3 tỉnh đoàn đã chấm
	foreign key (fk_giaoChiTieu) references giaoChiTieuchoDV(id),
)
go

create table quanHeDonVi
(
	iD int not null IDENTITY(1,1) primary key,
	chiDoan int,
	thanhDoan int,
	tinhDoan int,
	foreign key (chiDoan) references dm_donVi(iD),
	foreign key (thanhDoan) references dm_donVi(iD),
	foreign key (tinhDoan) references dm_donVi(iD)
)
go
insert into loaiTieuChi(ten, tongDiem)
values (N'Tiêu chí 1: Công tác giáo dục', 40)

insert into nhomChiTieu(ten, tongDiem, fk_loaiTieuChi)
values (N'Nội dung đánh giá: Học tập và làm theo tư tưởng, đạo đức, phong cách Hồ Chí Minh', 8, 1)

insert into chiTieu(ten, fk_loaiChiTieu)
values (N'Các đơn vị thuộc Cụm thi đua tổ chức học tập, quán triệt, tuyên truyền tối thiểu 02 chuyên đề học tập và làm theo tư tưởng, đạo đức, phong cách Hồ Chí Minh năm 2023 theo phương cách dễ hiểu, dễ nhớ, trực quan, sinh động.'
	, 1)

insert into chiTietChiTieu(noiDung, diem, fk_loaiChiTieu)
values (N'Đủ 2 chuyên đề.', 4, 1)

insert into chiTietChiTieu(noiDung, diem, fk_loaiChiTieu)
values (N' 01 chuyên đề', 2, 1)

insert into chiTietChiTieu(noiDung, diem, fk_loaiChiTieu)
values (N' Không đủ chuyên đề', 0, 1)
