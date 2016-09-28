using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class ShiftManagemetHelper
    {

        GenericService _genericService = null;
        public ShiftManagemetHelper()
        {
            _genericService = new GenericService();
        }
        public IList<ShiftManagementVM> GetShiftData(DateTime fromDate,DateTime toDate, int buldingId, int gateId)
        {
            


            return null;
        }
    }
}