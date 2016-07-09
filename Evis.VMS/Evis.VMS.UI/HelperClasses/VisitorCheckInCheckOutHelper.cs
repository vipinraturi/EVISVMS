/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Evis.VMS.Data.Model.Entities;

namespace Evis.VMS.UI.HelperClasses
{
    public class VisitorCheckInCheckOutHelper
    {
       private readonly  GenericService _genericService = null;

        public VisitorCheckInCheckOutHelper()
        {
            _genericService = new GenericService();
        }

        public VisitorDataVM GetVisitorsDetail(string globalSearch)
        {
            VisitorDataVM _visitorDataVM = new VisitorDataVM(); ;
            var lstVisitors = _genericService.VisitorMaster.GetAll().Where(x => x.ContactNo == globalSearch)
                                                        .Select(item => new VisitorDataVM
                                                        {
                                                            VisitorId = item.Id,
                                                            VisitorName = item.VisitorName,
                                                            Gender = item.GenderMaster.LookUpValue,
                                                            DOB = item.DOB ?? DateTime.MinValue,
                                                            MobileNo = item.ContactNo,
                                                            EmailId = item.EmailId,
                                                            IdentificationNo = item.IdNo,
                                                            Nationality = item.CountryMaster.LookUpValue
                                                        }).AsQueryable();
            if (lstVisitors.Count() > 0)
            {
                _visitorDataVM = lstVisitors.FirstOrDefault();
            }

            return _visitorDataVM;
        }

        public IList<VisitorAutoCompleteVM> GetVisitorsAutoCompleteData(string globalSearch)
        {
            var lstVisitorsAutoComplete = _genericService.VisitorMaster.GetAll()
                                                        .Select(item => new VisitorAutoCompleteVM
                                                        {
                                                            VisitorId = item.Id,
                                                            VisitorName = item.VisitorName,
                                                            MobileNo = item.ContactNo,
                                                            EmailId = item.EmailId,
                                                            IdentificationNo = item.IdNo,
                                                        }).AsQueryable();

            return lstVisitorsAutoComplete.ToList();
        }

        public IList<VisitorCheckInCheckOutHistoryVM> GetVisitorCheckInHistory(long visitorId, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {

            var lstVisitorsCheckInHistory = _genericService.VisitDetails.GetAll().Where(x=> x.VisitorId==visitorId).AsEnumerable()
                                                        .Select(item => new VisitorCheckInCheckOutHistoryVM
                                                        {
                                                            CheckInDate=item.CheckIn.Date,
                                                            CheckInTime=item.CheckIn.TimeOfDay,
                                                            CheckOutTime=item.CheckOut.Value.TimeOfDay
                                                        }).AsQueryable();

            return lstVisitorsCheckInHistory.ToList();
        }

        public bool SaveVisitorCheckIn(VisitorCheckInVM visitorCheckInVM)
        {
            VisitDetails _visitDetails = new VisitDetails();
            _visitDetails.VisitorId = visitorCheckInVM.VisitorId;
            _visitDetails.ContactPerson = visitorCheckInVM.ContactPerson;
            _visitDetails.NoOfPerson = visitorCheckInVM.NoOfPerson;
            _visitDetails.PurposeOfVisit = visitorCheckInVM.PurposeOfVisit;
            _visitDetails.CheckIn = visitorCheckInVM.CheckIn;
            _visitDetails.CheckOut = visitorCheckInVM.CheckOut;
            _visitDetails.CreatedBy = visitorCheckInVM.CreatedBy;
            _visitDetails.CheckInGate = visitorCheckInVM.CheckInGate;
            _visitDetails.CheckOutGate = visitorCheckInVM.CheckOutGate;

            _genericService.VisitDetails.Insert(_visitDetails);
            _genericService.Commit();
            return true;
        }
    }
}