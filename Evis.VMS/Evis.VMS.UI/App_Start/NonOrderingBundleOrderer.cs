using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace Evis.VMS.UI.App_Start
{
    public class NonOrderingBundleOrderer : IBundleOrderer
    {
       

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
