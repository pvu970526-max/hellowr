using System;
using System.Collections.Generic;

namespace AutoSpeed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            Console.WriteLine("======================================");
            Console.WriteLine("     QUẢN LÝ PHƯƠNG TIỆN AUTOSPEED");
            Console.WriteLine("======================================");


            // TC01 - KIỂM TRA VALIDATION NĂM SẢN XUẤT

            Console.WriteLine("\n===== TC01 =====");

            try
            {
                OTo otoLoi = new OTo(
                    "OT999",
                    "Toyota",
                    1850,
                    1000000000m,
                    5,
                    2.0);

                Console.WriteLine("FAIL");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("PASS");
                Console.WriteLine(ex.Message);
            }


            // TC02 - Ô TÔ 5 CHỖ

            Console.WriteLine("\n===== TC02 =====");

            OTo oto = new OTo(
                "OT001",
                "Toyota",
                2024,
                1000000000m,
                5,
                2.0);

            Console.WriteLine(oto.GetInfo());

            decimal giaOto = oto.TinhGiaLanBanh();

            Console.WriteLine(
                $"Giá lăn bánh: {giaOto:N0} VNĐ");

            if (giaOto == 1420000000m)
                Console.WriteLine("TC02: PASS");
            else
                Console.WriteLine("TC02: FAIL");


            // TC03 - XE MÁY 150CC

            Console.WriteLine("\n===== TC03 =====");

            XeMay xeMay = new XeMay(
                "XM001",
                "Honda",
                2023,
                50000000m,
                150);

            Console.WriteLine(xeMay.GetInfo());

            decimal giaXeMay =
                xeMay.TinhGiaLanBanh();

            Console.WriteLine(
                $"Giá lăn bánh: {giaXeMay:N0} VNĐ");

            if (giaXeMay == 51000000m)
                Console.WriteLine("TC03: PASS");
            else
                Console.WriteLine("TC03: FAIL");

            // TC04 - KIỂM TRA ĐA HÌNH

            Console.WriteLine("\n===== TC04 =====");

            List<PhuongTien> danhSach =
                new List<PhuongTien>();

            danhSach.Add(oto);
            danhSach.Add(xeMay);

            foreach (PhuongTien pt in danhSach)
            {
                Console.WriteLine(
                    $"{pt.TenHang}: " +
                    $"{pt.TinhGiaLanBanh():N0} VNĐ");
            }

            Console.WriteLine("TC04: PASS");

            // QUẢN LÝ PHƯƠNG TIỆN

            QuanLyPhuongTien quanLy =
                new QuanLyPhuongTien();

            quanLy.AddPhuongTien(oto);
            quanLy.AddPhuongTien(xeMay);


            // THÊM Ô TÔ THỨ 2

            OTo oto2 = new OTo(
                "OT002",
                "Ford",
                2022,
                800000000m,
                7,
                2.5);

            quanLy.AddPhuongTien(oto2);

            // HIỂN THỊ DANH SÁCH


            quanLy.DisplayAll();

            // TC05 - TÌM GIÁ LĂN BÁNH CAO NHẤT

            Console.WriteLine("\n===== TC05 =====");

            PhuongTien max =
                quanLy.FindMaxGiaLanBanh();

            if (max != null)
            {
                Console.WriteLine(
                    "Phương tiện có giá lăn bánh cao nhất:");

                Console.WriteLine(max.GetInfo());

                Console.WriteLine(
                    $"Giá lăn bánh: " +
                    $"{max.TinhGiaLanBanh():N0} VNĐ");

                if (max == oto)
                    Console.WriteLine("TC05: PASS");
                else
                    Console.WriteLine("TC05: FAIL");
            }
            // SEARCH BY NAME

            Console.WriteLine("\n===== TÌM KIẾM TOYOTA =====");

            List<PhuongTien> ketQua =
                quanLy.SearchByName("Toyota");

            foreach (PhuongTien pt in ketQua)
            {
                Console.WriteLine(pt.GetInfo());
            }
        }
    }
}