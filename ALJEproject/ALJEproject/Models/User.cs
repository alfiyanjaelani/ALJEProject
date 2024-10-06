using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALJEproject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
