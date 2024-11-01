using QLKS.Controller;
using QLKS.Model;
using QLKS.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace QLKS
{
    public partial class CheckOut : Form, IView
    {
        private CustomerController customerController;
        public CheckOut()
        {
            InitializeComponent();
            customerController = new CustomerController();
            loadCheckOut();
        }

     

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Lấy hàng được chọn
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                // Cập nhật thông tin khách hàng
                txtCName.Text = selectedRow.Cells["cname"].Value.ToString();

                // Chuyển đổi roomid sang string và gán cho txtRoom
                txtRoom.Text = selectedRow.Cells["roomid"].Value.ToString(); // roomid là kiểu int
                txtCheckIn.Text = Convert.ToDateTime(selectedRow.Cells["checkin"].Value).ToString("yyyy-MM-dd");
                txtCheckOut.Text = Convert.ToDateTime(selectedRow.Cells["checkout"].Value).ToString("yyyy-MM-dd");
                 
            } 
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            loadCheckOut();
        }

        private void loadCheckOut()
        {
            var customerRoomDetails = customerController.GetCustomerRoomDetails();
            guna2DataGridView1.DataSource = customerRoomDetails;
        }

        public void SetDataToText()
        {
            throw new NotImplementedException();
        }

        public void GetDataFromText()
        {
            throw new NotImplementedException();
        }
    }
}
