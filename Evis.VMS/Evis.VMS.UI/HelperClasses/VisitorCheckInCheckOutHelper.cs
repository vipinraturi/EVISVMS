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
using Evis.VMS.Utilities;

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
                                                            DOB = (item.DOB ?? DateTime.MinValue).ToShortDateString(),
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

        public VisitorDataVM GetVisitorCheckInHistory(long visitorId, string userId)
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
                                                            CheckInTime = item.CheckIn.ToLocalTime().ToString("hh:mm tt"),
                                                            CheckOutTime = (item.CheckOut == null ? string.Empty : item.CheckOut.Value.ToString("hh:mm tt")),
                                                            TotalDuration = (item.CheckOut == null ? "N/A" : Utility.TimeSince(item.CheckOut.Value.Subtract(item.CheckIn))),
                                                            CompanyName = item.CompanyName,
                                                            ContactPerson = item.ContactPerson,
                                                            NoOfPerson = item.NoOfPerson.ToString(),
                                                            Purpose = item.PurposeOfVisit,
                                                            VahicleNumber = item.VahicleNumber,
                                                            Floor = item.Floor
                                                        });
                    });

                    if (lstVisitorCheckInAndOuttimes.Count > 0)
                    {
                        var latestCheck = lstVisitorCheckInAndOuttimes.FirstOrDefault();

                        if (string.IsNullOrEmpty(latestCheck.CheckOutTime))
                        {
                            result.IsAlreadyCheckIn = true;
                        }
                        else
                        {
                            result.IsAlreadyCheckIn = false;
                        }
                    }

                }


                var shiftAssignedOrNot = _genericService.ShitfAssignment.GetAll().Where(item => item.UserId == userId && item.IsActive).FirstOrDefault();
                if (shiftAssignedOrNot != null)
                {
                    result.IsShiftAssignedToSecurity = true;
                }


                var gateCount = _genericService.GateMaster.GetAll().Count();
                result.IsAnyGateExist = (gateCount > 0 ? true : false);
                result.VisitorHiostory = lstVisitorCheckInAndOuttimes.ToList();
                result.DOB = (visitorData.DOB ?? DateTime.MinValue).ToShortDateString();
                result.EmailId = visitorData.EmailId;
                result.Gender = visitorData.GenderMaster.LookUpValue;
                result.TypeOfCard = visitorData.TypeOfCard.LookUpValue;
                result.IdentificationNo = visitorData.IdNo;
                result.MobileNo = visitorData.ContactNo;
                result.Nationality = visitorData.CountryMaster.LookUpValue;
                result.VisitorId = visitorData.Id;
                result.VisitorName = visitorData.VisitorName;
                result.CompanyName = visitorData.CompanyName;

            }

            return result;
        }

        public bool SaveVisitorCheckIn(VisitorCheckInVM visitorCheckInVM, string userId)
        {
            var gate = _genericService.ShitfAssignment.GetAll().Where(item => item.UserId == userId && item.IsActive).FirstOrDefault();

            if (gate != null)
            {
                VisitDetails _visitDetails = new VisitDetails();
                _visitDetails.VisitorId = visitorCheckInVM.VisitorId;
                _visitDetails.ContactPerson = visitorCheckInVM.ContactPerson;
                _visitDetails.NoOfPerson = visitorCheckInVM.NoOfPerson;
                _visitDetails.PurposeOfVisit = visitorCheckInVM.PurposeOfVisit;
                _visitDetails.CheckIn = DateTime.UtcNow;
                _visitDetails.CheckOut = null;
                _visitDetails.CreatedBy = visitorCheckInVM.CreatedBy;
                _visitDetails.CheckInGate = gate.GateId;
                _visitDetails.CheckOutGate = gate.GateId;
                _visitDetails.CompanyName = visitorCheckInVM.CompanyName;
                _visitDetails.VahicleNumber = visitorCheckInVM.VahicleNumber;
                _visitDetails.Floor = visitorCheckInVM.Floor;
                _genericService.VisitDetails.Insert(_visitDetails);
                _genericService.Commit();
                return true;
            }
            return false;
        }

        public bool SaveVisitorCheckOut(VisitorCheckInVM visitorCheckInVM)
        {
            var visitDetail = _genericService.VisitDetails.GetAll().Where(item => item.VisitorId == visitorCheckInVM.VisitorId).OrderByDescending(item => item.CheckIn).FirstOrDefault();

            if (visitDetail != null)
            {
                if (visitDetail.CheckOut == null)
                {
                    visitDetail.CheckOut = DateTime.UtcNow;
                    _genericService.Commit();
                    return true;
                }
            }

            return false;
        }

        public List<VisitorJsonModel> GetVisitorData(string searchterm, int? organizationId, bool isCheckIn, string userId)
        {
            var result = new List<VisitorJsonModel>();
            var qryVisitors = _genericService.VisitorMaster.GetAll()
                .Where(item => (organizationId == null || item.ApplicationUser.OrganizationId == organizationId) &&
                    (item.VisitorName.ToLower().Contains(searchterm.ToLower()) ||
                    item.EmailId.ToLower().Contains(searchterm.ToLower()) ||
                    item.ContactNo.ToLower().Contains(searchterm.ToLower()) ||
                    item.IdNo.ToLower().Contains(searchterm.ToLower()))
                    );

            if (!isCheckIn)//for checkout only
            {
                if (qryVisitors.FirstOrDefault() != null)
                {
                    var objVisitorDataVM = GetVisitorCheckInHistory(qryVisitors.FirstOrDefault().Id, userId);
                    if (!objVisitorDataVM.IsAlreadyCheckIn)//In case no check-in done, no need to check-out
                    {
                        return result;
                    }
                }
            }

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
                        LogoUrl = "/images/VisitorImages/" + item.ProfilePicPath,
                        IdentityImage1_Path = "/images/VisitorIdentityImages/" + item.IdentityImage1_Path,
                        IdentityImage2_Path = "/images/VisitorIdentityImages/" + item.IdentityImage2_Path,
                        IdentityImage3_Path = "/images/VisitorIdentityImages/" + item.IdentityImage3_Path,
                        CompanyName = item.CompanyName
                    });
                });
            }

            return result;
        }
    }
}