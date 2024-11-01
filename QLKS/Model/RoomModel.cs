using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS.Model
{
    internal class RoomModel : IModel
    {
        public int roomid { get; set; }
        public string roomNo { get; set; }
        public string roomType { get; set; }
        public string bed { get; set; }
        public long price { get; set; }
        public string booked { get; set; } = "NO"; // Giá trị mặc định là "NO"
        public bool IsValidate()
        {
            return true;
        }
    }
}
