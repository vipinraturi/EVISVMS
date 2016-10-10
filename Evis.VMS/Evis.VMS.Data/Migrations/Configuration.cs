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
            GenerateEmailFormat(context);
            context.SaveChanges();
        }

        private void GenerateEmailFormat(VMSContext context)
        {
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT EmailFormats OFF");

            context.EmailFormats.Add(new EmailFormats { Id = 1, Category = "ForgotPassword", IsActive = true, Format = "Dear {0}, <br/>Please click <a href={1}>here</a> to reset the password.<br/><br/><br/>Regards,<br/>Administrator" });
            context.EmailFormats.Add(new EmailFormats { Id = 2, Category = "UserCreation", IsActive = true, Format = "Dear {0}, <br/>Your account has been created, click <a href={1}>here</a> to activate the account.<br/>Use the below credentials after successfull activation. <br/>UserName: {2} <br/>Password: {3}<br/><br/>Regards,<br/>Administrator" });
            context.EmailFormats.Add(new EmailFormats { Id = 3, Category = "OrganizationCreation", IsActive = true, Format = "Dear Sir/Madam, <br/><br/>Your company with the name {0} has been created successfully.<br/><br/>Regards,<br/>Administrator" });
            context.EmailFormats.Add(new EmailFormats { Id = 4, Category = "PasswordReset", IsActive = true, Format = "Dear {0}, <br/>Your account password has changed successfully.<br/><br/>Regards,<br/>Administrator" });

            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT EmailFormats ON");
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
            context.LookUpValues.Add(new LookUpValues { Id = 23, LookUpTypeId = 3, LookUpValue = "Dammam", Description = "Dammam", IsActive = true, ParentId = 6 });

            context.LookUpValues.Add(new LookUpValues { Id = 53, LookUpTypeId = 3, LookUpValue = "Az Zahirah", Description = "Az Zahirah", IsActive = true, ParentId = 7 });
            context.LookUpValues.Add(new LookUpValues { Id = 54, LookUpTypeId = 3, LookUpValue = "Muscat", Description = "Muscat", IsActive = true, ParentId = 7 });
            context.LookUpValues.Add(new LookUpValues { Id = 55, LookUpTypeId = 3, LookUpValue = "Sohar", Description = "Sohar", IsActive = true, ParentId = 7 });
            context.LookUpValues.Add(new LookUpValues { Id = 56, LookUpTypeId = 3, LookUpValue = "Salalah", Description = "Salalah", IsActive = true, ParentId = 7 });

            context.LookUpValues.Add(new LookUpValues { Id = 57, LookUpTypeId = 3, LookUpValue = "Al Khor", Description = "Al Khor", IsActive = true, ParentId = 8 });
            context.LookUpValues.Add(new LookUpValues { Id = 58, LookUpTypeId = 3, LookUpValue = "Al Daayen", Description = "Al Daayen", IsActive = true, ParentId = 8 });
            context.LookUpValues.Add(new LookUpValues { Id = 59, LookUpTypeId = 3, LookUpValue = "Doha", Description = "Doha", IsActive = true, ParentId = 8 });
            context.LookUpValues.Add(new LookUpValues { Id = 60, LookUpTypeId = 3, LookUpValue = "Al Wakrah", Description = "Al Wakrah", IsActive = true, ParentId = 8 });

            context.LookUpValues.Add(new LookUpValues { Id = 61, LookUpTypeId = 3, LookUpValue = "Salmiya", Description = "Salmiya", IsActive = true, ParentId = 9 });
            context.LookUpValues.Add(new LookUpValues { Id = 62, LookUpTypeId = 3, LookUpValue = "Riffa", Description = "Riffa", IsActive = true, ParentId = 9 });
            context.LookUpValues.Add(new LookUpValues { Id = 63, LookUpTypeId = 3, LookUpValue = "Bayan", Description = "Bayan", IsActive = true, ParentId = 9 });
            context.LookUpValues.Add(new LookUpValues { Id = 64, LookUpTypeId = 3, LookUpValue = "Al-Jabriya", Description = "Al-Jabriya", IsActive = true, ParentId = 9 });

            context.LookUpValues.Add(new LookUpValues { Id = 65, LookUpTypeId = 3, LookUpValue = "Manama", Description = "Manama", IsActive = true, ParentId = 10 });
            context.LookUpValues.Add(new LookUpValues { Id = 66, LookUpTypeId = 3, LookUpValue = "Hawalli", Description = "Hawalli", IsActive = true, ParentId = 10 });
            context.LookUpValues.Add(new LookUpValues { Id = 67, LookUpTypeId = 3, LookUpValue = "Sitra", Description = "Sitra", IsActive = true, ParentId = 10 });
            context.LookUpValues.Add(new LookUpValues { Id = 68, LookUpTypeId = 3, LookUpValue = "Gudaibiya", Description = "Gudaibiya", IsActive = true, ParentId = 10 });

            // Inserting the city values
            context.LookUpValues.Add(new LookUpValues { Id = 24, LookUpTypeId = 4, LookUpValue = "Mysore", Description = "Mysore", IsActive = true, ParentId = 12 });
            context.LookUpValues.Add(new LookUpValues { Id = 25, LookUpTypeId = 4, LookUpValue = "Mandya", Description = "Mandya", IsActive = true, ParentId = 12 });
            context.LookUpValues.Add(new LookUpValues { Id = 26, LookUpTypeId = 4, LookUpValue = "Bangalore", Description = "Bangalore", IsActive = true, ParentId = 12 });
            context.LookUpValues.Add(new LookUpValues { Id = 27, LookUpTypeId = 4, LookUpValue = "Mangalore", Description = "Mangalore", IsActive = true, ParentId = 12 });

            context.LookUpValues.Add(new LookUpValues { Id = 69, LookUpTypeId = 4, LookUpValue = "Trivandrum", Description = "Trivandrum", IsActive = true, ParentId = 13 });
            context.LookUpValues.Add(new LookUpValues { Id = 70, LookUpTypeId = 4, LookUpValue = "Kochi", Description = "Kochi", IsActive = true, ParentId = 13 });
            context.LookUpValues.Add(new LookUpValues { Id = 71, LookUpTypeId = 4, LookUpValue = "Thrissur", Description = "Thrissur", IsActive = true, ParentId = 13 });
            context.LookUpValues.Add(new LookUpValues { Id = 72, LookUpTypeId = 4, LookUpValue = "Alappuzha", Description = "Alappuzha", IsActive = true, ParentId = 13 });

            context.LookUpValues.Add(new LookUpValues { Id = 73, LookUpTypeId = 4, LookUpValue = "Guntur", Description = "Guntur", IsActive = true, ParentId = 14 });
            context.LookUpValues.Add(new LookUpValues { Id = 74, LookUpTypeId = 4, LookUpValue = "Kakinada", Description = "Kakinada", IsActive = true, ParentId = 14 });
            context.LookUpValues.Add(new LookUpValues { Id = 75, LookUpTypeId = 4, LookUpValue = "Visakhapatnam", Description = "Visakhapatnam", IsActive = true, ParentId = 14 });
            context.LookUpValues.Add(new LookUpValues { Id = 76, LookUpTypeId = 4, LookUpValue = "Nellore", Description = "Nellore", IsActive = true, ParentId = 14 });

            context.LookUpValues.Add(new LookUpValues { Id = 77, LookUpTypeId = 4, LookUpValue = "Chennai", Description = "Chennai", IsActive = true, ParentId = 15 });
            context.LookUpValues.Add(new LookUpValues { Id = 78, LookUpTypeId = 4, LookUpValue = "Coimbatore", Description = "Coimbatore", IsActive = true, ParentId = 15 });
            context.LookUpValues.Add(new LookUpValues { Id = 79, LookUpTypeId = 4, LookUpValue = "Tiruchirappalli", Description = "Tiruchirappalli", IsActive = true, ParentId = 15 });
            context.LookUpValues.Add(new LookUpValues { Id = 80, LookUpTypeId = 4, LookUpValue = "Salem", Description = "Salem", IsActive = true, ParentId = 15 });

            context.LookUpValues.Add(new LookUpValues { Id = 28, LookUpTypeId = 4, LookUpValue = "Dubai Medina City", Description = "Dubai Medina City", IsActive = true, ParentId = 16 });
            context.LookUpValues.Add(new LookUpValues { Id = 29, LookUpTypeId = 4, LookUpValue = "Jabal Ali", Description = "Jabal Ali", IsActive = true, ParentId = 16 });
            context.LookUpValues.Add(new LookUpValues { Id = 30, LookUpTypeId = 4, LookUpValue = "Al Barsha", Description = "Al Barsha", IsActive = true, ParentId = 16 });
            context.LookUpValues.Add(new LookUpValues { Id = 31, LookUpTypeId = 4, LookUpValue = "Bur Dubai", Description = "Bur Dubai", IsActive = true, ParentId = 16 });

            context.LookUpValues.Add(new LookUpValues { Id = 81, LookUpTypeId = 4, LookUpValue = "Mussaffah", Description = "Mussaffah", IsActive = true, ParentId = 17 });
            context.LookUpValues.Add(new LookUpValues { Id = 82, LookUpTypeId = 4, LookUpValue = "Ruwais", Description = "Ruwais", IsActive = true, ParentId = 17 });
            context.LookUpValues.Add(new LookUpValues { Id = 83, LookUpTypeId = 4, LookUpValue = "Madinat Zayed", Description = "Madinat Zayed", IsActive = true, ParentId = 17 });
            context.LookUpValues.Add(new LookUpValues { Id = 84, LookUpTypeId = 4, LookUpValue = "MBZ City", Description = "MBZ City", IsActive = true, ParentId = 17 });

            context.LookUpValues.Add(new LookUpValues { Id = 85, LookUpTypeId = 4, LookUpValue = "Al Nahda", Description = "Al Nahda", IsActive = true, ParentId = 18 });
            context.LookUpValues.Add(new LookUpValues { Id = 86, LookUpTypeId = 4, LookUpValue = "Rolla", Description = "Rolla", IsActive = true, ParentId = 18 });
            context.LookUpValues.Add(new LookUpValues { Id = 87, LookUpTypeId = 4, LookUpValue = "Al Khan", Description = "Al Khan", IsActive = true, ParentId = 18 });
            context.LookUpValues.Add(new LookUpValues { Id = 88, LookUpTypeId = 4, LookUpValue = "Abu Shagara", Description = "Abu Shagara", IsActive = true, ParentId = 18 });

            context.LookUpValues.Add(new LookUpValues { Id = 89, LookUpTypeId = 4, LookUpValue = "Ajman Corniche", Description = "Ajman Corniche", IsActive = true, ParentId = 19 });
            context.LookUpValues.Add(new LookUpValues { Id = 90, LookUpTypeId = 4, LookUpValue = "Ajman Port", Description = "Ajman Port", IsActive = true, ParentId = 19 });
            context.LookUpValues.Add(new LookUpValues { Id = 91, LookUpTypeId = 4, LookUpValue = "Al Zora", Description = "Al Zora", IsActive = true, ParentId = 19 });
            context.LookUpValues.Add(new LookUpValues { Id = 92, LookUpTypeId = 4, LookUpValue = "Masfout", Description = "Masfout", IsActive = true, ParentId = 19 });

            context.LookUpValues.Add(new LookUpValues { Id = 93, LookUpTypeId = 4, LookUpValue = "Al Jumum", Description = "Al Jumum", IsActive = true, ParentId = 20 });
            context.LookUpValues.Add(new LookUpValues { Id = 94, LookUpTypeId = 4, LookUpValue = "Al Kamil", Description = "Al Kamil", IsActive = true, ParentId = 20 });
            context.LookUpValues.Add(new LookUpValues { Id = 95, LookUpTypeId = 4, LookUpValue = "Al Khurmah", Description = "Al Khurmah", IsActive = true, ParentId = 20 });
            context.LookUpValues.Add(new LookUpValues { Id = 96, LookUpTypeId = 4, LookUpValue = "Ranyah", Description = "Ranyah", IsActive = true, ParentId = 20 });

            context.LookUpValues.Add(new LookUpValues { Id = 97, LookUpTypeId = 4, LookUpValue = "Al Hunakiyah", Description = "Al Hunakiyah", IsActive = true, ParentId = 21 });
            context.LookUpValues.Add(new LookUpValues { Id = 98, LookUpTypeId = 4, LookUpValue = "Mahd Al Thahab", Description = "Mahd Al Thahab", IsActive = true, ParentId = 21 });
            context.LookUpValues.Add(new LookUpValues { Id = 99, LookUpTypeId = 4, LookUpValue = "Badr", Description = "Badr", IsActive = true, ParentId = 21 });
            context.LookUpValues.Add(new LookUpValues { Id = 100, LookUpTypeId = 4, LookUpValue = "Khaybar", Description = "Khaybar", IsActive = true, ParentId = 21 });

            context.LookUpValues.Add(new LookUpValues { Id = 101, LookUpTypeId = 4, LookUpValue = "Rimah", Description = "Rimah", IsActive = true, ParentId = 22 });
            context.LookUpValues.Add(new LookUpValues { Id = 102, LookUpTypeId = 4, LookUpValue = "al-Ghat", Description = "al-Ghat", IsActive = true, ParentId = 22 });
            context.LookUpValues.Add(new LookUpValues { Id = 103, LookUpTypeId = 4, LookUpValue = "Al-Kharj", Description = "Al-Kharj", IsActive = true, ParentId = 22 });
            context.LookUpValues.Add(new LookUpValues { Id = 104, LookUpTypeId = 4, LookUpValue = "Shagra", Description = "Shagra", IsActive = true, ParentId = 22 });

            context.LookUpValues.Add(new LookUpValues { Id = 105, LookUpTypeId = 4, LookUpValue = "Dammam", Description = "Dammam", IsActive = true, ParentId = 23 });
            context.LookUpValues.Add(new LookUpValues { Id = 106, LookUpTypeId = 4, LookUpValue = "Dhahran", Description = "Dhahran", IsActive = true, ParentId = 23 });
            context.LookUpValues.Add(new LookUpValues { Id = 107, LookUpTypeId = 4, LookUpValue = "Khobar", Description = "Khobar", IsActive = true, ParentId = 23 });
            context.LookUpValues.Add(new LookUpValues { Id = 108, LookUpTypeId = 4, LookUpValue = "Ras Tanura", Description = "Ras Tanura", IsActive = true, ParentId = 23 });

            context.LookUpValues.Add(new LookUpValues { Id = 109, LookUpTypeId = 4, LookUpValue = "Hayyal", Description = "Hayyal", IsActive = true, ParentId = 53 });
            context.LookUpValues.Add(new LookUpValues { Id = 110, LookUpTypeId = 4, LookUpValue = "Mushaqasah", Description = "Mushaqasah", IsActive = true, ParentId = 53 });
            context.LookUpValues.Add(new LookUpValues { Id = 111, LookUpTypeId = 4, LookUpValue = "Al Khalw", Description = "Al Khalw", IsActive = true, ParentId = 53 });
            context.LookUpValues.Add(new LookUpValues { Id = 112, LookUpTypeId = 4, LookUpValue = "Khuldah", Description = "Khuldah", IsActive = true, ParentId = 53 });

            context.LookUpValues.Add(new LookUpValues { Id = 113, LookUpTypeId = 4, LookUpValue = "Bawshar", Description = "Bawshar", IsActive = true, ParentId = 54 });
            context.LookUpValues.Add(new LookUpValues { Id = 114, LookUpTypeId = 4, LookUpValue = "Al-Amrat", Description = "Al-Amrat", IsActive = true, ParentId = 54 });
            context.LookUpValues.Add(new LookUpValues { Id = 115, LookUpTypeId = 4, LookUpValue = "Masqat[Muscat]", Description = "Masqat[Muscat]", IsActive = true, ParentId = 54 });
            context.LookUpValues.Add(new LookUpValues { Id = 116, LookUpTypeId = 4, LookUpValue = "Qurayyat", Description = "Qurayyat", IsActive = true, ParentId = 54 });

            context.LookUpValues.Add(new LookUpValues { Id = 117, LookUpTypeId = 4, LookUpValue = "Sohar Gate", Description = "Sohar Gate", IsActive = true, ParentId = 55 });
            context.LookUpValues.Add(new LookUpValues { Id = 118, LookUpTypeId = 4, LookUpValue = "Bahjat Al Anthar", Description = "Bahjat Al Anthar", IsActive = true, ParentId = 55 });
            context.LookUpValues.Add(new LookUpValues { Id = 119, LookUpTypeId = 4, LookUpValue = "Sohar Fort", Description = "Sohar Fort", IsActive = true, ParentId = 55 });
            context.LookUpValues.Add(new LookUpValues { Id = 120, LookUpTypeId = 4, LookUpValue = "Al Hujra", Description = "Al Hujra", IsActive = true, ParentId = 55 });

            context.LookUpValues.Add(new LookUpValues { Id = 121, LookUpTypeId = 4, LookUpValue = "New Salalah", Description = "New Salalah", IsActive = true, ParentId = 56 });
            context.LookUpValues.Add(new LookUpValues { Id = 122, LookUpTypeId = 4, LookUpValue = "Auqad", Description = "Auqad", IsActive = true, ParentId = 56 });
            context.LookUpValues.Add(new LookUpValues { Id = 123, LookUpTypeId = 4, LookUpValue = "Al-Haffa", Description = "Al-Haffa", IsActive = true, ParentId = 56 });
            context.LookUpValues.Add(new LookUpValues { Id = 124, LookUpTypeId = 4, LookUpValue = "Al-Mutaaza", Description = "Al-Mutaaza", IsActive = true, ParentId = 56 });

            context.LookUpValues.Add(new LookUpValues { Id = 125, LookUpTypeId = 4, LookUpValue = "Simaisma", Description = "Simaisma", IsActive = true, ParentId = 57 });
            context.LookUpValues.Add(new LookUpValues { Id = 126, LookUpTypeId = 4, LookUpValue = "Al Khor City", Description = "Al Khor City", IsActive = true, ParentId = 57 });
            context.LookUpValues.Add(new LookUpValues { Id = 127, LookUpTypeId = 4, LookUpValue = "Al Thakhira", Description = "Al Thakhira", IsActive = true, ParentId = 57 });
            context.LookUpValues.Add(new LookUpValues { Id = 128, LookUpTypeId = 4, LookUpValue = "Ras Laffan", Description = "Ras Laffan", IsActive = true, ParentId = 57 });

            context.LookUpValues.Add(new LookUpValues { Id = 129, LookUpTypeId = 4, LookUpValue = "Leabaib", Description = "Leabaib", IsActive = true, ParentId = 58 });
            context.LookUpValues.Add(new LookUpValues { Id = 130, LookUpTypeId = 4, LookUpValue = "Jeryan Jenaihat", Description = "Jeryan Jenaihat", IsActive = true, ParentId = 58 });
            context.LookUpValues.Add(new LookUpValues { Id = 131, LookUpTypeId = 4, LookUpValue = "Al Sakhama", Description = "Al Sakhama", IsActive = true, ParentId = 58 });
            context.LookUpValues.Add(new LookUpValues { Id = 132, LookUpTypeId = 4, LookUpValue = "Lusail", Description = "Lusail", IsActive = true, ParentId = 58 });

            context.LookUpValues.Add(new LookUpValues { Id = 133, LookUpTypeId = 4, LookUpValue = "Al Bidda", Description = "Al Bidda", IsActive = true, ParentId = 59 });
            context.LookUpValues.Add(new LookUpValues { Id = 134, LookUpTypeId = 4, LookUpValue = "Al Dafna", Description = "Al Dafna", IsActive = true, ParentId = 59 });
            context.LookUpValues.Add(new LookUpValues { Id = 135, LookUpTypeId = 4, LookUpValue = "Al Ghanim", Description = "Al Ghanim", IsActive = true, ParentId = 59 });
            context.LookUpValues.Add(new LookUpValues { Id = 136, LookUpTypeId = 4, LookUpValue = "Najma", Description = "Najma", IsActive = true, ParentId = 59 });

            context.LookUpValues.Add(new LookUpValues { Id = 137, LookUpTypeId = 4, LookUpValue = "Al Wukair", Description = "Al Wukair", IsActive = true, ParentId = 60 });
            context.LookUpValues.Add(new LookUpValues { Id = 138, LookUpTypeId = 4, LookUpValue = "Mesaieed", Description = "Mesaieed", IsActive = true, ParentId = 60 });
            context.LookUpValues.Add(new LookUpValues { Id = 139, LookUpTypeId = 4, LookUpValue = "Al Kharrara", Description = "Al Kharrara", IsActive = true, ParentId = 60 });
            context.LookUpValues.Add(new LookUpValues { Id = 140, LookUpTypeId = 4, LookUpValue = "Shagra", Description = "Shagra", IsActive = true, ParentId = 60 });

            context.LookUpValues.Add(new LookUpValues { Id = 141, LookUpTypeId = 4, LookUpValue = "Abu Thur Al-Ghafari St.", Description = "Abu Thur Al-Ghafari St.", IsActive = true, ParentId = 61 });
            context.LookUpValues.Add(new LookUpValues { Id = 142, LookUpTypeId = 4, LookUpValue = "Mousaed Al-Aazmi St.", Description = "Mousaed Al-Aazmi St.", IsActive = true, ParentId = 61 });
            context.LookUpValues.Add(new LookUpValues { Id = 143, LookUpTypeId = 4, LookUpValue = "Aldimnah St.", Description = "Aldimnah St.", IsActive = true, ParentId = 61 });
            context.LookUpValues.Add(new LookUpValues { Id = 144, LookUpTypeId = 4, LookUpValue = "Amman St.", Description = "Amman St.", IsActive = true, ParentId = 61 });

            context.LookUpValues.Add(new LookUpValues { Id = 145, LookUpTypeId = 4, LookUpValue = "West Riffa", Description = "West Riffa", IsActive = true, ParentId = 62 });
            context.LookUpValues.Add(new LookUpValues { Id = 146, LookUpTypeId = 4, LookUpValue = "East Riffa", Description = "East Riffa", IsActive = true, ParentId = 62 });
            context.LookUpValues.Add(new LookUpValues { Id = 147, LookUpTypeId = 4, LookUpValue = "Riffa Fort", Description = "Riffa Fort", IsActive = true, ParentId = 62 });
            context.LookUpValues.Add(new LookUpValues { Id = 148, LookUpTypeId = 4, LookUpValue = "Royal Golf Club", Description = "Royal Golf Club", IsActive = true, ParentId = 62 });

            context.LookUpValues.Add(new LookUpValues { Id = 149, LookUpTypeId = 4, LookUpValue = "Mishref", Description = "Mishref", IsActive = true, ParentId = 63 });
            context.LookUpValues.Add(new LookUpValues { Id = 150, LookUpTypeId = 4, LookUpValue = "Salwa", Description = "Salwa", IsActive = true, ParentId = 63 });
            context.LookUpValues.Add(new LookUpValues { Id = 151, LookUpTypeId = 4, LookUpValue = "Rumaithiya", Description = "Rumaithiya", IsActive = true, ParentId = 63 });
            context.LookUpValues.Add(new LookUpValues { Id = 152, LookUpTypeId = 4, LookUpValue = "Bayan Palace", Description = "Bayan Palace", IsActive = true, ParentId = 63 });

            context.LookUpValues.Add(new LookUpValues { Id = 153, LookUpTypeId = 4, LookUpValue = "Al Mawash Mosque", Description = "West Riffa", IsActive = true, ParentId = 64 });
            context.LookUpValues.Add(new LookUpValues { Id = 154, LookUpTypeId = 4, LookUpValue = "Al Qatan", Description = "Al Qatan", IsActive = true, ParentId = 64 });
            context.LookUpValues.Add(new LookUpValues { Id = 155, LookUpTypeId = 4, LookUpValue = "Dar El CID", Description = "Dar El CID", IsActive = true, ParentId = 64 });
            context.LookUpValues.Add(new LookUpValues { Id = 156, LookUpTypeId = 4, LookUpValue = "Tariq Rajab Museum", Description = "Tariq Rajab Museum", IsActive = true, ParentId = 64 });

            context.LookUpValues.Add(new LookUpValues { Id = 157, LookUpTypeId = 4, LookUpValue = "Juffair", Description = "Juffair", IsActive = true, ParentId = 65 });
            context.LookUpValues.Add(new LookUpValues { Id = 158, LookUpTypeId = 4, LookUpValue = "Hoora", Description = "Hoora", IsActive = true, ParentId = 65 });
            context.LookUpValues.Add(new LookUpValues { Id = 159, LookUpTypeId = 4, LookUpValue = "Gufool", Description = "Gufool", IsActive = true, ParentId = 65 });
            context.LookUpValues.Add(new LookUpValues { Id = 160, LookUpTypeId = 4, LookUpValue = "Sulmaniya", Description = "Sulmaniya", IsActive = true, ParentId = 65 });

            context.LookUpValues.Add(new LookUpValues { Id = 161, LookUpTypeId = 4, LookUpValue = "Rumaithiya", Description = "Rumaithiya", IsActive = true, ParentId = 66 });
            context.LookUpValues.Add(new LookUpValues { Id = 162, LookUpTypeId = 4, LookUpValue = "Shaab", Description = "Shaab", IsActive = true, ParentId = 66 });
            context.LookUpValues.Add(new LookUpValues { Id = 163, LookUpTypeId = 4, LookUpValue = "Al Asimah", Description = "Al Asimah", IsActive = true, ParentId = 66 });
            context.LookUpValues.Add(new LookUpValues { Id = 164, LookUpTypeId = 4, LookUpValue = "Qadsia SC", Description = "Qadsia SC", IsActive = true, ParentId = 66 });

            context.LookUpValues.Add(new LookUpValues { Id = 165, LookUpTypeId = 4, LookUpValue = "Maameer", Description = "Maameer", IsActive = true, ParentId = 67 });
            context.LookUpValues.Add(new LookUpValues { Id = 166, LookUpTypeId = 4, LookUpValue = "Eker", Description = "Eker", IsActive = true, ParentId = 67 });
            context.LookUpValues.Add(new LookUpValues { Id = 167, LookUpTypeId = 4, LookUpValue = "Nuwaidrat", Description = "Nuwaidrat", IsActive = true, ParentId = 67 });
            context.LookUpValues.Add(new LookUpValues { Id = 168, LookUpTypeId = 4, LookUpValue = "Wadyan", Description = "Wadyan", IsActive = true, ParentId = 67 });

            context.LookUpValues.Add(new LookUpValues { Id = 169, LookUpTypeId = 4, LookUpValue = "Awadhiya", Description = "Awadhiya", IsActive = true, ParentId = 68 });
            context.LookUpValues.Add(new LookUpValues { Id = 170, LookUpTypeId = 4, LookUpValue = "Bilad Al Qadeem", Description = "Bilad Al Qadeem", IsActive = true, ParentId = 68 });
            context.LookUpValues.Add(new LookUpValues { Id = 171, LookUpTypeId = 4, LookUpValue = "Bu Ghazal", Description = "Bu Ghazal", IsActive = true, ParentId = 68 });
            context.LookUpValues.Add(new LookUpValues { Id = 172, LookUpTypeId = 4, LookUpValue = "Mahooz", Description = "Mahooz", IsActive = true, ParentId = 68 });

            context.LookUpValues.Add(new LookUpValues { Id = 32, LookUpTypeId = 5, LookUpValue = "Emirates Id", Description = "Emirates Id", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 33, LookUpTypeId = 5, LookUpValue = "Driving Licence", Description = "Driving Licence", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 34, LookUpTypeId = 5, LookUpValue = "Others", Description = "Others", IsActive = true });

            context.LookUpValues.Add(new LookUpValues { Id = 35, LookUpTypeId = 6, LookUpValue = "Emirates", Description = "Emirates", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 36, LookUpTypeId = 6, LookUpValue = "Indian", Description = "Indian", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 37, LookUpTypeId = 6, LookUpValue = "American", Description = "American", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 38, LookUpTypeId = 6, LookUpValue = "Other", Description = "Other", IsActive = true });
            //smitha added
            context.LookUpValues.Add(new LookUpValues { Id = 47, LookUpTypeId = 6, LookUpValue = "Afghan", Description = "Afghan", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 48, LookUpTypeId = 6, LookUpValue = "Armenian", Description = "Armenian", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 49, LookUpTypeId = 6, LookUpValue = "Australian", Description = "Australian", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 50, LookUpTypeId = 6, LookUpValue = "Bahraini", Description = "Bahraini", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 51, LookUpTypeId = 6, LookUpValue = "Bangladeshi", Description = "Bangladeshi", IsActive = true });
            context.LookUpValues.Add(new LookUpValues { Id = 52, LookUpTypeId = 6, LookUpValue = "Tunisia", Description = "Tunisia", IsActive = true });
            
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
            context.Roles.Add(new ApplicationRole { Name = "BuildingAdmin", Description = "BuildingAdmin", IsActive = true });
            context.Roles.Add(new ApplicationRole { Name = "Supervisor", Description = "Supervisor", IsActive = true });
            context.Roles.Add(new ApplicationRole { Name = "Security", Description = "Security", IsActive = true });
        }

        private void GenerateSystemAdmin(VMSContext context)
        {
            var systemAdminrole = context.Roles.Add(new ApplicationRole { Name = "SuperAdmin", Description = "SuperAdmin", IsActive = true });


            var newSystemAdminUser = new ApplicationUser
            {
                FullName = "Super Admin",
                Email = "superadmin@ceenexglobal.com",
                PhoneNumber = "1234567890",
                UserName = "superadmin@ceenexglobal.com",
                GenderId = 1,
                Nationality = 3,
                IsActive = true,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                SecurityStamp = System.Guid.NewGuid().ToString(),
                ThemeName = "theme1",
                ProfilePicturePath = null,
                IsImageAvailable = false
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
