using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatraxImaging.Models {
    public class AccountModel {
        public string Username { get; set; }
        public string RealName { get; }
        public string UserId { get; }

        public AccountModel(string accountName, string realName, string accountId) {
            Username = accountName;
            RealName = realName;
            UserId = accountId;
        }
    }
}
