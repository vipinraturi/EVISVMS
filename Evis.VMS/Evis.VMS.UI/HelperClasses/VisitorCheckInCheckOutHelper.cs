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
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class VisitorCheckInCheckOutHelper
    {
        private readonly GenericService _genericService = null;

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

        public VisitorDataVM GetVisitorCheckInHistory(long visitorId)
        {
            var result = new VisitorDataVM();
            var visitorData = _genericService.VisitorMaster.GetAll().Where(item => item.Id == visitorId).FirstOrDefault();

            if (visitorData != null)
            {
                var query = _genericService.VisitDetails.GetAll().Where(x => x.VisitorId == visitorId).OrderByDescending(item => item.CheckIn);
                List<VisitorCheckInCheckOutHistoryVM> lstVisitorCheckInAndOuttimes = new List<VisitorCheckInCheckOutHistoryVM>();

                if (query.Count() > 0)
                {
                    query.ToList().ForEach(item =>
                    {
                        lstVisitorCheckInAndOuttimes.Add(new VisitorCheckInCheckOutHistoryVM
                                                        {
                                                            CheckInDate = item.CheckIn.Date.ToShortDateString(),
                                                            CheckInTime = item.CheckIn.TimeOfDay.ToString(),
                                                            CheckOutTime = (item.CheckOut == null ? string.Empty : item.CheckOut.Value.TimeOfDay.ToString()),
                                                            TotalDuration =  "0 mins",
                                                        });
                    });

                    if (lstVisitorCheckInAndOuttimes.Count >0 )
                    {
                        var latestCheck = lstVisitorCheckInAndOuttimes.FirstOrDefault();

                        if (string.IsNullOrEmpty(latestCheck.CheckOutTime))
                        {
                            result.IsAlreadyCheckIn = false;
                        }
                        else
                        {
                            result.IsAlreadyCheckIn = true;
                        }
                    }
                }

                var gateCount = _genericService.GateMaster.GetAll().Count();
                result.IsAnyGateExist = (gateCount > 0 ? true : false);
                result.VisitorHiostory = lstVisitorCheckInAndOuttimes.ToList();
                result.DOB = visitorData.DOB ?? DateTime.MinValue;
                result.EmailId = visitorData.EmailId;
                result.Gender = visitorData.GenderMaster.LookUpValue;
                result.TypeOfCard = visitorData.TypeOfCard.LookUpValue;
                result.IdentificationNo = visitorData.IdNo;
                result.MobileNo = visitorData.ContactNo;
                result.Nationality = visitorData.CountryMaster.LookUpValue;
                result.VisitorId = visitorData.Id;
                result.VisitorName = visitorData.VisitorName;

                
            }

            return result;
        }

        public bool SaveVisitorCheckIn(VisitorCheckInVM visitorCheckInVM)
        {
            VisitDetails _visitDetails = new VisitDetails();
            _visitDetails.VisitorId = visitorCheckInVM.VisitorId;
            _visitDetails.ContactPerson = visitorCheckInVM.ContactPerson;
            _visitDetails.NoOfPerson = visitorCheckInVM.NoOfPerson;
            _visitDetails.PurposeOfVisit = visitorCheckInVM.PurposeOfVisit;
            _visitDetails.CheckIn = DateTime.UtcNow;
            _visitDetails.CheckOut = null;
            _visitDetails.CreatedBy = visitorCheckInVM.CreatedBy;
            _visitDetails.CheckInGate = visitorCheckInVM.CheckInGate;
            _visitDetails.CheckOutGate = visitorCheckInVM.CheckOutGate;
            _genericService.VisitDetails.Insert(_visitDetails);
            _genericService.Commit();
            return true;
        }

        public List<VisitorJsonModel> GetVisitorData(string searchterm)
        {
            var result = new List<VisitorJsonModel>();
            var qryVisitors = _genericService.VisitorMaster.GetAll()
                .Where(item =>
                    item.VisitorName.ToLower().Contains(searchterm.ToLower()) ||
                    item.EmailId.ToLower().Contains(searchterm.ToLower()) ||
                    item.ContactNo.ToLower().Contains(searchterm.ToLower())
                    );

            if (qryVisitors.Count() > 0)
            {
                qryVisitors.ToList().ForEach(item =>
                {
                    result.Add(new VisitorJsonModel
                    {
                        VisitorName = item.VisitorName,
                        VisitorId = item.Id.ToString(),
                        Email = item.EmailId.ToString(),
                        MobileNumber = item.ContactNo.ToString(),
                        IndentityNumber = item.IdNo.ToString(),
                        LogoUrl = "/images/VisitorImages/" + item.ImagePath
                    });
                });
            }

            return result;
        }
    }
}