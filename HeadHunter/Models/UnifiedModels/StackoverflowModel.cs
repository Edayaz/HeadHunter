using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadHunter.Models.UnifiedModels
{
    public class StackoverflowModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public GoogleVisualizationDataTable DataTable { get; set; }
    }
}