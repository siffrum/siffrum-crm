using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.DomainModels.v1.License;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Siffrum.Web.Payroll.DAL.Seeds
{
    public class DatabaseSeeder<T> where T : EfCoreContextRoot
    {
        public void SetupDatabaseWithSeedData(ModelBuilder modelBuilder)
        {
            var defaultCreatedBy = "SeedAdmin";

            SeedDummyData(modelBuilder, defaultCreatedBy);
            SeedDummyCompanyData(modelBuilder, defaultCreatedBy);
        }
        public async Task<bool> SetupDatabaseWithTestData(T context, Func<string, string> encryptorFunc)
        {
            var defaultCreatedBy = "SeedAdmin";
            var defaultUpdatedBy = "UpdateAdmin";
            var apiDb = context as ApiDbContext;
            
            if (apiDb == null)
            {
                return false;
            }
            
            // Check if database already has users
            if (apiDb.ApplicationUsers.Any())
            {
                return false;
            }
            
            try
            {
                SeedDummyClientTheme(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyUserSetting(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientCompanyAttendenceShifts(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientCompanyDepartments(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummySuperAdminUsers(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummySystemAdminUsers(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientAdminUsers(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeUsers(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClienUsersAddress(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeBankDetails(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientGenericPayrollComponents(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyDocuments(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeAdditionalReimbursementLogs(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeCtcDetails(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeePayrollComponents(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeLeaves(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeDocuments(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientCompanyAddresses(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyPayrollTransactions(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyCompanyAccountsTransactions(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyClientEmployeeAttendance(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedDummyCompanyModules(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedLicenseTypes(apiDb, defaultCreatedBy);
                SeedDummyPermission(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                SeedLicenseTypes_Permissions(apiDb, defaultCreatedBy);
                SeedUserLicenseDetails(apiDb, defaultCreatedBy);
                SeedDummyClientCompanyHolidays(apiDb, defaultCreatedBy, defaultUpdatedBy, encryptorFunc);
                
                // Ensure all changes are saved
                await apiDb.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Re-throw to be handled by the controller
                throw;
            }
        }

        #region UserLicenseDetail

        private void SeedUserLicenseDetails(ApiDbContext apiDb, string defaultCreatedBy)
        {
            var trialUserLicenseDetails = new CompanyLicenseDetailsDM()
            {
                SubscriptionPlanName = "Trial",
                LicenseTypeId = 1,
                ClientCompanyDetailId = 1,
                DiscountInPercentage = 5,
                ActualPaidPrice = 500,
                ValidityInDays = 7,
                StartDateUTC = DateTime.UtcNow,
                ExpiryDateUTC = DateTime.UtcNow.AddDays(7),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                Currency = "inr",
                StripeSubscriptionId = "sub_coinTrial",
                StripeCustomerId = "cus_Oa8LzQu5QxGKR5",
                Status = "active",

            };
            apiDb.CompanyLicenseDetails.Add(trialUserLicenseDetails);
            apiDb.SaveChanges();
        }

        #endregion UserLicenseDetail

        #region LicenseTypes
        private void SeedLicenseTypes(ApiDbContext apiDb, string defaultCreatedBy)
        {
            var license1 = new LicenseTypeDM()
            {
                Title = "Trial",
                Description = "Dummy description of Trail license type",
                ValidityInDays = (1 * 7), //feature validatity into 1 month
                Amount = 0,
                LicenseTypeCode = "1BC2023RENO0110",
                IsPreDefined = true,
                ValidFor = RoleTypeDM.Unknown,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                StripePriceId = "0000",
            };
            var license2 = new LicenseTypeDM()
            {
                Title = "Basic",
                Description = "Dummy description of Basic license type",
                ValidityInDays = (1 * 30),
                Amount = 200,
                LicenseTypeCode = "2SD2023RENO0110",
                IsPreDefined = true,
                ValidFor = RoleTypeDM.Unknown,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                //StripePriceId = "price_1O3GHpSDDV20pV8rcjKJIule",
                StripePriceId = "price_1RfZT3SDaWILKYOeBs1uDnGv",

            };
            var license3 = new LicenseTypeDM()
            {
                Title = "Standard",
                Description = "Dummy description of Standard license type",
                ValidityInDays = (1 * 30),
                Amount = 200,
                LicenseTypeCode = "2SD2023RENO0110",
                IsPreDefined = true,
                ValidFor = RoleTypeDM.Unknown,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                //StripePriceId = "price_1O3GIZSDDV20pV8rYAeisbRN",
                StripePriceId = "price_1RfbPHSDaWILKYOeyE6KHxWM",
            };
            var license4 = new LicenseTypeDM()
            {
                Title = "Premium",
                Description = "Dummy description of Premium license type",
                ValidityInDays = (1 * 30),
                Amount = 399,
                LicenseTypeCode = "3PM2023RENO1010",
                IsPreDefined = true,
                ValidFor = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                //StripePriceId = "price_1O3GIrSDDV20pV8rY7oZ83J3",
                StripePriceId = "price_1RfbS1SDaWILKYOeEln14PZW",
            };

            apiDb.LicenseTypes.Add(license1);
            apiDb.LicenseTypes.Add(license2);
            apiDb.LicenseTypes.Add(license3);
            apiDb.LicenseTypes.Add(license4);
            apiDb.SaveChanges();
        }

        private void SeedLicenseTypes_Permissions(ApiDbContext apiDb, string defaultCreatedBy)
        {
            var licenseTypeDM_Permissions = new List<LicenseTypeDM_PermissionDM>()
                {
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 89,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 90,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 91,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 92,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 93,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 94,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 95,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 96,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 97,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 98,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 99,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 100,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 101,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 102,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 103,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 104,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 105,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 106,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 107,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 108,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 109,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 110,
                        LicenseTypeId = 1,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 111,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 112,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 113,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =114,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =115,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 116,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =117,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 118,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 119,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 120,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 121,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 122,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 123,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 124,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 125,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 126,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 127,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 128,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 129,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 130,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 131,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 132,
                        LicenseTypeId = 2,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 133,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =134,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =135,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 136,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =137,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 138,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 139,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 140,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 141,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 142,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 143,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 144,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 145,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 146,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 147,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 148,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 149,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 150,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 151,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 152,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 153,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =154,
                        LicenseTypeId = 3,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =155,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 156,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId =157,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 158,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 159,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 160,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 161,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 162,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 163,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 164,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 165,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 166,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 167,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 168,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 169,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 170,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 171,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 172,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 173,
                        LicenseTypeId = 4
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 174,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 175,
                        LicenseTypeId = 4,
                    },
                    new LicenseTypeDM_PermissionDM()
                    {
                        PermissionId = 176,
                        LicenseTypeId = 4,
                    },


        };

            apiDb.LicenseTypes_Permissions.AddRange(licenseTypeDM_Permissions);
            apiDb.SaveChanges();
        }

        #endregion LicenseTypes

        #region Company Holidays

        private void SeedDummyClientCompanyHolidays(ApiDbContext apidb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var clientCompanyHolidays1 = new ClientCompanyHolidaysDM()
            {
                Name = "Eid-ul-Fitr",
                Description = "Muslim Festival",
                DateTime = DateTime.Now,
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
            };
            var clientCompanyHolidays2 = new ClientCompanyHolidaysDM()
            {
                Name = "Eid-ul-Zuha",
                Description = "Muslim Festival",
                DateTime = DateTime.Now,
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
            };
            apidb.ClientCompanyHolidays.Add(clientCompanyHolidays1);
            apidb.ClientCompanyHolidays.Add(clientCompanyHolidays2);
            apidb.SaveChanges();
        }

        #endregion Company Holidays

        #region Theme

        private async void SeedDummyClientTheme(ApiDbContext apidb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var clientTheme1 = new ClientThemeDM()
            {
                Name = "Default Theme",
                Css = "{--body-background-color: #FDFCFC;--table-card-body-background-color:#FCFCFC;--table-card-heading-background-color: #E4ECF1;--login--card-background--color: #eeeded;--box-shadow:#d3cdcd;--pagging-background-color: #F5F8FA;--top-side-nav-background-color: #F5F8FA;--breadcrumb-heading-background-color: #FDFCFC;--breadcrmb-activeLnk-background:#627b93;--button-background-color: #455868 ;--toggle-button-background-color: #627b93;--active-link-background: #627b93;--hover-background-color: #91adc5;--login--select-background: #eeeded;--modal-content-background:#FCFCFC;--close-modal-button: transparent url(\"data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23000'%3e%3cpath d='M.293.293a1 1 0 011.414 0L8 6.586 14.293.293a1 1 0 111.414 1.414L9.414 8l6.293 6.293a1 1 0 01-1.414 1.414L8 9.414l-6.293 6.293a1 1 0 01-1.414-1.414L6.586 8 .293 1.707a1 1 0 010-1.414z'/%3e%3c/svg%3e\") center/1em auto no-repeat;--text-color: #2B2B2B;--side-nav-icon-color:#2B2B2B;--common-icon-color:#455868;--label-font-color: #2B2B2B;--login-icon-color: #283859;--button-text-color: #FFFFFF;--toggle-button-color: #FFFFFF;--active-Link-color:#ffffff;--line-of-separation:#ccc;--border-color: #ebedee;--login-border-bottom:#455868;--header-border-color: #ebedee;--font-family: Arial, sans-serif;--card-header-font-size: 17px;--table-header-font-size: 13px;--breadcrumb-font-size: 14px;--card-body-font-size:14px;--table-data-font-size: 14px;--input-font-size: 14px;--icon-font-size: 20px;--side-nav-icon-size:22px;--label-font-size: 14px;--span-font-size: 14px;--side-nav-font-size: 14px;--button-font-size: 14px;--nav-label-font-size: 14px;--top-nav-font-size: 12px;--side-nav-subItems-font-size: 12px;--ligin-icon-font-size: 22px;--toggle-font-size: 22px;--login--input--font--size: 22px;--form-button-font-size: 17px;--breadcrumb-font-weight: 400;--card-body-font-weight: 400;--table-data-font-weight: 400;--label-font-weight: 400;--side-nav-subItems-font-weight: 400;--nav-label-font-weight: 600;--card-header-font-weight: 500;--table-header-font-weight: 600;--top-nav-font-weight: 600;--side-nav-font-weight: 500;--span-font-weight: 500;--form-button-font-weight: 600;--button-font-weight: 600;--login--input--font--weight: 600;--active-font-weight:600;}",
                IsSelected = true,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
            };

            var clientTheme2 = new ClientThemeDM()
            {
                Name = "Dark Theme",
                Css = "{--body-background-color: #252121;--table-card-body-background-color: rgb(57, 56, 56);--table-card-heading-background-color: #4D4A4A;--login--card-background--color: #252525;--box-shadow:#353232;--pagging-background-color: #4D4A4A;--top-side-nav-background-color: #4D4A4A;--breadcrumb-heading-background-color: #252121;--breadcrmb-activeLnk-background:#ffffff;--button-background-color: #461E25;--toggle-button-background-color: #461E25;--active-link-background: #461E25;--hover-background-color: #5C383E;--login--select-background: #252525;--modal-content-background:#4d4a4a;--close-modal-button:transparent url(\"data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16' fill='%23fff'%3e%3cpath d='M.293.293a1 1 0 011.414 0L8 6.586 14.293.293a1 1 0 111.414 1.414L9.414 8l6.293 6.293a1 1 0 01-1.414 1.414L8 9.414l-6.293 6.293a1 1 0 01-1.414-1.414L6.586 8 .293 1.707a1 1 0 010-1.414z'/%3e%3c/svg%3e\") center/1em auto no-repeat;--text-color: #FFFFFF;--side-nav-icon-color:#FFFFFF;\r\n    --common-icon-color:#FFFFFF;--label-font-color: #FFFFFF;--login-icon-color: #FFFFFF;--button-text-color: #FFFFFF;--toggle-button-color: #FFFFFF;--active-Link-color:#ffffff;--line-of-separation:#ccc;--border-color: #5A5959;--login-border-bottom:#455868;--header-border-color: #767373;--font-family: Arial, sans-serif;--card-header-font-size: 17px;--table-header-font-size: 16px;--breadcrumb-font-size: 14px;--card-body-font-size:14px;--table-data-font-size: 14px;--input-font-size: 14px;--icon-font-size: 20px;--side-nav-icon-size:25px;--label-font-size: 14px;--span-font-size: 14px;--side-nav-font-size: 14px;--button-font-size: 14px;--nav-label-font-size: 14px;--top-nav-font-size: 12px;--side-nav-subItems-font-size: 12px;--ligin-icon-font-size: 22px;--toggle-font-size: 22px;--login--input--font--size: 22px;--form-button-font-size: 17px;--breadcrumb-font-weight: 400;--card-body-font-weight: 400;--table-data-font-weight: 400;--label-font-weight: 400;--side-nav-subItems-font-weight: 400; --nav-label-font-weight: 500;--card-header-font-weight: 500;--table-header-font-weight: 500;--top-nav-font-weight: 500;--side-nav-font-weight: 500;--span-font-weight: 500;--form-button-font-weight: 600;--button-font-weight: 600;--login--input--font--weight: 600; --active-font-weight:600;}",
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
            };
            apidb.ClientThemes.Add(clientTheme1);
            apidb.ClientThemes.Add(clientTheme2);
            apidb.SaveChanges();
        }

        #endregion Theme

        #region User Setting

        private async void SeedDummyUserSetting(ApiDbContext apidb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var userSetting1 = new UserSettingDM()
            {
                ClientThemeId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };

            var userSetting2 = new UserSettingDM()
            {
                ClientThemeId = 2,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apidb.UserSettings.Add(userSetting1);
            apidb.UserSettings.Add(userSetting2);
            apidb.SaveChanges();
        }

        #endregion User Setting

        #region Data To Entities

        #region Dummy Data

        private void SeedDummyData(ModelBuilder modelBuilder, string defaultCreatedBy)
        {
            modelBuilder.Entity<DummyTeacherDM>().HasData(
                new DummyTeacherDM() { Id = 1, FirstName = "Teacher A", LastName = "Khan", CreatedBy = defaultCreatedBy },
                new DummyTeacherDM() { Id = 2, FirstName = "Teacher B", LastName = "Kumar", CreatedBy = defaultCreatedBy },
                new DummyTeacherDM() { Id = 3, FirstName = "Teacher C", LastName = "Johar", CreatedBy = defaultCreatedBy }
                );

            modelBuilder.Entity<DummySubjectDM>().HasData(
                new DummySubjectDM() { Id = 1, SubjectName = "Physics", SubjectCode = "phy", DummyTeacherID = 1, CreatedBy = defaultCreatedBy },
                new DummySubjectDM() { Id = 2, SubjectName = "Chemistry", SubjectCode = "chem", DummyTeacherID = 2, CreatedBy = defaultCreatedBy },
                new DummySubjectDM() { Id = 3, SubjectName = "Biology", SubjectCode = "bio", DummyTeacherID = 1, CreatedBy = defaultCreatedBy }
            );
            modelBuilder.Entity<ClientGenericPayrollComponentDM>().ToTable("ClientGenericPayrollComponents");
            modelBuilder.Entity<ClientEmployeePayrollComponentDM>().ToTable("ClientEmployeePayrollComponents ");
        }

        #endregion Dummy Data

        #region Companies
        private void SeedDummyCompanyData(ModelBuilder modelBuilder, string defaultCreatedBy)
        {
            var renoComp = new ClientCompanyDetailDM()
            {
                Id = 1,
                Name = "Reno-Softwares",
                CompanyCode = "123",
                Description = "Software Development Company",
                CompanyContactEmail = "Reno123@gmail.com",
                CompanyContactNumber = "9876542341",
                CompanyWebsite = "www.reno.com",
                CompanyLogoPath = "wwwroot/Content/companies/logos/dummyCompanyLogo.jpg",
                CompanyDateOfEstablishment = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                //ClientCompanyAddressId = 2
            };
            var clustComp = new ClientCompanyDetailDM()
            {
                Id = 2,
                Name = "Clust-Tech",
                CompanyCode = "124",
                Description = "Software Development Company",
                CompanyContactEmail = "ctech@gmail.com",
                CompanyContactNumber = "1234567890",
                CompanyWebsite = "www.ctech.com",
                CompanyLogoPath = "wwwroot/Content/companies/logos/dummyCompanyLogo2.jpg",
                CompanyDateOfEstablishment = new DateTime(2009, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                //ClientCompanyAddressId = 1
            };
            modelBuilder.Entity<ClientCompanyDetailDM>().HasData(renoComp, clustComp);
        }

        #endregion Companies

        #region Users

        private void SeedDummySuperAdminUsers(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var superUser1 = new ApplicationUserDM()
            {
                RoleType = RoleTypeDM.SuperAdmin,
                FirstName = "Super",
                MiddleName = "Admin",
                EmailId = "saone@email.com",
                LastName = "One",
                LoginId = "super1",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                ProfilePicturePath = "wwwroot/Content/loginusers/profile/default_profile.jpg",
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var superUser2 = new ApplicationUserDM()
            {
                RoleType = RoleTypeDM.SuperAdmin,
                FirstName = "Super",
                MiddleName = "Admin",
                LastName = "Two",
                EmailId = "satwo@email.com",
                LoginId = "super2",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.ApplicationUsers.Add(superUser1);
            apiDb.SaveChanges();
            apiDb.ApplicationUsers.Add(superUser2);
            apiDb.SaveChanges();
        }
        private void SeedDummySystemAdminUsers(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var sysUser1 = new ApplicationUserDM()
            {
                RoleType = RoleTypeDM.SystemAdmin,
                FirstName = "System",
                MiddleName = "Admin",
                EmailId = "sysone@email.com",
                LastName = "One",
                LoginId = "system1",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var sysUser2 = new ApplicationUserDM()
            {
                RoleType = RoleTypeDM.SystemAdmin,
                FirstName = "System",
                MiddleName = "Admin",
                LastName = "Two",
                EmailId = "systwo@email.com",
                LoginId = "system2",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.ApplicationUsers.Add(sysUser1);
            apiDb.SaveChanges();
            apiDb.ApplicationUsers.Add(sysUser2);
            apiDb.SaveChanges();
        }

        private async void SeedDummyPermission(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            List<PermissionDM> permissions = new List<PermissionDM>()
            {
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 3,

                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 5,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 6,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 7,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 8,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 9,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 10,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 11,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 12,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 13,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 14,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 15,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 16,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 17,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 18,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 19,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 20,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 21,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 22,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 1,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 2,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 3,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 4,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 5,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 6,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 7,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 8,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 9,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 10,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 11,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 12,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 13,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 14,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 15,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 16,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 17,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 18,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 19,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 20,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 21,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 1,
                CompanyModulesId = 22,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 5,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 6,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 7,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 8,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 9,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 10,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 11,

                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 12,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 13,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 14,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 15,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 16,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 17,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 18,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 19,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 20,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 21,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 22,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 1,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 2,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 3,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 4,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 5,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 6,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 7,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 8,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 9,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 10,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 11,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 12,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 13,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 14,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 15,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 16,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 17,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 18,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 19,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 20,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 21,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                ClientCompanyDetailId = 2,
                CompanyModulesId = 22,
                RoleType = RoleTypeDM.ClientEmployee,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 1,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 2,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 3,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 4,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 5,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 6,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 7,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 8,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 9,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 10,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 11,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 12,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 13,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 14,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 15,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 16,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 17,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 18,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 19,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 20,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 21,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 22,
                LicenseTypeId = 1,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                  new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 1,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 2,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 3,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 4,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 5,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 6,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 7,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 8,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 9,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 10,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 11,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 12,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 13,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 14,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 15,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 16,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 17,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 18,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 19,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 20,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 21,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 22,
                LicenseTypeId = 2,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                  new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 1,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 2,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 3,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 4,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 5,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 6,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 7,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 8,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 9,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 10,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 11,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 12,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 13,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 14,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 15,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 16,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 17,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 18,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 19,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 20,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 21,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 22,
                LicenseTypeId = 3,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                  new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 1,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 2,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 3,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 4,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 5,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 6,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 7,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 8,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 9,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 10,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 11,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 12,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 13,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 14,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 15,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 16,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 17,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 18,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 19,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 20,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },

                new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 21,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            },
                 new PermissionDM()
            {
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                IsEnabledForClient = true,
                CompanyModulesId = 22,
                LicenseTypeId = 4,
                RoleType = RoleTypeDM.ClientAdmin,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            }


        };
            //var pagePermission1 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission2 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 2,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission3 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 3,

            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission4 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 4,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission5 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 5,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission6 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 6,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission7 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 7,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission8 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 8,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission9 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 9,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission10 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 10,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission11 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 11,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission12 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 12,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission13 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 13,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission14 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 14,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission15 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 15,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission16 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 16,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission17 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 17,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission18 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 18,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission19 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 19,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission20 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 20,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission81 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 21,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission85 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 22,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission82 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 21,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission21 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 1,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission22 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 2,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission23 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 3,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission24 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 4,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission25 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 5,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission26 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 6,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission27 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 7,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission28 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 8,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission29 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 9,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission30 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 10,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission31 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 11,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission32 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 12,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission33 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 13,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission34 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 14,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission35 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 15,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission36 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 16,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission37 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 17,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission38 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 18,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission39 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 19,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission40 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 20,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission86 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 22,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission87 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 22,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission83 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 21,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission41 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission42 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 2,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission43 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 3,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission44 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 4,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission45 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 5,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission46 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 6,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission47 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 7,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission48 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 8,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission49 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 9,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission50 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 10,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission51 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 11,

            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission52 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 12,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission53 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 13,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission54 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 14,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission55 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 15,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission56 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 16,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission57 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 17,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission58 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 18,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission59 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 19,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission60 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 20,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission84 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 21,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission88 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 22,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission61 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 1,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission62 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 2,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission63 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 3,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission64 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 4,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission65 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 5,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission66 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 6,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission67 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 7,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission68 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 8,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission69 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 9,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission70 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 10,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission71 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 11,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission72 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 12,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission73 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 13,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission74 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 14,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission75 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 15,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission76 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 16,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission77 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 17,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission78 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 18,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission79 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 19,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission80 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 2,
            //    CompanyModulesId = 20,
            //    RoleType = RoleTypeDM.ClientEmployee,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};




            //var pagePermission100 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 1,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};

            //var pagePermission101 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 2,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission102 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 3,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission103 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 4,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission104 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 5,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission105 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 6,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission106 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 7,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission107 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 8,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission108 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 9,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission109 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 10,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission110 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 11,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission111 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 12,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission112 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 13,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission113 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 14,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission114 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 15,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission115 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 16,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission116 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 17,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission117 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 18,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission118 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 19,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};
            //var pagePermission119 = new PermissionDM()
            //{
            //    View = true,
            //    Add = true,
            //    Edit = true,
            //    Delete = true,
            //    IsEnabledForClient = true,
            //    ClientCompanyDetailId = 1,
            //    CompanyModulesId = 20,
            //    LicenseTypeId = 1,
            //    RoleType = RoleTypeDM.ClientAdmin,
            //    CreatedBy = defaultCreatedBy,
            //    CreatedOnUTC = DateTime.UtcNow
            //};



            //apiDb.Permissions.AddRange(pagePermission1, pagePermission2, pagePermission3, pagePermission4, pagePermission5, pagePermission6, pagePermission7, pagePermission8, pagePermission9, pagePermission10, pagePermission11, pagePermission12, pagePermission13, pagePermission14, pagePermission15, pagePermission16, pagePermission17, pagePermission18, pagePermission19, pagePermission20, pagePermission21, pagePermission22, pagePermission23, pagePermission24, pagePermission25, pagePermission26, pagePermission27, pagePermission28, pagePermission29, pagePermission30, pagePermission31, pagePermission32, pagePermission33, pagePermission34, pagePermission35, pagePermission36, pagePermission37, pagePermission38, pagePermission39, pagePermission40, pagePermission41, pagePermission42, pagePermission43, pagePermission44, pagePermission45, pagePermission46, pagePermission47, pagePermission48, pagePermission49, pagePermission50, pagePermission51, pagePermission52, pagePermission53, pagePermission54, pagePermission55, pagePermission56, pagePermission57, pagePermission58, pagePermission59, pagePermission60, pagePermission61, pagePermission62, pagePermission63, pagePermission64, pagePermission65, pagePermission66, pagePermission67, pagePermission68, pagePermission69, pagePermission70, pagePermission71, pagePermission72, pagePermission73, pagePermission74, pagePermission75, pagePermission76, pagePermission77, pagePermission78, pagePermission79, pagePermission80, pagePermission81, pagePermission82, pagePermission83, pagePermission84, pagePermission85, pagePermission86, pagePermission87, pagePermission88, pagePermission100, pagePermission101, pagePermission102, pagePermission103, pagePermission104, pagePermission105, pagePermission106, pagePermission107, pagePermission108, pagePermission109, pagePermission110, pagePermission111, pagePermission112, pagePermission113, pagePermission114, pagePermission115, pagePermission116, pagePermission117, pagePermission118, pagePermission119);
            apiDb.Permissions.AddRange(permissions);
            apiDb.SaveChanges();
        }
        private void SeedDummyClientAdminUsers(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var cAdmin1 = new ClientUserDM()
            {
                ClientCompanyDetailId = 1,
                UserSettingId = 2,
                ClientCompanyAttendanceShiftId = 1,
                ClientCompanyDepartmentId = 2,
                EmployeeCode = "100",
                RoleType = RoleTypeDM.ClientAdmin,
                FirstName = "Client",
                MiddleName = "Admin",
                EmailId = "cadone@email.com",
                LastName = "One",
                LoginId = "clientadmin1",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                EmployeeStatus = EmployeeStatusDM.Active,
                Designation = "Employee",
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                ProfilePicturePath = "wwwroot/Content/loginusers/profile/default_profile.jpg",
                DateOfJoining = DateTime.UtcNow,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var cAdmin2 = new ClientUserDM()
            {
                RoleType = RoleTypeDM.ClientAdmin,
                ClientCompanyDetailId = 2,
                ClientCompanyAttendanceShiftId = 2,
                ClientCompanyDepartmentId = 2,
                EmployeeCode = "101",
                FirstName = "Clinet",
                MiddleName = "Admin",
                LastName = "Two",
                EmailId = "cadtwo@email.com",
                LoginId = "clientadmin2",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                EmployeeStatus = EmployeeStatusDM.Active,
                Designation = "Employee",
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                DateOfJoining = DateTime.UtcNow,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.ClientUsers.Add(cAdmin1);
            apiDb.SaveChanges();
            apiDb.ClientUsers.Add(cAdmin2);
            apiDb.SaveChanges();
        }

        private void SeedDummyClientCompanyAddresses(ApiDbContext apiDb, string defaultCreatedBY, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var cAddress1 = new ClientCompanyAddressDM()
            {
                Country = "USA",
                State = "J&K",
                City = "NewYork",
                Address1 = "Near Alfala Complex",
                Address2 = "Near Shopping Mall",
                PinCode = "991122",
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBY,
                CreatedOnUTC = DateTime.UtcNow
            };

            var cAddress2 = new ClientCompanyAddressDM()
            {
                Country = "Europe",
                State = "J&K",
                City = "Germany",
                Address1 = "Near Hotel Dominos",
                Address2 = "Near Iqra Masjid",
                PinCode = "775533",
                ClientCompanyDetailId = 2,
                CreatedBy = defaultCreatedBY,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.ClientCompanyAddresss.Add(cAddress1);
            apiDb.SaveChanges();
            apiDb.ClientCompanyAddresss.Add(cAddress2);
            apiDb.SaveChanges();
        }

        private async void SeedDummyClientCompanyDepartments(ApiDbContext apidb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var clientCompanyDepartment1 = new ClientCompanyDepartmentDM()
            {
                DepartmentName = "IT",
                DepartmentDescription = "this is used for Development Purposes",
                DepartmenntLocation = "near Cafeteria",
                DepartmentCode = "999",
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };

            var clientCompanyDepartment2 = new ClientCompanyDepartmentDM()
            {
                DepartmentName = "Administration",
                DepartmentDescription = "handles all the Queries of Administration",
                DepartmenntLocation = "near IT Department",
                DepartmentCode = "777",
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apidb.ClientCompanyDepartments.Add(clientCompanyDepartment1);
            apidb.ClientCompanyDepartments.Add(clientCompanyDepartment2);
            apidb.SaveChanges();
        }

        private async void SeedDummyClientCompanyAttendenceShifts(ApiDbContext apidb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var companyAttendanceShift1 = new ClientCompanyAttendanceShiftDM()
            {
                ShiftFrom = DateTime.Parse("01/Apr/2023 10:00:00 AM"),
                ShiftTo = DateTime.Parse("31/Dec/2023 06:00:00 PM"),
                ShiftName = "Day-Shift",
                ShiftDescription = "Working hours from 10 A.M to 6 P.M",
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };

            var companyAttendanceShift2 = new ClientCompanyAttendanceShiftDM()
            {
                ShiftFrom = DateTime.Parse("01/Apr/2023 10:00:00 PM"),
                ShiftTo = DateTime.Parse("31/Dec/2023 05:00:00 AM"),
                ShiftName = "Night-Shift",
                ShiftDescription = "Working hours from 10 P.M to 5 A.M",
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apidb.ClientCompanyAttendanceShifts.Add(companyAttendanceShift1);
            apidb.ClientCompanyAttendanceShifts.Add(companyAttendanceShift2);
            apidb.SaveChanges();
        }
        private async void SeedDummyClientEmployeeAttendance(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var employeeAttendances1 = new ClientEmployeeAttendanceDM()
            {
                CheckIn = "01/4/2023 10:00:00 AM",
                CheckOut = "01/4/2023 05:00:00 PM",
                AttendanceStatus = AttendanceStatusDM.P,
                AttendanceDate = DateTime.Now,
                ClientUserId = 1,
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.ClientEmployeeAttendances.Add(employeeAttendances1);
            apiDb.SaveChanges();
        }

        private async void SeedDummyCompanyModules(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var companyModules1 = new CompanyModulesDM()
            {
                ModuleValue = "CompanyDetail",
                Description = "This Module is used for Company Module",
                ModuleName = ModuleNameDM.CompanyDetail,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules2 = new CompanyModulesDM()
            {
                ModuleValue = "CompanyAddress",
                Description = "This Module is used for Company Address Module",
                ModuleName = ModuleNameDM.CompanyAddress,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules3 = new CompanyModulesDM()
            {
                ModuleValue = "CompanyLetters",
                Description = "This Module is used for Company Address Module",
                ModuleName = ModuleNameDM.CompanyLetters,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules4 = new CompanyModulesDM()
            {
                ModuleValue = "CompanyAccountTransaction",
                Description = "This Module is used for Company Account Module",
                ModuleName = ModuleNameDM.CompanyAccountTransaction,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules5 = new CompanyModulesDM()
            {
                ModuleValue = "Employee",
                Description = "This Module is used for Employee Module",
                ModuleName = ModuleNameDM.Employee,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules6 = new CompanyModulesDM()
            {
                ModuleValue = "EmployeeCTC",
                Description = "This Module is used for Employee CTC Module",
                ModuleName = ModuleNameDM.EmployeeCTC,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules7 = new CompanyModulesDM()
            {
                ModuleValue = "EmployeeDocument",
                Description = "This Module is used for Employee Document Module",
                ModuleName = ModuleNameDM.EmployeeDocument,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules8 = new CompanyModulesDM()
            {
                ModuleValue = "Attendance",
                Description = "This Module is used for Attendance Module",
                ModuleName = ModuleNameDM.Attendance,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules9 = new CompanyModulesDM()
            {
                ModuleValue = "EmployeeDirectory",
                Description = "This Module is used for Employee Directory Module",
                ModuleName = ModuleNameDM.EmployeeDirectory,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules10 = new CompanyModulesDM()
            {
                ModuleValue = "EmployeeGenericPayroll",
                Description = "This Module is used for Employee Generic Payroll Module",
                ModuleName = ModuleNameDM.EmployeeGenericPayroll,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules11 = new CompanyModulesDM()
            {
                ModuleValue = "EmployeeReimbursement",
                Description = "This Module is used for Employee Reimbursement Module",
                ModuleName = ModuleNameDM.EmployeeReimbursement,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules12 = new CompanyModulesDM()
            {
                ModuleValue = "EmployeeAddress",
                Description = "This Module is used for Employee Address Module",
                ModuleName = ModuleNameDM.EmployeeAddress,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules13 = new CompanyModulesDM()
            {
                ModuleValue = "Leave",
                Description = "This Module is used for Employee Leave Module",
                ModuleName = ModuleNameDM.Leave,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules14 = new CompanyModulesDM()
            {
                ModuleValue = "GenerateLetters",
                Description = "This Module is used for Generate Letters Module",
                ModuleName = ModuleNameDM.GenerateLetters,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules15 = new CompanyModulesDM()
            {
                ModuleValue = "PayrollTransacton",
                Description = "This Module is used for Payroll Transaction Module",
                ModuleName = ModuleNameDM.PayrollTransacton,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules16 = new CompanyModulesDM()
            {
                ModuleValue = "Reports",
                Description = "This Module is used for Employee Module",
                ModuleName = ModuleNameDM.Reports,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules17 = new CompanyModulesDM()
            {
                ModuleValue = "Setting",
                Description = "This Module is used for Setting Module",
                ModuleName = ModuleNameDM.Setting,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules18 = new CompanyModulesDM()
            {
                ModuleValue = "Document",
                Description = "This Module is used for Document Module",
                ModuleName = ModuleNameDM.Document,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules19 = new CompanyModulesDM()
            {
                ModuleValue = "BankDetail",
                Description = "This Module is used for Bank Detail Module",
                ModuleName = ModuleNameDM.BankDetail,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules20 = new CompanyModulesDM()
            {
                ModuleValue = "AttendanceShift",
                Description = "This Module is used for Attendance Shift Module",
                ModuleName = ModuleNameDM.AttendanceShift,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules21 = new CompanyModulesDM()
            {
                ModuleValue = "DashBoard",
                Description = "This Module is used for DashBoard Module",
                ModuleName = ModuleNameDM.DashBoard,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var companyModules22 = new CompanyModulesDM()
            {
                ModuleValue = "CompanyDepartment",
                Description = "This Module is used for Company Department Module",
                ModuleName = ModuleNameDM.CompanyDepartment,
                IsEnabled = true,
                StandAlone = false,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.CompanyModules.AddRange(companyModules1, companyModules2, companyModules3, companyModules4, companyModules5, companyModules6, companyModules7, companyModules8, companyModules9, companyModules10, companyModules11, companyModules12, companyModules13, companyModules14, companyModules15, companyModules16, companyModules17, companyModules18, companyModules19, companyModules20, companyModules21, companyModules22);
            apiDb.SaveChanges();
        }

        private async void SeedDummyCompanyAccountsTransactions(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var companyAccountsTransaction = new CompanyAccountsTransactionDM()
            {
                ExpenseName = "Building Rent",
                ExpensePurpose = "have to pay building rent to the Owner ",
                ExpenseDate = DateTime.UtcNow,
                ExpenseAmount = 25000,
                ExpenseMode = 0,
                ExpensePaid = false,
                CurrencyCode = "INR",
                ClientCompanyDetailId = 1,
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.CompanyAccountsTransactions.Add(companyAccountsTransaction);
            apiDb.SaveChanges();
        }
        private async void SeedDummyPayrollTransactions(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var empCtc = apiDb.ClientEmployeeCTCDetails.Where(x => x.CurrentlyActive == true).FirstOrDefault();
            ClientEmployeePayrollComponentDM clientEmployeePayrollComponent = new ClientEmployeePayrollComponentDM();
            float amountMonthly = 0;
            foreach (var item in empCtc.ClientEmployeePayrollComponents)
            {
                amountMonthly = amountMonthly + (item.AmountYearly) / 12;
            }
            var payment1 = new PayrollTransactionDM()
            {
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                PaymentAmount = amountMonthly,
                ToPay = amountMonthly,
                ToPaid = amountMonthly,
                PaymentFor = DateTime.Now.ToString("MMMM-yyyy"),
                PaymentMode = PaymentModeDM.Credit,
                PaymentPaid = false,
                PaymentType = PaymentTypeDM.Salary,
                ClientUserId = 1,
                ClientEmployeeCTCDetailId = empCtc.Id,
                ClientCompanyDetailId = 1
            };
            apiDb.PayrollTransactions.Add(payment1);
            apiDb.SaveChanges();
        }

        private void SeedDummyClientEmployeeDocuments(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var empaddDocument1 = new ClientEmployeeDocumentDM()
            {
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                Name = "AadharCard",
                EmployeeDocumentType = EmployeeDocumentTypeDM.ProofOfIdentification,
                DocumentDescription = "Uploaded AadharCard by ClientAdmin",
                EmployeeDocumentPath = "wwwroot/Content/documents/AadharCard.pdf",
                Extension = "pdf",
                ClientUserId = 1,
                ClientCompanyDetailId = 1
            };

            var empaddDocument2 = new ClientEmployeeDocumentDM()
            {
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                Name = "BankPassbook",
                EmployeeDocumentType = EmployeeDocumentTypeDM.other,
                DocumentDescription = "Uploaded BankPassbbok by ClientAdmin",
                EmployeeDocumentPath = "wwwroot/Content/documents/BankPassbook.pdf",
                Extension = "pdf",
                ClientUserId = 2,
                ClientCompanyDetailId = 2
            };

            var empaddDocument3 = new ClientEmployeeDocumentDM()
            {
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                Name = "InternetBill",
                EmployeeDocumentType = EmployeeDocumentTypeDM.other,
                DocumentDescription = "Uploaded InternetBill by ClientAdmin",
                EmployeeDocumentPath = "wwwroot/Content/documents/internetbill.pdf",
                Extension = "pdf",
                ClientUserId = 1,
                ClientCompanyDetailId = 1
            };

            apiDb.ClientEmployeeDocuments.Add(empaddDocument1);
            apiDb.ClientEmployeeDocuments.Add(empaddDocument2);
            apiDb.ClientEmployeeDocuments.Add(empaddDocument3);
            apiDb.SaveChanges();

        }


        private void SeedDummyClientEmployeeLeaves(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var empLeaves1 = new ClientEmployeeLeaveDM()
            {
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                ApprovedByUserName = "ClientAdmin",
                LeaveDateFromUTC = DateTime.UtcNow,
                LeaveDateToUTC = DateTime.UtcNow,
                LeaveType = LeaveTypeDM.Casual_Leave,
                ClientUserId = 1,
                ClientCompanyDetailId = 1,
                EmployeeComment = "Employee casually decides to be out of office"
            };

            var empLeaves2 = new ClientEmployeeLeaveDM()
            {
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow,
                ApprovedByUserName = "ClientAdmin",
                LeaveDateFromUTC = DateTime.UtcNow,
                LeaveDateToUTC = DateTime.UtcNow,
                LeaveType = LeaveTypeDM.Marriage_Leave,
                ClientUserId = 2,
                ClientCompanyDetailId = 2,
                EmployeeComment = "employee decides to be out of office due to Marriage Ceremony"
            };

            apiDb.ClientEmployeeLeaves.Add(empLeaves1);
            apiDb.ClientEmployeeLeaves.Add(empLeaves2);
            apiDb.SaveChanges();

        }

        private void SeedDummyClientEmployeePayrollComponents(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {

            var employeePayrollBasic = new ClientEmployeePayrollComponentDM()
            {
                Name = "Basic",
                Description = "Basic Amount",
                Percentage = 10f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 12000f,

            };
            var employeePayrollHra = new ClientEmployeePayrollComponentDM()
            {
                Name = "HRA",
                Description = "House Rent Amount",
                Percentage = 30f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 36000f,
            };
            var employeePayrollSa = new ClientEmployeePayrollComponentDM()
            {
                Name = "SA",
                Description = "Special Allowance",
                Percentage = 10f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Yearly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 12000f,
            };
            var employeePayrollDa = new ClientEmployeePayrollComponentDM()
            {
                Name = "DA",
                Description = "Dearness Allowance",
                Percentage = 15f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 18000f,
            };
            var employeePayrollEpf = new ClientEmployeePayrollComponentDM()
            {
                Name = "EPF",
                Description = "Employee Provident Fund",
                Percentage = 12f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 14400f,
            };
            var employeePayrollBonus = new ClientEmployeePayrollComponentDM()
            {
                Name = "BONUS",
                Description = "Employee Bonus",
                Percentage = 8f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Yearly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 9600f,
            };
            var employeePayrollCa = new ClientEmployeePayrollComponentDM()
            {
                Name = "CA",
                Description = "Conveyance Allowance",
                Percentage = 15f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientEmployeeCTCDetailId = 1,
                AmountYearly = 18000f,
            };
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollBasic);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollHra);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollSa);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollCa);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollBonus);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollEpf);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollDa);
            apiDb.SaveChanges();

            employeePayrollBasic.Id = 0;
            employeePayrollBasic.ClientEmployeeCTCDetailId = 2;
            employeePayrollBasic.AmountYearly = 21000f;
            employeePayrollHra.Id = 0;
            employeePayrollHra.ClientEmployeeCTCDetailId = 2;
            employeePayrollHra.AmountYearly = 63000f;
            employeePayrollSa.Id = 0;
            employeePayrollSa.ClientEmployeeCTCDetailId = 2;
            employeePayrollSa.AmountYearly = 21000f;
            employeePayrollDa.Id = 0;
            employeePayrollDa.ClientEmployeeCTCDetailId = 2;
            employeePayrollDa.AmountYearly = 31500f;
            employeePayrollEpf.Id = 0;
            employeePayrollEpf.ClientEmployeeCTCDetailId = 2;
            employeePayrollEpf.AmountYearly = 25200f;
            employeePayrollBonus.Id = 0;
            employeePayrollBonus.ClientEmployeeCTCDetailId = 2;
            employeePayrollBonus.AmountYearly = 16800f;
            employeePayrollCa.Id = 0;
            employeePayrollCa.ClientEmployeeCTCDetailId = 2;
            employeePayrollCa.AmountYearly = 31500f;
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollBasic);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollHra);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollSa);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollCa);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollBonus);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollEpf);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollDa);
            apiDb.SaveChanges();

            employeePayrollBasic.Id = 0;
            employeePayrollBasic.ClientEmployeeCTCDetailId = 3;
            employeePayrollBasic.AmountYearly = 53000f;
            employeePayrollHra.Id = 0;
            employeePayrollHra.ClientEmployeeCTCDetailId = 3;
            employeePayrollHra.AmountYearly = 159000f;
            employeePayrollSa.Id = 0;
            employeePayrollSa.ClientEmployeeCTCDetailId = 3;
            employeePayrollSa.AmountYearly = 53000f;
            employeePayrollDa.Id = 0;
            employeePayrollDa.ClientEmployeeCTCDetailId = 3;
            employeePayrollDa.AmountYearly = 79500f;
            employeePayrollEpf.Id = 0;
            employeePayrollEpf.ClientEmployeeCTCDetailId = 3;
            employeePayrollEpf.AmountYearly = 63600f;
            employeePayrollBonus.Id = 0;
            employeePayrollBonus.ClientEmployeeCTCDetailId = 3;
            employeePayrollBonus.AmountYearly = 42400f;
            employeePayrollCa.Id = 0;
            employeePayrollCa.ClientEmployeeCTCDetailId = 3;
            employeePayrollCa.AmountYearly = 79500f;
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollBasic);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollHra);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollSa);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollCa);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollBonus);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollEpf);
            apiDb.ClientEmployeePayrollComponents.Add(employeePayrollDa);
            apiDb.SaveChanges();

        }

        private void SeedDummyClientEmployeeCtcDetails(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var EmployeeCtcDetail1 = new ClientEmployeeCTCDetailDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                CTCAmount = 120000f,
                CurrencyCode = "INR",
                StartDateUtc = DateTime.UtcNow,
                EndDateUtc = DateTime.UtcNow,
                CurrentlyActive = false,
                ClientUserId = 1
            };

            var EmployeeCtcDetail2 = new ClientEmployeeCTCDetailDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                CTCAmount = 210000.00f,
                CurrencyCode = "INR",
                StartDateUtc = DateTime.UtcNow,
                EndDateUtc = DateTime.UtcNow,
                CurrentlyActive = false,
                ClientUserId = 1
            };

            var EmployeeCtcDetail3 = new ClientEmployeeCTCDetailDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                CTCAmount = 530000.00f,
                CurrencyCode = "INR",
                StartDateUtc = DateTime.UtcNow,
                EndDateUtc = DateTime.UtcNow,
                CurrentlyActive = true,
                ClientUserId = 1
            };

            apiDb.ClientEmployeeCTCDetails.Add(EmployeeCtcDetail1);
            apiDb.ClientEmployeeCTCDetails.Add(EmployeeCtcDetail2);
            apiDb.ClientEmployeeCTCDetails.Add(EmployeeCtcDetail3);
            apiDb.SaveChanges();
        }
        private void SeedDummyClientEmployeeUsers(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var cEmp1 = new ClientUserDM()
            {
                ClientCompanyDetailId = 1,
                ClientCompanyAttendanceShiftId = 1,
                ClientCompanyDepartmentId = 1,
                EmployeeCode = "102",
                RoleType = RoleTypeDM.ClientEmployee,
                FirstName = "Client",
                MiddleName = "Employee",
                EmailId = "cempone@email.com",
                LastName = "One",
                LoginId = "clientemp1",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                EmployeeStatus = EmployeeStatusDM.Active,
                DateOfJoining = DateTime.UtcNow,
                Designation = "Employee",
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var cEmp2 = new ClientUserDM()
            {
                RoleType = RoleTypeDM.ClientEmployee,
                ClientCompanyDetailId = 2,
                ClientCompanyAttendanceShiftId = 2,
                ClientCompanyDepartmentId = 1,
                UserSettingId = 2,
                EmployeeCode = "103",
                FirstName = "Clinet",
                MiddleName = "Employee",
                LastName = "Two",
                EmailId = "cemptwo@email.com",
                LoginId = "clientemp2",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                EmployeeStatus = EmployeeStatusDM.Active,
                DateOfJoining = DateTime.UtcNow,
                Designation = "Employee",
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            var cEmp3 = new ClientUserDM()
            {
                RoleType = RoleTypeDM.ClientEmployee,
                ClientCompanyDetailId = 1,
                ClientCompanyAttendanceShiftId = 1,
                ClientCompanyDepartmentId = 1,
                EmployeeCode = "104",
                FirstName = "Client",
                MiddleName = "Employee",
                LastName = "Three",
                EmailId = "cempthree@email.com",
                LoginId = "clientemp3",
                IsEmailConfirmed = true,
                LoginStatus = LoginStatusDM.Enabled,
                EmployeeStatus = EmployeeStatusDM.Active,
                DateOfJoining = DateTime.UtcNow,
                Designation = "Employee",
                IsPhoneNumberConfirmed = true,
                PasswordHash = encryptorFunc("pass123"),
                ProfilePicturePath = "wwwroot/Content/loginusers/profile/pic1.png",
                PhoneNumber = "9966778811",
                PersonalEmailId = "employee@gmail.com",
                CreatedBy = defaultCreatedBy,
                CreatedOnUTC = DateTime.UtcNow
            };
            apiDb.ClientUsers.Add(cEmp1);
            apiDb.ClientUsers.Add(cEmp2);
            apiDb.ClientUsers.Add(cEmp3);
            apiDb.SaveChanges();
        }

        #endregion Users

        #region Application Specific Tables

        private void SeedDummyClientEmployeeAdditionalReimbursementLogs(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var path1 = (Path.GetFullPath("wwwroot/Content/documents/InternetBill.pdf"));
            var path2 = (Path.GetFullPath("wwwroot/Content/documents/TravellingAllowanceBill.pdf"));
            var empaddReimburs1 = new ClientEmployeeAdditionalReimbursementLogDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ReimbursementType = ReimbursementTypeDM.Telephone,
                ReimburseDocumentName = "InternetBill",
                Extension = "Pdf",
                ReimbursementAmount = 600,
                ReimbursementDate = DateTime.UtcNow,
                ReimbursementDocumentPath = "wwwroot/Content/documents/InternetBill.pdf",
                ReimbursementDescription = "Uploaded InternetBill by ClientEmployee",
                ClientUserId = 1,
                ClientCompanyDetailId = 1
            };

            var empaddReimburs2 = new ClientEmployeeAdditionalReimbursementLogDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ReimbursementType = ReimbursementTypeDM.Travel,
                ReimburseDocumentName = "TravellingAllowanceBill",
                Extension = "Pdf",
                ReimbursementAmount = 600,
                ReimbursementDate = DateTime.UtcNow,
                ReimbursementDocumentPath = "wwwroot/Content/documents/TravellingAllowanceBill.pdf",
                ReimbursementDescription = "Uploaded TravellingAllowanceBill by ClientEmployee",
                ClientUserId = 2,
                ClientCompanyDetailId = 2
            };

            apiDb.ClientEmployeeAdditionalReimbursementLogs.Add(empaddReimburs1);
            apiDb.ClientEmployeeAdditionalReimbursementLogs.Add(empaddReimburs2);
            apiDb.SaveChanges();

        }

        private void SeedDummyClienUsersAddress(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var userAddress1 = new ClientUserAddressDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                Address1 = "NewYork",
                Address2 = "California",
                City = "NewYork",
                State = "J&K",
                PinCode = "190001",
                ClientUserAddressType = ClientUserAddressTypeDM.PermanentAddress,
                ClientUserId = 1,
                ClientCompanyDetailId = 1
            };

            var userAddress2 = new ClientUserAddressDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                Address1 = "Sydney",
                Address2 = "New Cafeteria",
                City = "Melbourne",
                State = "Australia",
                PinCode = "190001",
                ClientUserAddressType = ClientUserAddressTypeDM.MailingAddress,
                ClientUserId = 2,
                ClientCompanyDetailId = 2
            };

            apiDb.ClientUserAddresss.Add(userAddress1);
            apiDb.ClientUserAddresss.Add(userAddress2);
            apiDb.SaveChanges();

        }

        private void SeedDummyDocuments(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var path1 = (Path.GetFullPath("wwwroot/Content/documents/offerletter.docx"));
            var path2 = (Path.GetFullPath("wwwroot/Content/documents/appointmentletter.docx"));
            var documents1 = new DocumentsDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                Name = "OfferLetter",
                Description = "Offer Letter of XYZ Company",
                LetterData = System.IO.File.ReadAllBytes(path1),
                Extension = "doc",
                ClientCompanyDetailId = 1
            };

            var documents2 = new DocumentsDM()
            {
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                Name = "AppointmentLetter",
                Description = "Appointment Letter of PQR Company",
                LetterData = System.IO.File.ReadAllBytes(path2),
                Extension = "doc",
                ClientCompanyDetailId = 1
            };

            apiDb.Documents.Add(documents1);
            apiDb.Documents.Add(documents2);
            apiDb.SaveChanges();

        }

        private void SeedDummyClientGenericPayrollComponents(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var genericPayrollBasic = new ClientGenericPayrollComponentDM()
            {
                Name = "Basic",
                Description = "Basic Amount",
                Percentage = 10f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };
            var genericPayrollHra = new ClientGenericPayrollComponentDM()
            {
                Name = "HRA",
                Description = "House Rent Amount",
                Percentage = 30f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };

            var genericPayrollSa = new ClientGenericPayrollComponentDM()
            {
                Name = "SA",
                Description = "Special Allowance",
                Percentage = 10f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };

            var genericPayrollDa = new ClientGenericPayrollComponentDM()
            {
                Name = "DA",
                Description = "Dearness Allowance",
                Percentage = 15f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };

            var genericPayrollEpf = new ClientGenericPayrollComponentDM()
            {
                Name = "EPF",
                Description = "Employee Provident Fund",
                Percentage = 12f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };

            var genericPayrollBonus = new ClientGenericPayrollComponentDM()
            {
                Name = "BONUS",
                Description = "Employee Bonus",
                Percentage = 8f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };

            var genericPayrollCa = new ClientGenericPayrollComponentDM()
            {
                Name = "CA",
                Description = "Conveyance Allowance",
                Percentage = 15f,
                ComponentCalculationType = ComponentCalculationTypeDM.Addition,
                ComponentPeriodType = ComponentPeriodTypeDM.Monthly,
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
                ClientCompanyDetailId = 1
            };


            apiDb.ClientGenericPayrollComponents.Add(genericPayrollBasic);
            apiDb.ClientGenericPayrollComponents.Add(genericPayrollHra);
            apiDb.ClientGenericPayrollComponents.Add(genericPayrollSa);
            apiDb.ClientGenericPayrollComponents.Add(genericPayrollCa);
            apiDb.ClientGenericPayrollComponents.Add(genericPayrollBonus);
            apiDb.ClientGenericPayrollComponents.Add(genericPayrollEpf);
            apiDb.ClientGenericPayrollComponents.Add(genericPayrollDa);
            apiDb.SaveChanges();


        }

        private void SeedDummyClientEmployeeBankDetails(ApiDbContext apiDb, string defaultCreatedBy, string defaultUpdatedBy, Func<string, string> encryptorFunc)
        {
            var cEmpBank1 = new ClientEmployeeBankDetailDM()
            {
                ClientUserId = 1,
                ClientCompanyDetailId = 1,
                Branch = "Solina",
                BankName = "JkBank",
                AccountNo = 1234567891234567,
                IfscCode = "JAKA0SGR",
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow
            };

            var cEmpBank2 = new ClientEmployeeBankDetailDM()
            {
                ClientUserId = 2,
                ClientCompanyDetailId = 2,
                Branch = "Solina",
                BankName = "SBIBank",
                AccountNo = 1234567891234567,
                IfscCode = "JAKA0SGR",
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
            };

            var cEmpBank3 = new ClientEmployeeBankDetailDM()
            {
                ClientUserId = 3,
                ClientCompanyDetailId = 1,
                Branch = "Solina",
                BankName = "HDFCBank",
                AccountNo = 1234567891234567,
                IfscCode = "JAKA0SGR",
                CreatedBy = "Seeder",
                CreatedOnUTC = DateTime.UtcNow,
            };

            apiDb.ClientEmployeeBankDetails.Add(cEmpBank1);
            apiDb.ClientEmployeeBankDetails.Add(cEmpBank2);
            apiDb.ClientEmployeeBankDetails.Add(cEmpBank3);
            apiDb.SaveChanges();

        }

        #endregion Application Specific Tables

        #endregion Data To Entities
    }
}
