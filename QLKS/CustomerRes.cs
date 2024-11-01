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

namespace QLKS
{
    public partial class CustomerRes : Form, IView
    {
        private CustomerController controller;
        private CustomerModel customer;
        private BindingList<CustomerModel> customerList; // Thêm BindingList

        public CustomerRes()
        {
            InitializeComponent();
            controller = new CustomerController();
            customer = new CustomerModel();
            customerList = new BindingList<CustomerModel>();

            // Gán giá trị cho ComboBox giới tính
            cmbGender.Items.Add("Nam");
            cmbGender.Items.Add("Nữ");

            // Gán sự kiện SelectedIndexChanged cho cmbRoomId
            cmbRoomId.SelectedIndexChanged += cmbRoomId_SelectedIndexChanged;
        }

        public void GetDataFromText()
        {
            // Kiểm tra tên khách hàng
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên khách hàng không được để trống.");
                return;
            }
            customer.cname = txtName.Text.Trim();

            // Kiểm tra số điện thoại
            if (!long.TryParse(txtPhoneNo.Text.Trim(), out var phone) || phone <= 0)
            {
                MessageBox.Show("Số điện thoại không hợp lệ.");
                return;
            }
            customer.mobile = phone;

            // Kiểm tra quốc tịch
            if (string.IsNullOrWhiteSpace(txtNationnality.Text))
            {
                MessageBox.Show("Quốc tịch không được để trống.");
                return;
            }
            customer.nationality = txtNationnality.Text.Trim();

            // Kiểm tra giới tính
            customer.gender = cmbGender.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(customer.gender))
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return;
            }

            // Lấy giá trị ngày sinh
            customer.dob = txtDob.Value;

            // Kiểm tra chứng minh thư
            if (string.IsNullOrWhiteSpace(txtIDProof.Text))
            {
                MessageBox.Show("Chứng minh thư không được để trống.");
                return;
            }
            customer.idproof = txtIDProof.Text.Trim();

            // Kiểm tra địa chỉ
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Địa chỉ không được để trống.");
                return;
            }
            customer.address = txtAddress.Text.Trim();

            // Lấy giá trị ngày check-in
            customer.checkin = txtCheckIn.Value.Date;

            // Kiểm tra và lấy giá trị roomid
            if (!int.TryParse(cmbRoomId.SelectedItem?.ToString(), out var roomId) || roomId < 0)
            {
                MessageBox.Show("Vui lòng chọn phòng hợp lệ.");
                return;
            }
            customer.roomid = roomId;
        }

        public void SetDataToText()
        {
            if (customer == null)
            {
                MessageBox.Show("Không có thông tin khách hàng để hiển thị.");
                return;
            }

            // Cập nhật dữ liệu khách hàng vào các TextBox và ComboBox
            txtName.Text = !string.IsNullOrWhiteSpace(customer.cname) ? customer.cname : string.Empty;
            txtPhoneNo.Text = customer.mobile > 0 ? customer.mobile.ToString() : string.Empty;
            txtNationnality.Text = !string.IsNullOrWhiteSpace(customer.nationality) ? customer.nationality : string.Empty;
            cmbGender.SelectedItem = !string.IsNullOrWhiteSpace(customer.gender) ? customer.gender : null;
            txtDob.Value = customer.dob != DateTime.MinValue ? customer.dob : DateTime.Today;
            txtIDProof.Text = !string.IsNullOrWhiteSpace(customer.idproof) ? customer.idproof : string.Empty;
            txtAddress.Text = !string.IsNullOrWhiteSpace(customer.address) ? customer.address : string.Empty;
            txtCheckIn.Value = customer.checkin != DateTime.MinValue ? customer.checkin : DateTime.Today;
            cmbRoomId.SelectedItem = customer.roomid > 0 ? customer.roomid.ToString() : null;

            // Lấy thông tin phòng từ RoomController dựa trên roomid của khách hàng
            RoomController roomController = new RoomController();
            RoomModel room = roomController.Read(new RoomModel { roomid = customer.roomid }) as RoomModel;

            if (room != null)
            {
                // Cập nhật dữ liệu phòng vào ComboBox và TextBox
                cmbRoomType.SelectedItem = !string.IsNullOrWhiteSpace(room.roomType) ? room.roomType : null;
                cmbBed.SelectedItem = !string.IsNullOrWhiteSpace(room.bed) ? room.bed : null;
                txtPrice.Text = room.price > 0 ? room.price.ToString() : string.Empty;
            }
            else
            {
                // Thông báo nếu không tìm thấy phòng
                MessageBox.Show("Không tìm thấy thông tin phòng cho RoomId đã chọn.");
                cmbRoomType.SelectedIndex = -1;
                cmbBed.SelectedIndex = -1;
                txtPrice.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetDataFromText();
            if (customer.IsValidate())
            {
                try
                {
                    if (controller.Create(customer))
                    {
                        MessageBox.Show("Khách hàng đã được thêm thành công!");

                        customerList.Add(customer); // Thêm khách hàng mới vào BindingList để cập nhật DataGridView tự động
                        // ClearForm(); // Nếu bạn muốn làm sạch form sau khi thêm
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi thêm khách hàng.");
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

     

        private void CustomerRes_Load(object sender, EventArgs e)
        {
            loadCustomer();
        }

        private void loadCustomer()
        {
            try
            {
                // Khởi tạo RoomController để tải dữ liệu phòng
                RoomController roomController = new RoomController();

                // Kiểm tra nếu tải dữ liệu phòng thành công
                if (roomController.Load())
                {
                    // Đổ dữ liệu roomid vào ComboBox cmbRoomId
                    cmbRoomId.DataSource = roomController.Items.Select(r => ((RoomModel)r).roomid).ToList();
                    cmbRoomId.DisplayMember = "roomid"; // hoặc có thể dùng tên hiển thị khác nếu có

                    // Đổ dữ liệu roomType vào ComboBox cmbRoomType
                    cmbRoomType.DataSource = roomController.Items.Select(r => ((RoomModel)r).roomType).Distinct().ToList();

                    // Đổ dữ liệu bed vào ComboBox cmbBed
                    cmbBed.DataSource = roomController.Items.Select(r => ((RoomModel)r).bed).Distinct().ToList();
                }
                else
                {
                    MessageBox.Show("Không thể tải dữ liệu phòng từ cơ sở dữ liệu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu phòng: {ex.Message}");
            }
        }

        private void cmbRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRoomId.SelectedItem != null)
            {
                int selectedRoomId = (int)cmbRoomId.SelectedItem;

                // Khởi tạo RoomController để lấy thông tin phòng
                RoomController roomController = new RoomController();
                RoomModel room = roomController.Read(new RoomModel { roomid = selectedRoomId }) as RoomModel;

                if (room != null)
                {
                    // Cập nhật thông tin phòng vào các ComboBox và TextBox
                    cmbRoomType.SelectedItem = room.roomType;
                    cmbBed.SelectedItem = room.bed;
                    txtPrice.Text = room.price.ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin phòng cho mã phòng đã chọn.");
                    cmbRoomType.SelectedIndex = -1;
                    cmbBed.SelectedIndex = -1;
                    txtPrice.Clear();
                }
            }
        }
    }
}
