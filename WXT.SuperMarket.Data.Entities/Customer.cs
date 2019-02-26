using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXT.SuperMarket.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string EmaiAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
