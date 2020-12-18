using HeadHunter.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadHunter.Models.UnifiedModels
{
    public class SignUpModel
    {
        public Account Account { get; set; }
        public AccountPassword AccountPassword { get; set; }
        public Profil Profile { get; set; }
        public string StackOverFlowUsername { get; set; }
    }
}