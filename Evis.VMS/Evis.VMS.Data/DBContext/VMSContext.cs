/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.DBContext
{
    public class VMSContext : IdentityDbContext<ApplicationUser>
    {
        public VMSContext()
            : base("VMSContext")
        {
            Database.SetInitializer<VMSContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<BuildingMaster> BuildingMaster { get; set; }

        public DbSet<CardTypeMaster> CardTypeMaster { get; set; }

        public DbSet<EmailFormats> EmailFormats { get; set; }

        public DbSet<GateMaster> GateMaster { get; set; }

        public DbSet<LookUpType> LookUpType { get; set; }

        public DbSet<LookUpValues> LookUpValues { get; set; }

        public DbSet<Organization> Organization { get; set; }

        public DbSet<ShitfAssignment> ShitfAssignment { get; set; }

        public DbSet<ShitfMaster> ShitfMaster { get; set; }

        public DbSet<VisitDetails> VisitDetails { get; set; }

        public DbSet<VisitorCardTypeDetails> VisitorCardTypeDetails { get; set; }

        public DbSet<VisitorMaster> VisitorMaster { get; set; }

        public DbSet<ShiftDetails> ShiftDetails { get; set; }



    }
}
