/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class VisitorCheckInViewModel
    {

    }

    public class VisitorAutoCompleteVM
    {
        public long VisitorId { get; set; }

        public string VisitorName { get; set; }

        public string MobileNo { get; set; }

        public string EmailId { get; set; }

        public string IdentificationNo { get; set; }
    }

    public class VisitorDataVM
    {
        public long VisitorId { get; set; }

        public string VisitorName { get; set; }

        public string Gender { get; set; }
        
        public string TypeOfCard { get; set; }

        public DateTime DOB { get; set; }

        public string MobileNo { get; set; }

        public string EmailId { get; set; }

        public string IdentificationNo { get; set; }

        public string Nationality { get; set; }

        public IList<VisitorCheckInCheckOutHistoryVM> VisitorHiostory { get; set; }

        public bool IsSecurityPerson { get; set; }

        public bool IsAnyGateExist { get; set; }

        public string CompanyName { get; set; }

        public string VahicleNumber { get; set; }

        public string Floor { get; set; }
    }

    public class VisitorCheckInVM
    {
        public long VisitorId { get; set; }

        public string ContactPerson { get; set; }

        public int NoOfPerson { get; set; }

        public string PurposeOfVisit { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public string CreatedBy { get; set; }

        public int CheckInGate { get; set; }

        public int CheckOutGate { get; set; }
    }

    public class VisitorCheckInCheckOutHistoryListVM
    {
        public List<VisitorCheckInCheckOutHistoryVM> VisitorCheckInCheckOutHistoryVM { get; set; }

        public int TotalCount { get; set; }
    }
    public class VisitorCheckInCheckOutHistoryVM
    {
        public DateTime CheckInDate { get; set; }

        public TimeSpan CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        public string TotalDuration { get; set; }
    }

    public class VisitorJsonModel
    {
        public string VisitorName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string IndentityNumber { get; set; }
        public string VisitorId { get; set; }
        public string LogoUrl { get; set; }
    }
}