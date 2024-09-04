using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LoginLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public User User { get; set; }

    }
}
