using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace vuhongphong._24810320183
{
    public partial class Form1 : Form
    {
        private BindingList<Product> bindingList;
        private BindingSource bindingSource;
        private List<Product> masterList;

        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            masterList = new List<Product>();
            bindingList = new BindingList<Product>(masterList);
            bindingSource = new BindingSource();
            bindingSource.DataSource = bindingList;

            // Setup DataGridView columns
            dgvProducts.Columns.Clear();
            dgvProducts.AutoGenerateColumns = false;

            var colId = new DataGridViewTextBoxColumn { HeaderText = "Mã SP", DataPropertyName = "ProductId", Name = "colId" };
            var colName = new DataGridViewTextBoxColumn { HeaderText = "Tên SP", DataPropertyName = "ProductName", Name = "colName", Width = 200 };
            var colCategory = new DataGridViewTextBoxColumn { HeaderText = "Danh Mục", DataPropertyName = "Category", Name = "colCategory" };
            var colPrice = new DataGridViewTextBoxColumn { HeaderText = "Đơn Giá", DataPropertyName = "UnitPrice", Name = "colPrice", Width = 120 };
            var colQty = new DataGridViewTextBoxColumn { HeaderText = "Số Lượng", DataPropertyName = "Quantity", Name = "colQty" };

            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCategory, colPrice, colQty });
            dgvProducts.DataSource = bindingSource;

            // categories
            var categories = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("Phone","Điện thoại"),
                new KeyValuePair<string,string>("Laptop","Laptop"),
                new KeyValuePair<string,string>("Accessory","Phụ kiện")
            };
            cboCategory.DisplayMember = "Value";
            cboCategory.ValueMember = "Key";
            cboCategory.DataSource = categories;

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            toolStripStatusLabel.Text = $"Tổng số sản phẩm: {masterList.Count}";
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                picAvatar.ImageLocation = dlg.FileName;
            }
        }

        private bool ValidateInputs()
        {
            errorProvider.Clear();
            bool ok = true;
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider.SetError(txtProductName, "Tên SP không được để trống");
                ok = false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out var price) || price <= 0)
            {
                errorProvider.SetError(txtUnitPrice, "Đơn giá phải là số > 0");
                ok = false;
            }
            if (!int.TryParse(txtQuantity.Text, out var qty) || qty < 0)
            {
                errorProvider.SetError(txtQuantity, "Số lượng phải là số nguyên ≥ 0");
                ok = false;
            }
            return ok;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var product = new Product
            {
                ProductId = txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim()),
                Quantity = int.Parse(txtQuantity.Text.Trim()),
                Category = cboCategory.SelectedValue?.ToString() ?? string.Empty,
                ImagePath = picAvatar.ImageLocation
            };

            masterList.Add(product);
            RefreshBinding();
            ClearInputs();
            UpdateStatus();
        }

        private void RefreshBinding()
        {
            bindingList = new BindingList<Product>(masterList);
            bindingSource.DataSource = bindingList;
            dgvProducts.DataSource = bindingSource;
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = 0;
            picAvatar.ImageLocation = null;
        }

        private Product GetSelectedProduct()
        {
            if (dgvProducts.CurrentRow == null) return null;
            return dgvProducts.CurrentRow.DataBoundItem as Product;
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            var p = GetSelectedProduct();
            if (p == null) return;
            txtProductId.Text = p.ProductId;
            txtProductName.Text = p.ProductName;
            txtUnitPrice.Text = p.UnitPrice.ToString();
            txtQuantity.Text = p.Quantity.ToString();
            // set category by Key
            for (int i = 0; i < cboCategory.Items.Count; i++)
            {
                var kv = (KeyValuePair<string, string>)cboCategory.Items[i];
                if (kv.Key == p.Category)
                {
                    cboCategory.SelectedIndex = i;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(p.ImagePath) && File.Exists(p.ImagePath))
            {
                picAvatar.ImageLocation = p.ImagePath;
            }
            else
            {
                picAvatar.Image = null;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedProduct();
            if (selected == null) return;
            if (!ValidateInputs()) return;

            selected.ProductId = txtProductId.Text.Trim();
            selected.ProductName = txtProductName.Text.Trim();
            selected.UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim());
            selected.Quantity = int.Parse(txtQuantity.Text.Trim());
            selected.Category = cboCategory.SelectedValue?.ToString() ?? string.Empty;
            selected.ImagePath = picAvatar.ImageLocation;

            // refresh grid
            dgvProducts.Refresh();
            UpdateStatus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedProduct();
            if (selected == null) return;
            var result = MessageBox.Show("Are you sure you want to delete the selected product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                masterList.Remove(selected);
                RefreshBinding();
                UpdateStatus();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var q = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(q))
            {
                bindingSource.DataSource = new BindingList<Product>(masterList);
            }
            else
            {
                var filtered = masterList.Where(p => p.ProductName?.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                bindingSource.DataSource = new BindingList<Product>(filtered);
            }
            dgvProducts.DataSource = bindingSource;
        }

        private void exportCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog();
            dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            dlg.DefaultExt = "csv";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var sw = new StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8);
                sw.WriteLine("Mã SP,Tên SP,Danh Mục,Đơn Giá,Số Lượng,ImagePath");
                foreach (var p in masterList)
                {
                    var priceFormatted = string.Format("{0:N0} VNĐ", p.UnitPrice);
                    var line = $"{EscapeCsv(p.ProductId)},{EscapeCsv(p.ProductName)},{EscapeCsv(p.Category)},{EscapeCsv(priceFormatted)},{p.Quantity},{EscapeCsv(p.ImagePath)}";
                    sw.WriteLine(line);
                }
                MessageBox.Show("Export completed.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string EscapeCsv(string s)
        {
            if (s == null) return "";
            if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
            {
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            }
            return s;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns[e.ColumnIndex].Name == "colPrice" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out var price))
                {
                    e.Value = string.Format("{0:N0} VNĐ", price);
                    e.FormattingApplied = true;
                }
            }
            if (dgvProducts.Columns[e.ColumnIndex].Name == "colCategory" && e.Value != null)
            {
                // map keys to display values
                var key = e.Value.ToString();
                if (key == "Phone") e.Value = "Điện thoại";
                else if (key == "Accessory") e.Value = "Phụ kiện";
            }
        }
    }
}
