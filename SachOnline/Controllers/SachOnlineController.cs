using SachOnline.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        QLBANSACHEntities3 db = new QLBANSACHEntities3();
        // GET: SachOnline
        public ActionResult Index()
        {
            var lsSach = db.SACHes.OrderByDescending(s => s.NgayCapNhat).Take(6).ToList();
            return View(lsSach);
        }
        public ActionResult ChuDeParital()
        {
            return View();
        }
        public ActionResult NXBParital()
        {
            return View();
        }
        public ActionResult Chitietsach(int id)
        {
            var sach = db.SACHes.FirstOrDefault(s => s.MaSach == id);

            return View(sach);
        }
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            // Lấy giá trị từ form đăng ký
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TaiKhoan"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNhapLai"];
            var sDiaChiKH = collection["DiaChiKH"];
            var sEmail = collection["Email"];
            var sDienThoaiKH = collection["DienThoaiKH"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            // Kiểm tra giá trị nhập vào
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoaiKH))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                // Gán giá trị cho đối tượng khách hàng
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChiKH= sDiaChiKH;
                kh.DienThoaiKH = sDienThoaiKH;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);

                // Thêm khách hàng mới vào database
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();

                return RedirectToAction("DangNhap", "SachOnline");
            }

            return this.DangKy();
        }


    }
}