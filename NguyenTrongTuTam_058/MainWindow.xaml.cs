using Microsoft.EntityFrameworkCore;
using OnThi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnThi
{
    public partial class MainWindow : Window
    {
        public static List<LoaiSp> lsp = new List<LoaiSp>();
        public static List<SanPham> sp = new List<SanPham>();
        public static List<HoaDonChiTiet> hd = new List<HoaDonChiTiet>();
        public static List<ItemSourceFile> it = new List<ItemSourceFile>();
        WpfPracticeContext db = new WpfPracticeContext();
        public MainWindow()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            hienThiDuLieu();
            selectLoaiSP.ItemsSource = lsp;
            selectLoaiSP.DisplayMemberPath = "TenLoai";
            selectLoaiSP.SelectedValuePath = "MaLoai";
            selectLoaiSP.SelectedIndex = 0;
        }
        private void hienThiDuLieu()
        {
           
            var querry = from LoaiSp in db.LoaiSps
                         select LoaiSp;
            var querry2 = from Sp in db.SanPhams
                          select Sp;
            var querry3 = from HoaDon in db.HoaDonChiTiets
                          select HoaDon;
            var querry4 = from Sp2 in db.SanPhams
                          orderby Sp2.DonGia ascending
                          select new
                          {
                              MaSp = Sp2.MaSp,
                              TenSp = Sp2.TenSp,
                              MaLoai = Sp2.MaLoai,
                              SoLuongCo = Sp2.SoLuongCo,
                              DonGia = Sp2.DonGia,
                              ThanhTien = Sp2.DonGia * Sp2.SoLuongCo
                          };
            lsp = querry.ToList();
            sp = querry2.ToList();
            hd = querry3.ToList();
            dgv.ItemsSource = querry4.ToList();
        }
        private void sua_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //add new sp
            try
            {
                SanPham newSP = new SanPham();
                newSP.MaSp = txtMaSP.Text;
                newSP.TenSp = txtTenSP.Text;
                newSP.DonGia = Convert.ToDecimal(txtDonGia.Text);
                newSP.SoLuongCo = Convert.ToInt32(txtSoLuongCo.Text);
                newSP.MaLoai = selectLoaiSP.SelectedValue.ToString();
                LoaiSp newL = new LoaiSp();
                newL.MaLoai = selectLoaiSP.SelectedValue.ToString();
                if (!db.SanPhams.Contains(newSP) && db.LoaiSps.Contains(newL))
                {
                    db.SanPhams.Add(newSP);
                    db.SaveChanges();
                    hienThiDuLieu();
                }
                else if (db.SanPhams.Contains(newSP))
                {
                    MessageBox.Show("Đã có sản phẩm " + txtMaSP.Text, "THÊM DỮ LIỆU",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Không tồn tại mã sản phẩm " + selectLoaiSP.SelectedValue.ToString(), "THÊM DỮ LIỆU",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Số lượng hàng hoặc đơn giá nhập không đúng định dạng!!!", "THÊM DỮ LIỆU",
                  MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.Message);
            }

        }

        private void dgv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgv.SelectedItem != null)
            {
                try
                {
                    Type t = dgv.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtMaSP.Text = p[0].GetValue(dgv.SelectedValue).ToString();
                    txtTenSP.Text = p[1].GetValue(dgv.SelectedValue).ToString();
                    selectLoaiSP.SelectedValue = p[2].GetValue(dgv.SelectedValue).ToString();
                    txtSoLuongCo.Text = p[3].GetValue(dgv.SelectedValue).ToString();
                    txtDonGia.Text = p[4].GetValue(dgv.SelectedValue).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Co Loi" + ex.Message, "Thong bao");
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void xoa_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(t => t.MaSp.Equals(txtMaSP.Text));
            if (query != null)
            {
                MessageBoxResult rs = MessageBox.Show("Ban co chac chan xoa?", "Thong bao",
                    MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(query);
                    db.SaveChanges();
                    MessageBox.Show("Da xoa", "Thong bao");
                    hienThiDuLieu();
                }
            }
            else
            {
                MessageBox.Show("Khong tim thay sp muon xoa", "Thong bao");
            }
        }

        private void tim_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.Where(t => t.MaSp.Equals(txtMaSP.Text)).Select(query => new
            {
                query.MaSp,
                query.TenSp,
                query.MaLoai,
                query.SoLuongCo,
                query.DonGia,
                ThanhTien = query.SoLuongCo * query.DonGia
            });
            if (query != null)
            {
                dgv.ItemsSource = query.ToList();
            }
            else
            {
                MessageBox.Show("Khong tim thay sp ", "Thong bao");
            }
        }
    }
}
