using HeadHunter.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadHunter.Models.UnifiedModels
{
    public class LoginModel
    {
        public Account Account { get; set; }
        public AccountPassword AccountPassword { get; set; }
    }
}