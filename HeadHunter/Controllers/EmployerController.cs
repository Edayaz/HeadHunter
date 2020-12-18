using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HeadHunter.Controllers
{
    public class EmployerController : Controller
    {
        HeadHunter.Models.Entity.HeadHunterEntities db = new Models.Entity.HeadHunterEntities();
        // GET: Employer
        [HttpGet]
        public ActionResult SendMail(int id)
        {
            var user = db.Account.Find(id);

            int _AccountId = id;
            string mail = db.Account.FirstOrDefault(m => m.AccountId == _AccountId).AccountMail;
            string body = "Merhaba," + System.Environment.NewLine +
                "        Sizin için bir iş teklifi var eğer ilgileniyorsanız bu maile cevap vererek teklif yapan firmanın ileşim bilgilerini alabilirsiniz.";
            var fromAddress = new MailAddress("headhuntersystem@gmail.com");
            var toAddress = new MailAddress(mail);
            const string subject = "HeadHunter | İşte size bir iş teklifi";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "headhunter9090")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }

            //Response.Write("mail başarıyla gönderildi.");
            return View();
        }
        [HttpPost]
        public ActionResult SendMail()
        {
            int _profileId=2;
            int _AccountId=_profileId;
            string mail =  db.Account.FirstOrDefault(m => m.AccountId == _AccountId).AccountMail;
            string body = "hi";
            var fromAddress = new MailAddress("headhuntersystem@gmail.com");
            var toAddress = new MailAddress(mail);
            const string subject = "HeadHunter | İşte size bir iş teklifi";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "headhunter9090")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }

            Response.Write("mail başarıyla gönderildi.");
            return View();
        }


        [AuthorizeRoleAttribute(RoleNames.Employer)]
        public ActionResult Profile()
        {


            return View();
        }

        [AuthorizeRoleAttribute(RoleNames.Employer)]
        public ActionResult Home()
        {
            return View();
        }

        [AuthorizeRoleAttribute(RoleNames.Employer)]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            List<string> FilterPL = new List<string>();
      
            foreach(var key in collection.Keys)
            {
                var getFilterType = key.ToString().Substring(0, 3);
                var filter = ((string[])(collection.GetValue(key.ToString()).RawValue));
                switch (getFilterType)
                {
                    case "PL_":
                        if (filter.Length > 1)
                            FilterPL.Add(filter[0]);
                        break;
                }
                //Console.WriteLine(key.ToString() + " = " + collection.GetValue(key.ToString()).RawValue.ToString());
            }
            var arrFilter = FilterPL.ToArray();
            var countFilter = arrFilter.Length;
            var profilePL = db.ViewProfileProgramLanguage.Where(x => arrFilter.Contains(x.SourceDataTypeName)).GroupBy(x => x.ProfileId).Where(x => x.Count() == countFilter).Select(x => x.Key).ToArray();

            //Sorgusu yazılacak
            //bu kısım filtreleri and ile birleştirmek için bu kısımda ekleme yapılıyor
            var profileRS = profilePL;//yeni filtre

            var lastPL = from p1 in profilePL
                         from p2 in profileRS.Where(x => x == p1)
                             // from p3 in profileGN.Where(x => x== p1)
                             // from p4 in profileLG.Where(x => x== p1)
                         select p1;
            ViewBag.ProfileList = db.Profil.Where(x => lastPL.Contains(x.ProfileId)).ToList();
            return View();
        }
    }
}