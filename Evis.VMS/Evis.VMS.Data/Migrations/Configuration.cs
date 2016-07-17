/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

namespace Evis.VMS.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Evis.VMS.Data.DBContext;
    using Evis.VMS.Data.Model.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<Evis.VMS.Data.DBContext.VMSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(VMSContext context)
        {
            GenerateGender(context);
            GenerateRoles(context);
            GenerateCardType(context);
            GenerateSystemAdmin(context);
            context.SaveChanges();
        }

        private static void GenerateGender(VMSContext context)
        {
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LookUpType OFF");
            //For Gender
            context.LookUpType.Add(new LookUpType { Id = 1, TypeName = "Gender", TypeCode = "Gender", Description = "Gender", IsActive = true });
            // For Country
            context.LookUpType.Add(new LookUpType { Id = 2, TypeName = "Country", TypeCode = "Country", Description = "Country", IsActive = true });
            // For State
            context.LookUpType.Add(new LookUpType { Id = 3, TypeName = "State", TypeCode = "State", Description = "State", IsActive = true });
            // For City
            context.LookUpType.Add(new LookUpType { Id = 4, TypeName = "City", TypeCode = "City", Description = "City", IsActive = true });
            context.LookUpType.Add(new LookUpType { Id = 5, TypeName = "TypeOfCards", TypeCode = "TypeOfCards", Description = "Type Of Cards", IsActive = true });
            context.LookUpType.Add(new LookUpType { Id = 6, TypeName = "Nationalities", TypeCode = "Nationalities", Description = "Nationalities", IsActive = true });
            context.LookUpType.Add(new LookUpType { Id = 7, TypeName = "Theme", TypeCode = "Theme", Description = "Theme", IsActive = true });

            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LookUpType ON");
            //context.SaveChanges();

            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LookUpValues OFF");
            // Inserting the gender values
            context.LookUpValues.Add(new LookUpValues { Id = 1, LookUpTypeId = 1, LookUpValue = "Male", Description = "Male", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 2, LookUpTypeId = 1, LookUpValue = "Female", Description = "Female", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 3, LookUpTypeId = 1, LookUpValue = "Others", Description = "Other", IsActive = true });

            // Inserting the country values
            context.LookUpValues.Add(new LookUpValues { Id = 4, LookUpTypeId = 2, LookUpValue = "India", Description = "India", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 5, LookUpTypeId = 2, LookUpValue = "UAE", Description = "UAE", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 6, LookUpTypeId = 2, LookUpValue = "Saudi Arabia", Description = "Saudi Arabia", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 7, LookUpTypeId = 2, LookUpValue = "Oman", Description = "Oman", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 8, LookUpTypeId = 2, LookUpValue = "Qatar", Description = "Qatar", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 9, LookUpTypeId = 2, LookUpValue = "Bahrain", Description = "Bahrain", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 10, LookUpTypeId = 2, LookUpValue = "Kuwait", Description = "Kuwait", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 11, LookUpTypeId = 2, LookUpValue = "Others", Description = "Others", IsActive = true });

            // Inserting the state values
            context.LookUpValues.Add(new LookUpValues { Id = 12, LookUpTypeId = 3, LookUpValue = "Karnataka", Description = "Karnataka", IsActive = true, ParentId = 4 });
            context.LookUpValues.Add(new LookUpValues { Id = 13, LookUpTypeId = 3, LookUpValue = "Kerala", Description = "Kerala", IsActive = true, ParentId = 4 });
            context.LookUpValues.Add(new LookUpValues { Id = 14, LookUpTypeId = 3, LookUpValue = "Andhra Pradesh", Description = "Andhra Pradesh", IsActive = true, ParentId = 4 });
            context.LookUpValues.Add(new LookUpValues { Id = 15, LookUpTypeId = 3, LookUpValue = "Tamil Nadu", Description = "Tamil Nadu", IsActive = true, ParentId = 4 });

            context.LookUpValues.Add(new LookUpValues { Id = 16, LookUpTypeId = 3, LookUpValue = "Dubai", Description = "Dubai", IsActive = true, ParentId = 5 });
            context.LookUpValues.Add(new LookUpValues { Id = 17, LookUpTypeId = 3, LookUpValue = "Abu Dhabi", Description = "Abu Dhabi", IsActive = true, ParentId = 5 });
            context.LookUpValues.Add(new LookUpValues { Id = 18, LookUpTypeId = 3, LookUpValue = "Sharjah", Description = "Sharjah", IsActive = true, ParentId = 5 });
            context.LookUpValues.Add(new LookUpValues { Id = 19, LookUpTypeId = 3, LookUpValue = "Ajmaan", Description = "Ajmaan", IsActive = true, ParentId = 5 });

            context.LookUpValues.Add(new LookUpValues { Id = 20, LookUpTypeId = 3, LookUpValue = "Mecca", Description = "Mecca", IsActive = true, ParentId = 6 });
            context.LookUpValues.Add(new LookUpValues { Id = 21, LookUpTypeId = 3, LookUpValue = "Madina", Description = "Madina", IsActive = true, ParentId = 6 });
            context.LookUpValues.Add(new LookUpValues { Id = 22, LookUpTypeId = 3, LookUpValue = "Riyadh", Description = "Riyadh", IsActive = true, ParentId = 6 });
            context.LookUpValues.Add(new LookUpValues { Id = 23, LookUpTypeId = 3, LookUpValue = "Dammam", Description = "Tamil Nadu", IsActive = true, ParentId = 6 });

            // Inserting the city values
            context.LookUpValues.Add(new LookUpValues { Id = 24, LookUpTypeId = 4, LookUpValue = "Mysore", Description = "Mysore", IsActive = true, ParentId = 12 });
            context.LookUpValues.Add(new LookUpValues { Id = 25, LookUpTypeId = 4, LookUpValue = "Mandya", Description = "Mandya", IsActive = true, ParentId = 12 });
            context.LookUpValues.Add(new LookUpValues { Id = 26, LookUpTypeId = 4, LookUpValue = "Bangalore", Description = "Bangalore", IsActive = true, ParentId = 12 });
            context.LookUpValues.Add(new LookUpValues { Id = 27, LookUpTypeId = 4, LookUpValue = "Mangalore", Description = "Mangalore", IsActive = true, ParentId = 12 });

            context.LookUpValues.Add(new LookUpValues { Id = 28, LookUpTypeId = 4, LookUpValue = "Dubai Medina City", Description = "Dubai Medina City", IsActive = true, ParentId = 16 });
            context.LookUpValues.Add(new LookUpValues { Id = 29, LookUpTypeId = 4, LookUpValue = "Jabal Ali", Description = "Jabal Ali", IsActive = true, ParentId = 16 });
            context.LookUpValues.Add(new LookUpValues { Id = 30, LookUpTypeId = 4, LookUpValue = "Al Barsha", Description = "Al Barsha", IsActive = true, ParentId = 16 });
            context.LookUpValues.Add(new LookUpValues { Id = 31, LookUpTypeId = 4, LookUpValue = "Bur Dubai", Description = "Bur Dubai", IsActive = true, ParentId = 16 });

            context.LookUpValues.Add(new LookUpValues { Id = 32, LookUpTypeId = 5, LookUpValue = "Emirates Id", Description = "Emirates Id", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 33, LookUpTypeId = 5, LookUpValue = "Driving Licence", Description = "Driving Licence", IsActive = true});
            context.LookUpValues.Add(new LookUpValues { Id = 34, LookUpTypeId = 5, LookUpValue = "Others", Description = "Others", IsActive = true });

            context.LookUpValues.Add(new LookUpValues { Id = 35, LookUpTypeId = 6, LookUpValue = "Emirates", Description = "Emirates", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 36, LookUpTypeId = 6, LookUpValue = "Indian", Description = "Indian", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 37, LookUpTypeId = 6, LookUpValue = "American", Description = "American", IsActive = true});
            context.LookUpValues.Add(new LookUpValues { Id = 38, LookUpTypeId = 6, LookUpValue = "Other", Description = "Other", IsActive = true });

            //inserting theme
            context.LookUpValues.Add(new LookUpValues { Id = 39, LookUpTypeId = 7, LookUpValue = "theme1", Description = "Theme1", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 40, LookUpTypeId = 7, LookUpValue = "theme2", Description = "Theme2", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 41, LookUpTypeId = 7, LookUpValue = "theme3", Description = "Theme3", IsActive = true });

            context.LookUpValues.Add(new LookUpValues { Id = 42, LookUpTypeId = 7, LookUpValue = "theme4", Description = "Theme4", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 43, LookUpTypeId = 7, LookUpValue = "theme5", Description = "Theme5", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 44, LookUpTypeId = 7, LookUpValue = "theme6", Description = "Theme6", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 45, LookUpTypeId = 7, LookUpValue = "theme7", Description = "Theme7", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 46, LookUpTypeId = 7, LookUpValue = "theme8", Description = "Theme8", IsActive = true });
           

            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT LookUpValues ON");
        }
        

        private static void GenerateRoles(VMSContext context)
        {
            context.Roles.Add(new ApplicationRole { Name = "Supervisor", Description = "Supervisor" });
            context.Roles.Add(new ApplicationRole { Name = "Security", Description = "Security" });
        }

        private void GenerateSystemAdmin(VMSContext context)
        {
            var systemAdminrole = context.Roles.Add(new ApplicationRole { Name = "SuperAdmin", Description = "SuperAdmin" });

            var newSystemAdminUser = new ApplicationUser
            {
                FullName = "Super Admin",
                Email = "superadmin@evisuae.com",
                PhoneNumber = "1234567890",
                UserName = "superadmin@evisuae.com",
                GenderId = 1,
                Nationality = 3,
                IsActive = true,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = System.Guid.NewGuid().ToString()
            };

            var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher();
            var hashedPassword = passwordHash.HashPassword("Admin@123");
            newSystemAdminUser.PasswordHash = hashedPassword;

            var systemAdminUser = context.Users.Add(newSystemAdminUser);

            systemAdminUser.Roles.Add(
                new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole
                {
                    UserId = systemAdminUser.Id,
                    RoleId = systemAdminrole.Id
                });

            context.Users.AddOrUpdate(systemAdminUser);
        }
        private static void GenerateCardType(VMSContext context)
        {
            context.CardTypeMaster.Add(new CardTypeMaster { Id = 1, CardName = "EmiratesId", Description = "EmiratesId", IsActive = true });
            context.CardTypeMaster.Add(new CardTypeMaster { Id = 2, CardName = "Driving Licence", Description = "Driving Licence", IsActive = true });
            context.CardTypeMaster.Add(new CardTypeMaster { Id = 3, CardName = "Passport", Description = "Passport", IsActive = true });
            context.CardTypeMaster.Add(new CardTypeMaster { Id = 4, CardName = "Others", Description = "Others", IsActive = true });
        }
    }
}
