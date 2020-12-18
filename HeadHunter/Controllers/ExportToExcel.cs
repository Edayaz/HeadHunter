using HeadHunter.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace HeadHunter.Controllers
{
    public class ExportToExcelController : Controller
    {

        HeadHunterEntities db = new HeadHunterEntities();

        public ActionResult ExporttoExcel()
        { 
            var data = db.ViewProfileProgramLanguage.ToList();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ProgrammingLanguages.xml");
            Response.ContentType = "text/xml";

            var serializer = new
            System.Xml.Serialization.XmlSerializer(data.GetType());
            serializer.Serialize(Response.OutputStream, data);

            return View();

        }


    }

}