using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class GraphVM
    {
        public GraphVM()
        {

        }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class GraphDataList
    {
        public GraphDataList()
        {

        }
        public GraphVM GraphList { get; set; }
    }
    
}