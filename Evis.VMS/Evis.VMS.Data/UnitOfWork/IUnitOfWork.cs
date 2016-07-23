/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Contract;
using Evis.VMS.Data.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits the unit of work
        /// </summary>
        void Commit();

        IRepository<BuildingMaster> BuildingMaster { get; }

        IRepository<CardTypeMaster> CardTypeMaster { get; }

        IRepository<EmailFormats> EmailFormats { get; }

        IRepository<GateMaster> GateMaster { get; }

        IRepository<LookUpType> LookUpType { get; }

        IRepository<LookUpValues> LookUpValues { get; }

        IRepository<Organization> Organization { get; }

        IRepository<ShitfAssignment> ShitfAssignment { get; }

        IRepository<ShitfMaster> ShitfMaster { get; }

        IRepository<VisitDetails> VisitDetails { get; }
        IRepository<VisitorCardTypeDetails> VisitorCardTypeDetails { get; }
        IRepository<VisitorMaster> VisitorMaster { get; }
    }
}
