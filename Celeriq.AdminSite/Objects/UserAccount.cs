using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Celeriq.AdminSite.Objects
{
    [Serializable]
    public class UserAccount
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}