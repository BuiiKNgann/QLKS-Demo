using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS.Model
{
    internal class EmployeeModel : IModel
    {
        //public int Eid { get; set; }
        //public string Ename { get; set; }
        //public long Mobile { get; set; }
        //public string Gender { get; set; }
        //public string EmailId { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }

        public int eid { get; set; }
        public string ename { get; set; }
        public long mobile { get; set; }
        public string gender { get; set; }
        public string emailid { get; set; }
        public string role { get; set; }
        public string pass { get; set; }
        public bool IsValidate()
        {
            return true;
        }
    }
}
