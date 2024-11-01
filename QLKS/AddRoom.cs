using QLKS.Controller;
using QLKS.Model;
using QLKS.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QLKS
{
    public partial class AddRoom : Form, IView
    {
        private RoomController controller;
        private RoomModel room;
        private BindingList<RoomModel> roomList; // Thêm BindingList
        public AddRoom()
        {
            InitializeComponent();
            controller = new RoomController();
            room = new RoomModel();
            roomList = new BindingList<RoomModel>();

        }

        public void GetDataFromText()
        {
            room.roomNo = txtRoomNo.Text.Trim();
            room.roomType = cmbBed.SelectedItem?.ToString() ?? string.Empty;
            room.bed = cmbRoomType.SelectedItem?.ToString() ?? string.Empty;
            room.price = Convert.ToInt64(txtPrice.Text.Trim());

        }

        public void SetDataToText()
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                // Tạo một đối tượng EmployeeModel từ dữ liệu của dòng đã chọn
                room = new RoomModel
                {
                    roomid = Convert.ToInt32(selectedRow.Cells["roomid"].Value),
                    roomNo = selectedRow.Cells["roomNo"].Value.ToString(),
                    roomType = selectedRow.Cells["roomType"].Value.ToString(),
                    bed = selectedRow.Cells["bed"].Value.ToString(),
                    price = Convert.ToInt64(selectedRow.Cells["price"].Value),
                    booked = selectedRow.Cells["booked"].Value.ToString(),
                };

            };

            // Cập nhật dữ liệu vào TextBox
            txtRoomNo.Text = room.roomNo.ToString();
            cmbBed.SelectedItem = room.bed;
            cmbRoomType.SelectedItem = room.roomType;
            txtPrice.Text = room.price.ToString();

        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            LoadRooms();
        }

        private void LoadRooms()
        {
            try
            {
                if (controller.Load())
                {
                    roomList.Clear();
                    foreach (var room in controller.Items.Cast<RoomModel>())
                    {
                        roomList.Add(room); // Thêm mỗi phòng vào BindingList
                    }

                    guna2DataGridView1.DataSource = roomList; // Gán BindingList làm nguồn dữ liệu

                    guna2DataGridView1.Columns["roomid"].HeaderText = "Mã phòng";
                    guna2DataGridView1.Columns["roomNo"].HeaderText = "Số phòng";
                    guna2DataGridView1.Columns["roomType"].HeaderText = "Loại phòng";
                    guna2DataGridView1.Columns["bed"].HeaderText = "Loại giường";
                    guna2DataGridView1.Columns["price"].HeaderText = "Giá";
                    guna2DataGridView1.Columns["booked"].HeaderText = "Booked";

                    // Load data into ComboBoxes
                    cmbBed.Items.Clear();
                    cmbRoomType.Items.Clear();

                    // Thêm các giá trị duy nhất vào ComboBox
                    foreach (var room in roomList)
                    {
                        if (!cmbBed.Items.Contains(room.bed))
                            cmbBed.Items.Add(room.bed);

                        if (!cmbRoomType.Items.Contains(room.roomType))
                            cmbRoomType.Items.Add(room.roomType);
                    }

                    // Chọn mục đầu tiên trong ComboBox
                    if (cmbBed.Items.Count > 0) cmbBed.SelectedIndex = 0;
                    if (cmbRoomType.Items.Count > 0) cmbRoomType.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}");
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SetDataToText(); // Hiển thị dữ liệu row được chọn
            }
        }
        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                SetDataToText(); // Cập nhật dữ liệu trong TextBox khi chọn dòng mới
            }

        }
        public void UpdateDataGridView()
        {
            roomList.Clear(); // Xóa danh sách hiện tại trước khi thêm mới

            foreach (var room in controller.Items.Cast<RoomModel>())
            {
                roomList.Add(room); // Thêm mỗi chi nhánh vào BindingList
            }

            guna2DataGridView1.DataSource = roomList; // Gán BindingList làm nguồn dữ liệu
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            GetDataFromText();
            if (room.IsValidate())
            {
                try
                {
                    if (controller.Create(room))
                    {
                        MessageBox.Show("Phòng đã được thêm thành công!");

                        roomList.Add(room); // Thêm phòng mới vào BindingList để cập nhật DataGridView tự động
                       ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi thêm phòng.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetDataFromText();

            if (room.IsValidate())
            {
                // Hỏi người dùng xác nhận trước khi xóa
                var confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa khách hàng này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                // Kiểm tra nếu người dùng chọn "Yes"
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Gọi hàm Delete với đối tượng EmployeeModel
                        if (controller.Delete(room))
                        {
                            MessageBox.Show("Khách hàng đã được xóa thành công!");
                           ClearForm();
                            LoadRooms(); // Refresh danh sách khách hàng
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra khi xóa khách hàng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            GetDataFromText();

            if (room.IsValidate()) // Kiểm tra tính hợp lệ của dữ liệu
            {
                try
                {
                    if (controller.Update(room)) // Cập nhật thông tin nhân viên
                    {
                        MessageBox.Show("Room đã được cập nhật thành công!");
                      ClearForm();
                        LoadRooms(); // Tải lại danh sách nhân viên sau khi cập nhật
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi cập nhật phòng.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
            }
        }
        private void ClearForm()
        {
            txtRoomNo.Clear();
            cmbBed.SelectedIndex = -1;
            cmbRoomType.SelectedIndex = -1;
            txtPrice.Clear();
        }
    }
}
