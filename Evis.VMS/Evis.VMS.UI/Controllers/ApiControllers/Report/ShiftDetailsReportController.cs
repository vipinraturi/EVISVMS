/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Evis.VMS.Data.Model.Entities;
using System.Web;
using Microsoft.AspNet.Identity;
using Evis.VMS.Utilities;
using System.Globalization;
using Newtonsoft.Json;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class ReportController 
    {

         [Route("~/Api/Report/Getorganisation")]
         [HttpGet]
         public async Task<ApplicationUser> GetOrganisation()
         {
             string userId = HttpContext.Current.User.Identity.GetUserId();
             var organisation = await _userService.GetAsync(x => x.Id == userId);
             return organisation;


         }
         [Route("~/Api/Report/GetBuildings")]
         [HttpGet]
         public IEnumerable<GeneralDropDownVM> GetAllBuildingDetails(int Id)
         {
             if (Id != 0)
             {
                 var result = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true && x.OrganizationId == Id)
                   .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.BuildingName });
                 return result;
             }
             return null;

         }
     
         [Route("~/Api/Report/GetGates")]
         [HttpGet]
         public IEnumerable<GeneralDropDownVM> GetAllGates(int Id)
         {
             var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true && x.BuildingMaster.OrganizationId ==Id)
                 .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
             return result;
         }

         [Route("~/Api/Report/GetUsers")]
         [HttpGet]
         public async Task<IEnumerable<DropDownVM>> GetUsers()
         {

             var user = (await _userService.GetAllAsync()).FirstOrDefault();
             //////var gates = _genericService.GateMaster.GetAll().FirstOrDefault(x => x.Id == GateId && x.IsActive == true);
             var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true &&
                            (user == null || (user != null && x.OrganizationId == user.OrganizationId))).AsQueryable();

             var getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();
             var result = (from users in getUsers
                           join roles in getRoles on users.Roles.First().RoleId equals roles.Id
                           where roles.Name == "Security"
                           select new DropDownVM
                           {
                               Id = users.Id,
                               Name = users.FullName
                           }).AsQueryable();

             return result;
         }
         [Route("~/Api/Report/GetShifts")]
         [HttpGet]
         public IEnumerable<GeneralDropDownVM> GetShifts()
         {

             var result = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true )
              .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.ShitfName });
             return result;

         }

        [Route("~/Api/Report/GetShiftDetailsGrid")]
        [HttpPost]
         public string GetShiftDataGrid(string search, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "";
            }

            int totalCount = 0;
            pageIndex = (pageIndex - 1);

            var result = _ShiftDetailsReportHelper.GetShiftData(search, pageIndex, pageSize, sortField, sortOrder, out totalCount);

            var jsonData = JsonConvert.SerializeObject(result.OrderBy(x => x.UserName));
            return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
        }
           


        }
         
        //    //Reports.DataSet.ShiftDetailReportDataset _shiftDetailReportDataset;
        //    //_shiftDetailReportDataset = new Reports.DataSet.ShiftDetailReportDataset();

        //    //foreach (var item in shiftDetails)
        //    //{
        //    //    var AddshiftRow = _shiftDetailReportDataset.ShiftDetailDatatable.NewShiftDetailDatatableRow();
        //    //    AddshiftRow.FullName = item.UserName;
        //    //    AddshiftRow.BuildingName = item.BuildingName;
        //    //    AddshiftRow.GateNumber = item.GateName;
        //    //    AddshiftRow.ShiftName = item.ShiftName;
        //    //    AddshiftRow.FromDate = item.FromDate.ToString();
        //    //    AddshiftRow.ToDate = item.ToDate.ToString();

        //    //    _shiftDetailReportDataset.ShiftDetailDatatable.AddShiftDetailDatatableRow(AddshiftRow);

        //    //}

            //return shiftDetails.ToList();
        //}
        
    
}
