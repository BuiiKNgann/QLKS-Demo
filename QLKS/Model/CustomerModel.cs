using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS.Model
{
    internal class CustomerModel : IModel
    {
        public int cid { get; set; }          // cid
        public string cname { get; set; }      // cname
        public long mobile { get; set; }              // mobile
        public string nationality { get; set; }       // nationality
        public string gender { get; set; }            // gender
        public DateTime dob { get; set; }     // dob
        public string idproof { get; set; }           // idproof
        public string address { get; set; }           // address
        public DateTime checkin { get; set; }         // checkin
        public DateTime? checkout { get; set; }       
  
        public int roomid { get; set; }               
        public bool IsValidate()
        {
          return true;
        }
    }
}
