using Microsoft.AspNetCore.Http;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.Enums;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.AppUsers;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public partial class ClientUserProcess : LoginUserProcess<ClientUserSM>
    {
        #region --Properties--

        private readonly IPasswordEncryptHelper _passwordEncryptHelper;
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        private readonly APIConfiguration _apiConfiguration;

        #endregion --Properties--

        #region --Constructor--

        public ClientUserProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, IPasswordEncryptHelper passwordEncryptHelper, ClientCompanyDetailProcess clientCompanyDetailProcess, APIConfiguration apiConfiguration)
            : base(mapper, loginUserDetail, apiDbContext)
        {
            _passwordEncryptHelper = passwordEncryptHelper;
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            _apiConfiguration = apiConfiguration;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientUserSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientUsers;
            IQueryable<ClientUserSM> retSM = await MapEntityAsToQuerable<ClientUserDM, ClientUserSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD 

        #region --Get--

        /// <summary>
        /// Get All ClientUsers in a database.
        /// </summary>
        /// <returns>Service Model of List of ClientUser in database</returns>
        public async Task<List<ClientUserSM>> GetAllClientUsers()
        {
            var dm = await _apiDbContext.ClientUsers.ToListAsync();
            var sm = _mapper.Map<List<ClientUserSM>>(dm);
            return sm;
        }



        /// <summary>
        /// Get ClientUser Detail Based on ClientUserId in a database.
        /// </summary>
        /// <param name="id">Primary Key of ClientUser Object</param>
        /// <returns>Service Model of List of ClientUser in database</returns>
        public async Task<ClientUserSM> GetClientUserById(int id)
        {
            ClientUserDM clientUserDM = await _apiDbContext.ClientUsers.FindAsync(id);
            if (clientUserDM != null)
            {
                var departmentName = await _apiDbContext.ClientCompanyDepartments.Where(x => x.Id == clientUserDM.ClientCompanyDepartmentId).Select(y => y.DepartmentName).FirstOrDefaultAsync();
                var attendanceShift = await _apiDbContext.ClientCompanyAttendanceShifts.Where(x => x.Id == clientUserDM.ClientCompanyAttendanceShiftId).Select(y => y.ShiftName).FirstOrDefaultAsync();
                ClientUserSM clientUserSM = new ClientUserSM()
                {
                    EmployeeCode = clientUserDM.EmployeeCode,
                    PhoneNumber = clientUserDM.PhoneNumber,
                    FirstName = clientUserDM.FirstName,
                    MiddleName = clientUserDM.MiddleName,
                    LastName = clientUserDM.LastName,
                    PersonalEmailId = clientUserDM.PersonalEmailId,
                    EmailId = clientUserDM.EmailId,
                    DateOfBirth = clientUserDM.DateOfBirth,
                    DateOfJoining = clientUserDM.DateOfJoining,
                    Designation = clientUserDM.Designation,
                    LastWorkingDay = clientUserDM.LastWorkingDay,
                    DateOfResignation = clientUserDM.DateOfResignation,
                    LoginId = clientUserDM.LoginId,
                    Department = departmentName,
                    AttendanceShift = attendanceShift,
                    ClientCompanyAttendanceShiftId = clientUserDM.ClientCompanyAttendanceShiftId,
                    ClientCompanyDepartmentId = clientUserDM.ClientCompanyDepartmentId,
                    Id = clientUserDM.Id,
                    ClientCompanyDetailId = clientUserDM.ClientCompanyDetailId,
                    UserSettingId = clientUserDM.UserSettingId,
                    ProfilePicturePath = clientUserDM.ProfilePicturePath,
                    RoleType = (RoleTypeSM)clientUserDM.RoleType,
                    LoginStatus = (LoginStatusSM)clientUserDM.LoginStatus,
                    IsEmailConfirmed = clientUserDM.IsEmailConfirmed,
                    IsPhoneNumberConfirmed = clientUserDM.IsPhoneNumberConfirmed,
                    EmployeeStatus = (EmployeeStatusSM)clientUserDM.EmployeeStatus,
                    Gender = (GenderSM)clientUserDM.Gender,
                    PasswordHash = clientUserDM.PasswordHash,
                    CreatedBy = clientUserDM.CreatedBy,
                    CreatedOnUTC = clientUserDM.CreatedOnUTC,
                    LastModifiedBy = clientUserDM.LastModifiedBy,
                    LastModifiedOnUTC = clientUserDM.LastModifiedOnUTC,
                };
                return clientUserSM;
            }
            else
            {
                return null;
            }

        }


        public async Task<ClientUserSM> GetClientUserByEmail(string email)
        {
            ClientUserDM clientUserDM = await _apiDbContext.ClientUsers.FirstOrDefaultAsync(x => x.EmailId == email);
            if (clientUserDM != null)
            {
                return _mapper.Map<ClientUserSM>(clientUserDM);
            }
            else
            {
                return null;
            }
        }


        #endregion --Get--

        #region Forgot Password and Reset Password

        /// <summary>
        /// Send Reset Password Link on Mail
        /// </summary>
        /// <param name="forgotPassword">ForgotPassword Object</param>
        /// <param name="companyId">Primary key of ClientCompanyDetailId</param>
        /// <returns>boolean value on success or failure</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<BoolResponseRoot> SendResetPasswordLink(ForgotPasswordSM forgotPassword, string link)
        {
            var timeExpiry = _apiConfiguration.Time;
            DateTime currentTime = DateTime.Now.AddMinutes(timeExpiry);
            forgotPassword.Expiry = currentTime;
            var authCode = await _passwordEncryptHelper.ProtectAsync(forgotPassword);
            authCode = authCode.Replace("+", "%2B");
            if (string.IsNullOrWhiteSpace(forgotPassword.UserName))
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, "User Name cannot be empty.");
            var user = ValidateUserFromDatabaseandGetEmail(forgotPassword);
            if (!string.IsNullOrWhiteSpace(user.email))
            {
                link = $"{link}?authCode={authCode}";
                var subject = "Password Reset Request";
                var body = $"Hi {forgotPassword.UserName}, <br/> You recently requested to reset your password for your account. " +
                    $"Click the link below to reset it. " +
                     $" <br/><br/><a href='{link}'>{link}</a> <br/><br/>" +
                     "If you did not request a password reset, please ignore this email.<br/><br/> Thank you";
                SendEmail(user.email, subject, string.Format(body, forgotPassword.UserName, user.pwd));
                return new BoolResponseRoot(true, "Reset Password Link has been sent Successfully");
            }
            else
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"No ClientUser with username '{forgotPassword.UserName}' found.");
            }
        }

        public async Task<IntResponseRoot> ValidatePassword(string authCode)
        {
            ForgotPasswordSM forgotPassword = await _passwordEncryptHelper.UnprotectAsync<ForgotPasswordSM>(authCode);
            if (string.IsNullOrWhiteSpace(forgotPassword.UserName))
            {
                return new IntResponseRoot((int)ValidatePasswordLinkStatusSM.Invalid, "UserName Not Found");
            }
            if (forgotPassword.Expiry < DateTime.Now)
            {
                return new IntResponseRoot((int)ValidatePasswordLinkStatusSM.Invalid, "Password reset link expired.");
            }
            return new IntResponseRoot((int)ValidatePasswordLinkStatusSM.Valid, "Success");

        }

        public async Task<BoolResponseRoot> UpdatePassword(ResetPasswordRequestSM resetPasswordRequest)
        {
            ForgotPasswordSM forgotPassword = await _passwordEncryptHelper.UnprotectAsync<ForgotPasswordSM>(resetPasswordRequest.authCode);
            if (forgotPassword.Expiry < DateTime.Now)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Link expired.", $"Password reset link expired.");
            }
            if (!string.IsNullOrWhiteSpace(resetPasswordRequest.NewPassword))
            {
                if (string.IsNullOrEmpty(forgotPassword.UserName))
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"No ClientUser with username '{forgotPassword.UserName}' found.", $"No ClientUser with username '{forgotPassword.UserName}' found.");
                }

                var user = (from r in _apiDbContext.ClientUsers
                            where r.LoginId == forgotPassword.UserName.ToUpper()
                            select new { ClientUser = r }).FirstOrDefault();

                if (user != null)
                {
                    string decrypt = "";
                    string newPassword = await _passwordEncryptHelper.ProtectAsync<string>(resetPasswordRequest.NewPassword);
                    if (string.Equals(user.ClientUser.PasswordHash, newPassword))
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Please don't use old password, use new one.", $"Please don't use old password, use new one.");
                    }
                    else
                    {
                        resetPasswordRequest.NewPassword = await _passwordEncryptHelper.ProtectAsync(resetPasswordRequest.NewPassword);
                        user.ClientUser.PasswordHash = resetPasswordRequest.NewPassword;
                        await _apiDbContext.SaveChangesAsync();
                        return new BoolResponseRoot(true, "Password Updated Successfully");
                    }
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"No ClientUser with username '{forgotPassword.UserName}' found.", $"No ClientUser with username '{forgotPassword.UserName}' found.");
                }
            }
            else
            {

                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, "Password can not be empty", "Password should not contain Spaces.");
            }
        }

        #endregion Forgot Password and Reset Password

        #region --My-Get-Methods--

        /// <summary>
        /// Get All ClientUsers in a database by Company-Id.
        /// </summary>
        /// <param name="currentCompanyId">Primary Key of ClientCompany Object</param>
        /// <returns>Service Model of List of ClientUser in database</returns>
        public async Task<List<ClientUserSM>> GetAllClientUsersOfMyCompany(int currentCompanyId)
        {
            var dm = await _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<ClientUserSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientUser in a Database By ClientUserId  and CompanyId.
        /// </summary>
        /// <param name="id">Primary Key of ClientUser Object</param>
        /// <param name="currenCompanyId">Primary Key of ClientComapnyDetail Object.</param>
        /// <returns></returns>

        public async Task<ClientUserSM> GetClientUserByIdOfMyCompany(int id, int currenCompanyId)
        {
            ClientUserDM clientUserDM = await _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currenCompanyId && x.Id == id).FirstOrDefaultAsync();

            if (clientUserDM != null)
            {
                return _mapper.Map<ClientUserSM>(clientUserDM);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get BadgeCard Detail in a Database based on ClientUserId and CompanyId.
        /// </summary>
        /// <param name="id">Primary Key of ClientUser Object</param>
        /// <param name="currentCompanyId">Primary Key of CleintCompanyDetail Object.</param>
        /// <returns>Service Model of List of ClientUser in database</returns>

        public async Task<BadgeIdCardsSM> GetBadgeIdCardByIdOfMyCompany(int id, int currentCompanyId)
        {
            ClientUserDM clientUserDM = await _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.Id == id).FirstOrDefaultAsync();
            if (clientUserDM == null)
            {
                return null;
            }
            var company = await _clientCompanyDetailProcess.GetClientCompanyDetailById(clientUserDM.ClientCompanyDetailId);
            BadgeIdCardsSM badgeIdCardsSM = new BadgeIdCardsSM();
            badgeIdCardsSM.ProfilePicture = clientUserDM.ProfilePicturePath == null ? clientUserDM.ProfilePicturePath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(clientUserDM.ProfilePicturePath)));
            badgeIdCardsSM.Id = clientUserDM.Id;
            badgeIdCardsSM.EmployeeName = clientUserDM.LoginId;
            badgeIdCardsSM.EmployeeMail = clientUserDM.PersonalEmailId;
            badgeIdCardsSM.EmployeePhone = clientUserDM.PhoneNumber;
            badgeIdCardsSM.EmployeeDesignation = clientUserDM.Designation;
            badgeIdCardsSM.CompanyName = company.Name;
            badgeIdCardsSM.CompanyPhone = company.CompanyMobileNumber;
            badgeIdCardsSM.CompanyEmail = company.CompanyContactEmail;
            badgeIdCardsSM.CompanyWebsite = company.CompanyWebsite;
            badgeIdCardsSM.IssuedDate = DateTime.UtcNow;
            badgeIdCardsSM.ExpiryDate = DateTime.UtcNow.AddYears(1);
            return badgeIdCardsSM;
        }

        /// <summary>
        /// Get ClientUser Name in a database by ClientUserId.
        /// </summary>
        /// <param name="clientUserId">Primary Key of ClientUser Object</param>
        /// <returns>The value of UserName from a DataBase.</returns>
        public async Task<string> GetUserName(int clientUserId)
        {
            var userName = await _apiDbContext.ClientUsers.Where(x => x.Id == clientUserId).Select(x => x.LoginId).FirstOrDefaultAsync();
            return userName;
        }

        public async Task<int?> GetUserId(int clientCompanyDetailId, RoleTypeSM roleType)
        {
            var clientUserId = await _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == clientCompanyDetailId && x.RoleType == (RoleTypeDM)roleType).Select(x => x.Id).FirstOrDefaultAsync();

            if (clientUserId != 0)
            {
                return (clientUserId);
            }
            else
            {
                return null;
            }
        }

        #endregion --My-Get-Methods--

        #region --Count--

        /// <summary>
        /// Get ClientEmployeeUser Count by EmployeeId in database.
        /// </summary>
        /// <param name="companyId">Primary key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>

        public async Task<int> GetClientCompanyUserCountsResponse(int companyId)
        {
            int resp = _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == companyId && x.RoleType == RoleTypeDM.ClientAdmin).Count();
            return resp;
        }

        /// <summary>
        /// Get ClientEmployeeUser Count by EmployeeId in database.
        /// </summary>
        /// <param name="companyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>integer response based on employeeId</returns>

        public async Task<int> GetClientEmployeeUserCountResponse(int companyId)
        {
            int resp = _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == companyId && x.RoleType == RoleTypeDM.ClientEmployee).Count();
            return resp;
        }

        /// <summary>
        /// Get All ClientEmployeeUser Report Based On Status Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param> <returns>integer response based on CompanyId</returns></returns>
        public async Task<int> GetEmployeeReportCountBasedOnStatus(EmployeeStatusSM employeeStatusSM, int currentCompanyId)
        {
            var reportCount = 0;
            switch (employeeStatusSM)
            {
                case EmployeeStatusSM.Active:
                    reportCount = _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Active) && (x.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                case EmployeeStatusSM.Resigned:
                    reportCount = _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Resigned) && (x.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                case EmployeeStatusSM.Suspended:
                    reportCount = _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Suspended) && (x.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                case EmployeeStatusSM.Retired:
                    reportCount = _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Retired) && (x.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                case EmployeeStatusSM.Expelled:
                    reportCount = _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Expelled) && (x.ClientCompanyDetailId == currentCompanyId)).Count();
                    break;
                default:
                    break;
            }
            return reportCount;
        }

        #endregion --Count--

        #region --Add Update--

        /// <summary>
        /// Add new ClientUser
        /// </summary>
        /// <param name="clientUserSM">ClientUser object</param>
        /// <returns> the added record</returns>
        public async Task<ClientUserSM> AddClientUser(ClientUserSM clientUserSM)
        {
            var objDM = _mapper.Map<ClientUserDM>(clientUserSM);
            objDM.CreatedBy = _loginUserDetail.LoginId;
            objDM.CreatedOnUTC = DateTime.UtcNow;
            objDM.PasswordHash = await _passwordEncryptHelper.ProtectAsync(objDM.PasswordHash);
            await _apiDbContext.ClientUsers.AddAsync(objDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientUserSM>(objDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientUser of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientUserSM">ClientUser object to update</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<ClientUserSM> UpdateClientUser(int objIdToUpdate, ClientUserSM clientUserSM)
        {
            if (clientUserSM != null && objIdToUpdate > 0)
            {
                ClientUserDM objDM = await _apiDbContext.ClientUsers.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    clientUserSM.Id = objIdToUpdate;
                    clientUserSM.PasswordHash = objDM.PasswordHash;
                    _mapper.Map(clientUserSM, objDM);

                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientUserSM>(objDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        public async Task<string> AddOrUpdateProfilePictureInDb(int userId, string webRootPath, IFormFile postedFile)
            => await base.AddOrUpdateProfilePictureInDb(await _apiDbContext.ClientUsers.FirstOrDefaultAsync(x => x.Id == userId), webRootPath, postedFile);


        #endregion --Add Update--

        #region Mine-Update

        /// <summary>
        /// Update ClientUser of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="themeId">Primary Key of ClientTheme Id</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<BoolResponseRoot> UpdateClientUserTheme(int themeId, int objIdToUpdate)
        {
            if (objIdToUpdate > 0)
            {
                ClientUserDM objDM = await _apiDbContext.ClientUsers.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    objDM.UserSettingId = themeId;
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new BoolResponseRoot(true, "Updated Successfully");
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion Mine-Update

        #region My Update-Department

        /// <summary>
        /// Update ClientUser Department of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="DepartmentId">Primary Key of ClientCompanyDepartment Id</param>
        /// <param name="currentCompanyId">Primary Key of CLientCompanyDetail Id</param>
        /// <returns>boolean for success in updating record</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<BoolResponseRoot> UpdateClientUserDepartment(int DepartmentId, int objIdToUpdate, int currentCompanyId)
        {
            if (objIdToUpdate > 0)
            {
                ClientUserDM objDM = await _apiDbContext.ClientUsers.Where(x => x.Id == objIdToUpdate && x.ClientCompanyDetailId == currentCompanyId).FirstOrDefaultAsync();
                if (objDM != null)
                {
                    objDM.ClientCompanyDepartmentId = DepartmentId;
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new BoolResponseRoot(true, "Updated Successfully");
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion My Update-Department

        #region --Add/Update My & Mine Profile-Picture

        /// <summary>
        /// Add or Update New Profile Picture for ClientUser.
        /// </summary>
        /// <param name="profileImage">Image to be Added or Updated</param>
        /// <param name="currentUserId">Id to Update</param>
        /// <returns>The Added/Updated Image</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<string> AddUpdateMineProfilePicture(string profileImage, int currentUserId)
        {
            if (!String.IsNullOrWhiteSpace(profileImage) && currentUserId > 0)
            {
                var objDM = await _apiDbContext.ClientUsers.FindAsync(currentUserId);
                if (objDM != null)
                {
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    Byte[] pictureToUploadBytes = Convert.FromBase64String(profileImage);
                    string profilePic = objDM.LoginId + ".jpeg";
                    var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ProfilePictures"));

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, profilePic);
                    File.WriteAllBytes(filePath, pictureToUploadBytes);
                    objDM.ProfilePicturePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return profileImage;
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {currentUserId}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        /// <summary>
        /// Gets Profile Picture of a LoggedIn User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the image the image in the form of base64 from a database</returns>
        public async Task<string> GetMineProfilePicture(int id, RoleTypeSM roleTypeSM)
        {
            if (roleTypeSM == RoleTypeSM.SuperAdmin || roleTypeSM == RoleTypeSM.SuperAdmin)
            {
                var superUserDM = await _apiDbContext.ApplicationUsers.Where(x => x.Id == id && x.RoleType == (RoleTypeDM)roleTypeSM).Select(x => new { x.ProfilePicturePath }).FirstOrDefaultAsync();
                var profilePicFullPath = superUserDM.ProfilePicturePath == null ? null : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(superUserDM.ProfilePicturePath)));
                return profilePicFullPath;
            }
            else
            {
                var clientUserDM = await _apiDbContext.ClientUsers.Where(x => x.Id == id && x.RoleType == (RoleTypeDM)roleTypeSM).Select(x => new { x.ProfilePicturePath }).FirstOrDefaultAsync();
                var profilePicFullPath = clientUserDM.ProfilePicturePath == null ? null : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(clientUserDM.ProfilePicturePath)));
                return profilePicFullPath;
            }
        }

        /// <summary>
        /// Add or Update Profile Picture of Employees 
        /// </summary>
        /// <param name="profileImage">Image to be added</param>
        /// <param name="currentCompanyId">Primary Key Of a ClientCompanyDetail</param>
        /// <param name="currentUserId">Id to be Updated with Image</param>
        /// <returns>The Added/Updated Image</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<string> AddUpdateMyClientProfilePicture(string profileImage, int currentCompanyId, int currentUserId)
        {
            if (!String.IsNullOrWhiteSpace(profileImage) && currentUserId > 0)
            {
                var objDM = await _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.Id == currentUserId).FirstOrDefaultAsync();
                if (objDM != null)
                {
                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;
                    Byte[] pictureToUploadBytes = Convert.FromBase64String(profileImage);
                    string profilePic = objDM.LoginId + ".jpeg";
                    var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ProfilePictures"));

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, profilePic);
                    File.WriteAllBytes(filePath, pictureToUploadBytes);
                    objDM.ProfilePicturePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return profileImage;
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientUser not found: {currentUserId}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update Mine & My Profile-Picture

        #region --My & Mine Delete-Profile-Picture EndPoint--

        /// <summary>
        /// Delete User ProfilePicture by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteUserProfilePictureById(int id, int currentCompanyId)
        {
            var objDM = await _apiDbContext.ClientUsers.Where(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId && x.ProfilePicturePath != null).FirstOrDefaultAsync();

            if (objDM != null)
            {

                var profilePictureFilePath = Path.GetFullPath(objDM.ProfilePicturePath);
                var profilePictureFolderPath = Path.GetDirectoryName(profilePictureFilePath);
                if (Directory.Exists(profilePictureFolderPath))
                {
                    File.Delete(profilePictureFilePath);

                    objDM.ProfilePicturePath = null;
                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return new DeleteResponseRoot(true, "Profile Picture deleted successfully.");
                    }
                }
                return new DeleteResponseRoot(false, "Profile Picture could not be deleted, please try again.");
            }
            return new DeleteResponseRoot(false, "Profile Picture Not found");

        }


        #endregion --My & Mine Delete-Profile-Picture EndPoint--

        #region --Delete--

        /// <summary>
        /// Delete ClientUser by  Id
        /// </summary>
        /// <param name="id">primary key of ClientUser</param>
        /// <returns>boolean for success in removing record</returns>
        public async Task<DeleteResponseRoot> DeleteClientUserById(int id)
        {
            var isPresent = await _apiDbContext.ClientUsers.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.ClientUsers  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientUserDM() { Id = id };
                _apiDbContext.ClientUsers.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "ClientUser Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        public async Task<DeleteResponseRoot> DeleteProfilePictureById(int userId, string webRootPath)
            => await base.DeleteProfilePictureById(await _apiDbContext.ClientUsers.FirstOrDefaultAsync(x => x.Id == userId), webRootPath);

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientUser by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyClientUserById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientUsers.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            //Linq to sql syntax
            //(from sub in _apiDbContext.ClientUsers  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientUserDM() { Id = id };
                _apiDbContext.ClientUsers.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "ClientUser Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #region --Report--

        /// <summary>
        /// Get All ClientUser Count in database of a particular company.
        /// </summary>
        /// <param name="baseReportFilter">BaseReportFilterSM object.</param>
        /// <param name="currentCompanyId">Primary key of ClientCompanyDetail</param>
        /// <returns>integer response based on CompanyId.</returns>
        public async Task<int> GetUserReportCount(BaseReportFilterSM baseReportFilter, int currentCompanyId)
        {
            var today = baseReportFilter.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            var reportCount = 0;
            baseReportFilter.SearchString = String.IsNullOrWhiteSpace(baseReportFilter.SearchString) ? null : baseReportFilter.SearchString.Trim();
            if (!String.IsNullOrWhiteSpace(baseReportFilter.SearchString))
            {
                reportCount = _apiDbContext.ClientUsers.Count(x => x.ClientCompanyDetailId == currentCompanyId &&
                (x.LoginId.Contains(baseReportFilter.SearchString)) || (x.FirstName.Contains(baseReportFilter.SearchString)) || (x.LastName.Contains(baseReportFilter.SearchString)));
                return reportCount;
            }
            switch (baseReportFilter.DateFilterType)
            {
                case DateFilterTypeSM.Monthly:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case DateFilterTypeSM.Yearly:
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case DateFilterTypeSM.Custom:
                    startDate = baseReportFilter.DateFrom;
                    endDate = baseReportFilter.DateTo;
                    break;
                default:
                    break;
            }
            reportCount = _apiDbContext.ClientUsers.Where(x => (((x.DateOfJoining > startDate && x.DateOfJoining < endDate))) && (x.ClientCompanyDetailId == currentCompanyId)).Count();
            return reportCount;
        }

        /// <summary>
        /// Gets all ClientEmployee report.
        /// </summary>
        /// <param name="baseReportFilterSM">BaseReportFilterSM object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="skip">integer parameter</param>
        /// <param name="top">integer parameter</param>
        /// <returns>Service Model of List of ClientEmployeeCTCDetailExtendedUser in database.</returns>

        public async Task<List<ClientUserSM>> GetAllClientUsersReport(BaseReportFilterSM baseReportFilterSM, int currentCompanyId, int skip, int top)
        {
            List<ClientUserDM> clientUserDMs = new List<ClientUserDM>();
            var today = baseReportFilterSM.DateFrom;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            if (skip != -1 && top != -1)
            {
                switch (baseReportFilterSM.DateFilterType)
                {
                    case DateFilterTypeSM.Monthly:
                        startDate = new DateTime(today.Year, today.Month, 1);
                        endDate = startDate.AddMonths(1).AddDays(-1);
                        break;
                    case DateFilterTypeSM.Yearly:
                        startDate = new DateTime(today.Year, 1, 1);
                        endDate = new DateTime(today.Year, 12, 31);
                        break;
                    case DateFilterTypeSM.Custom:
                        startDate = baseReportFilterSM.DateFrom;
                        endDate = baseReportFilterSM.DateTo;
                        break;
                    default:
                        break;
                }
                baseReportFilterSM.SearchString = String.IsNullOrWhiteSpace(baseReportFilterSM.SearchString) ? null : baseReportFilterSM.SearchString.Trim();
                clientUserDMs = await _apiDbContext.ClientUsers.Where(x => (((x.DateOfJoining > startDate && x.DateOfJoining < endDate) || (x.DateOfJoining > startDate && x.DateOfJoining < endDate) || (x.LoginId.Contains(baseReportFilterSM.SearchString)) || (x.FirstName.Contains(baseReportFilterSM.SearchString) || (x.LastName.Contains(baseReportFilterSM.SearchString))))) && (x.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
            }
            else
            {
                clientUserDMs = await _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();

            }
            var clientUsers = _mapper.Map<List<ClientUserSM>>(clientUserDMs);
            return clientUsers;
        }


        /// <summary>
        /// Gets all ClientEmployee report Based on Employee Status.
        /// </summary>
        /// <param name="employeeStatusSM">EmployeeStatus object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of List of ClientEmployeeCTCDetailExtendedUser in database.</returns>

        public async Task<List<ClientUserSM>> GetAllClientUsersReportBasedOnStatus(EmployeeStatusSM employeeStatusSM, int currentCompanyId)
        {
            List<ClientUserDM> dm = new List<ClientUserDM>();
            switch (employeeStatusSM)
            {
                case EmployeeStatusSM.Active:
                    dm = await _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Active) && (x.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
                    break;
                case EmployeeStatusSM.Resigned:
                    dm = await _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Resigned) && (x.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
                    break;
                case EmployeeStatusSM.Suspended:
                    dm = await _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Suspended) && (x.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
                    break;
                case EmployeeStatusSM.Retired:
                    dm = await _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Retired) && (x.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
                    break;
                case EmployeeStatusSM.Expelled:
                    dm = await _apiDbContext.ClientUsers.Where(x => (x.EmployeeStatus == EmployeeStatusDM.Expelled) && (x.ClientCompanyDetailId == currentCompanyId)).ToListAsync();
                    break;
                default:
                    break;
            }
            var clientUsers = _mapper.Map<List<ClientUserSM>>(dm);
            return clientUsers;
        }

        #endregion --Report--

        #region --Change Password--

        /// <summary>
        /// This Function is used for 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Service Model of UpdateCredentialSM in database.</returns>

        public async Task<UpdateCredentialSM> ChangePassword(int userId)
        {
            UpdateCredentialSM updateCredential = new UpdateCredentialSM();
            var dm = await GetClientUserById(userId);
            updateCredential = new UpdateCredentialSM()
            {
                Username = dm.LoginId,
                UserId = dm.Id,
            };
            return updateCredential;
        }

        /// <summary>
        /// This Function is used for Change Password of a User.
        /// </summary>
        /// <param name="updateCredentialSM">UpdateCredentialSM Object.</param>
        /// <returns>Boolean reponse.</returns>
        /// <exception cref="SiffrumPayrollException"></exception>
        public async Task<BoolResponseRoot> ChangePassword(UpdateCredentialSM updateCredentialSM, int userId)
        {
            if (!string.IsNullOrWhiteSpace(updateCredentialSM.NewPassword))
            {
                string oldPassword = await _passwordEncryptHelper.ProtectAsync<string>(updateCredentialSM.OldPassword);
                if (String.IsNullOrWhiteSpace(updateCredentialSM.OldPassword))
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Old Password cannot be Empty, use new one.", $"Old Password cannot be Empty, use new one.");
                }
                var user = (from r in _apiDbContext.ClientUsers
                            where r.LoginId == updateCredentialSM.Username.ToLower() && r.PasswordHash == oldPassword && r.Id == userId && r.EmployeeStatus == EmployeeStatusDM.Active
                            select new { ClientUser = r }).FirstOrDefault();

                if (user != null)
                {
                    string decrypt = "";
                    string newPassword = await _passwordEncryptHelper.ProtectAsync<string>(updateCredentialSM.NewPassword);
                    if (string.Equals(oldPassword, newPassword))
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Old Password and New Password cannot be same, use new one.", $"Old Password and New Password cannot be same, use new one.");
                    }
                    if (string.Equals(user.ClientUser.PasswordHash, newPassword))
                    {
                        throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Please don't use old password, use new one.", $"Please don't use old password, use new one.");
                    }
                    else
                    {
                        user.ClientUser.PasswordHash = newPassword;
                        user.ClientUser.LoginStatus = LoginStatusDM.Enabled;
                        await _apiDbContext.SaveChangesAsync();
                        return new BoolResponseRoot(true, "Password Updated Successfully");
                    }
                }
                else
                {
                    return new BoolResponseRoot(false, "UserName or Old Password Not Found.");
                }
            }
            else
            {

                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, "Password can not be empty", "Password should not contain Spaces.");
            }
        }

        #endregion --Change Password--

        #region --Dashboard--
        /// <summary>
        /// Get the DashBoard Details.
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns> the count based on CompanyId</returns>
        public async Task<DashBoardSM> GetDashBoardDetails(int currentCompanyId)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMonths(-1).AddDays(-1);
            int employeesCount = _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.RoleType == RoleTypeDM.ClientEmployee).Count();
            int adminCount = _apiDbContext.ClientUsers.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.RoleType == RoleTypeDM.ClientAdmin).Count();
            int leavesApprovedCount = _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.IsApproved == true && (x.LeaveDateFromUTC.Date <= startDate.Date)).Count();
            int leavesPendingCount = _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.IsApproved == null && (x.LeaveDateFromUTC.Date.Date <= startDate.Date.Date)).Count();
            int leavesRejectedCount = _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.IsApproved == false && (x.LeaveDateFromUTC.Date <= startDate.Date)).Count();
            int employeePresent = _apiDbContext.ClientEmployeeAttendances.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.AttendanceStatus == AttendanceStatusDM.P && x.AttendanceDate.Date == startDate.Date.Date).Count();
            int employeeAbsent = _apiDbContext.ClientEmployeeAttendances.Where(x => x.ClientCompanyDetailId == currentCompanyId && x.AttendanceStatus == AttendanceStatusDM.A && x.AttendanceDate.Date == startDate.Date).Count();
            int employeeLeave = _apiDbContext.ClientEmployeeLeaves.Where(x => x.ClientCompanyDetailId == currentCompanyId && (x.LeaveDateFromUTC.Date == startDate.Date || x.LeaveDateToUTC.Date >= startDate.Date)).Count();
            int departmentCount = _apiDbContext.ClientCompanyDepartments.Where(x => x.ClientCompanyDetailId == currentCompanyId).Count();
            var companyDepartments = await _apiDbContext.ClientCompanyDepartments.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            int departmentEmployeeCount = 0;

            List<ClientCompanyDepartmentReportSM> clientCompanyDepartmentReports = new List<ClientCompanyDepartmentReportSM>();
            foreach (var item in companyDepartments)
            {
                departmentEmployeeCount = _apiDbContext.ClientUsers.Count(x => x.ClientCompanyDepartmentId == item.Id && x.ClientCompanyDetailId == currentCompanyId);
                clientCompanyDepartmentReports.Add(new ClientCompanyDepartmentReportSM()
                {
                    DepartmentName = item.DepartmentName,
                    EmployeeCount = departmentEmployeeCount,
                });
            }


            DashBoardSM dashBoardSM = new DashBoardSM()
            {
                NumberOfEmployees = employeesCount,
                NumberOfAdmins = adminCount,
                NumberOfDepartments = departmentCount,
                NumberOfLeavesApproved = leavesApprovedCount,
                NumberOfLeavesPending = leavesPendingCount,
                NumberOfLeavesRejected = leavesRejectedCount,
                ClientCompanyDepartment = clientCompanyDepartmentReports != null ? clientCompanyDepartmentReports : null,
                NumberOfEmployeeOnLeave = employeeLeave != 0 ? employeeLeave : 0,
                NumberOfEmployeesAbsent = employeeAbsent != 0 ? employeeAbsent : 0,
                NumberOfEmployeesPresent = employeePresent != 0 ? employeePresent : 0,
            };
            return dashBoardSM;
        }

        #endregion --DashBoard--

        #endregion CRUD

        #region Private Functions
        private BoolResponseRoot SendEmail(string emailTo, string subject, string message, List<Attachment> attachments = null)
        {
            MailMessage mailMessage = new MailMessage("hazimaltaf889@gmail.com", emailTo)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                if (attachments != null)
                {
                    attachments.ForEach(x => mailMessage.Attachments.Add(x));
                }
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("hazimaltaf889@gmail.com", "mzrqbxlbbehwmcia");
                smtpClient.Port = 587;
                smtpClient.Send(mailMessage);
                mailMessage.Attachments.Dispose();
                smtpClient.Dispose();
                return new BoolResponseRoot(true, "Success");
            }
            catch (Exception ex)
            {
                mailMessage.Attachments.Dispose();
                smtpClient.Dispose();
                throw;
            }
        }

        private (string email, string pwd) ValidateUserFromDatabaseandGetEmail(ForgotPasswordSM forgotPassword)
        {
            if (string.IsNullOrWhiteSpace(forgotPassword.UserName))
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, "User Name cannot be empty.");
            var user = (from u in _apiDbContext.ClientUsers
                        where u.LoginId.ToUpper() == forgotPassword.UserName.ToUpper()
                        select new { ClientUser = u }).FirstOrDefault();
            if (user == null)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"No ClientUser with username '{forgotPassword.UserName}' found.", $"No ClientUser with username '{forgotPassword.UserName}' found.");
            }
            if (string.IsNullOrWhiteSpace(user.ClientUser.EmailId))
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"Please Update Email For ClientUser With Username '{forgotPassword.UserName}'.");
            return (user.ClientUser.EmailId, user.ClientUser.PasswordHash);
        }

        #endregion Private Functions

    }
}
