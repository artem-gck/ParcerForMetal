using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes.Models.Login
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string NumberOfPhone { get; set; }
        public string Position { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
