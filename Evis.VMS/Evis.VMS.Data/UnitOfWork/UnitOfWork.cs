/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Context;
using Evis.VMS.Data.Contract;
using Evis.VMS.Data.DBContext;
using Evis.VMS.Data.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {

        #region Private member variables...

        protected VMSContext _DataContext;
        private IRepository<BuildingMaster> _buildingMaster;
        private IRepository<CardTypeMaster> _cardTypeMaster;
        private IRepository<EmailFormats> _emailFormats;
        private IRepository<GateMaster> _gateMaster;
        private IRepository<LookUpType> _lookUpType;
        private IRepository<LookUpValues> _lookUpValues;
        private IRepository<Organization> _organization;
        private IRepository<ShitfAssignment> _shitfAssignment;
        private IRepository<ShitfMaster> _shitfMaster;
        private IRepository<VisitDetails> _visitDetails;
        private IRepository<VisitorCardTypeDetails> _visitorCardTypeDetails;
        private IRepository<VisitorMaster> _visitorMaster;

        #endregion

        #region Constructor

        public UnitOfWork()
        {
            _DataContext = new VMSContext();
        }


        #endregion

        #region Public IRepository Creation properties...

        public IRepository<BuildingMaster> BuildingMaster
        {
            get
            {
                if (this._buildingMaster == null)
                {
                    this._buildingMaster = new Repository<BuildingMaster>(_DataContext);
                }
                return _buildingMaster;
            }
        }

        public IRepository<CardTypeMaster> CardTypeMaster
        {
            get
            {
                if (this._cardTypeMaster == null)
                {
                    this._cardTypeMaster = new Repository<CardTypeMaster>(_DataContext);
                }
                return _cardTypeMaster;
            }
        }

        public IRepository<EmailFormats> EmailFormats
        {
            get
            {
                if (this._emailFormats == null)
                {
                    this._emailFormats = new Repository<EmailFormats>(_DataContext);
                }
                return _emailFormats;
            }
        }

        public IRepository<GateMaster> GateMaster
        {
            get
            {
                if (this._gateMaster == null)
                {
                    this._gateMaster = new Repository<GateMaster>(_DataContext);
                }
                return _gateMaster;
            }
        }

        public IRepository<LookUpType> LookUpType
        {
            get
            {
                if (this._lookUpType == null)
                {
                    this._lookUpType = new Repository<LookUpType>(_DataContext);
                }
                return _lookUpType;
            }
        }

        public IRepository<LookUpValues> LookUpValues
        {
            get
            {
                if (this._lookUpValues == null)
                {
                    this._lookUpValues = new Repository<LookUpValues>(_DataContext);
                }
                return _lookUpValues;
            }
        }

        public IRepository<Organization> Organization
        {
            get
            {
                if (this._organization == null)
                {
                    this._organization = new Repository<Organization>(_DataContext);
                }
                return _organization;
            }
        }
        public IRepository<ShitfAssignment> ShitfAssignment
        {
            get
            {
                if (this._shitfAssignment == null)
                {
                    this._shitfAssignment = new Repository<ShitfAssignment>(_DataContext);
                }
                return _shitfAssignment;
            }
        }
        public IRepository<ShitfMaster> ShitfMaster
        {
            get
            {
                if (this._shitfMaster == null)
                {
                    this._shitfMaster = new Repository<ShitfMaster>(_DataContext);
                }
                return _shitfMaster;
            }
        }

        public IRepository<VisitDetails> VisitDetails
        {
            get
            {
                if (this._visitDetails == null)
                {
                    this._visitDetails = new Repository<VisitDetails>(_DataContext);
                }
                return _visitDetails;
            }
        }

        public IRepository<VisitorCardTypeDetails> VisitorCardTypeDetails
        {
            get
            {
                if (this._visitorCardTypeDetails == null)
                {
                    this._visitorCardTypeDetails = new Repository<VisitorCardTypeDetails>(_DataContext);
                }
                return _visitorCardTypeDetails;
            }
        }

        public IRepository<VisitorMaster> VisitorMaster
        {
            get
            {
                if (this._visitorMaster == null)
                {
                    this._visitorMaster = new Repository<VisitorMaster>(_DataContext);
                }
                return _visitorMaster;
            }
        }

        #endregion

        #region Public member methods...

        /// <summary>
        /// Commits the unit of work
        /// </summary>
        public void Commit()
        {
            _DataContext.SaveChanges();
        }
        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _DataContext.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
