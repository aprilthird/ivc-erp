using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Common.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
