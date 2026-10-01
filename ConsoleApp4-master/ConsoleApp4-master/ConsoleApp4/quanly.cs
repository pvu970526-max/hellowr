using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSpeed
{
    public class QuanLyPhuongTien
    {
        private List<PhuongTien> danhSach;

        public QuanLyPhuongTien()
        {
            danhSach = new List<PhuongTien>();
        }

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null)
                throw new ArgumentNullException(nameof(pt));

            danhSach.Add(pt);
        }

        public void DisplayAll()
        {
            Console.WriteLine("\n===== DANH SÁCH PHƯƠNG TIỆN =====");

            foreach (PhuongTien pt in danhSach)
            {
                Console.WriteLine(pt.GetInfo());
                Console.WriteLine(
                    $"Giá lăn bánh: {pt.TinhGiaLanBanh():N0} VNĐ");
                Console.WriteLine("--------------------------------");
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (danhSach.Count == 0)
                return null;

            return danhSach
                .OrderByDescending(pt => pt.TinhGiaLanBanh())
                .First();
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return danhSach
                .Where(pt => pt.TenHang.Contains(
                    keyword.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}