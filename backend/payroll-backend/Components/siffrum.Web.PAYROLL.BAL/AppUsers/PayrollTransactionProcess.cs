using DinkToPdf;
using DinkToPdf.Contracts;
using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.BAL.Client;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.AppUsers;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Enums;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.IO;

namespace Siffrum.Web.Payroll.BAL.AppUsers
{
    public class PayrollTransactionProcess : SiffrumPayrollBalOdataBase<PayrollTransactionSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly IConverter _converter;
        private readonly ClientEmployeeCTCDetailProcess _clientEmployeeCTCDetailProcess;
        private readonly ClientUserProcess _clientUserProcess;
        private readonly ClientUserAddressProcess _clientUserAddressProcess;
        private readonly ClientCompanyDetailProcess _clientCompanyDetailProcess;
        private readonly ClientCompanyAddressProcess _clientCompanyAddressProcess;

        #endregion --Properties--

        #region --Constructor--

        public PayrollTransactionProcess(IMapper mapper, ILoginUserDetail loginUserDetail, IConverter converter, ApiDbContext apiDbContext, ClientEmployeeCTCDetailProcess clientEmployeeCTCDetailProcess, ClientUserProcess clientUserProcess, ClientCompanyDetailProcess clientCompanyDetailProcess, ClientUserAddressProcess clientUserAddressProcess, ClientCompanyAddressProcess clientCompanyAddressProcess)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _clientEmployeeCTCDetailProcess = clientEmployeeCTCDetailProcess;
            _clientUserProcess = clientUserProcess;
            _clientCompanyDetailProcess = clientCompanyDetailProcess;
            _converter = converter;
            _clientUserAddressProcess = clientUserAddressProcess;
            _clientCompanyAddressProcess = clientCompanyAddressProcess;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<PayrollTransactionSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.PayrollTransactions;
            IQueryable<PayrollTransactionSM> retSM = await MapEntityAsToQuerable<PayrollTransactionDM, PayrollTransactionSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD

        #region --Get--

        /// <summary>
        /// Get All PaymentGeneration details in database
        /// </summary>
        /// <returns>Service Model of List of PaymentGeneration in database</returns>
        public async Task<List<PayrollTransactionSM>> GetAllPaymentGenerations()
        {
            var dm = await _apiDbContext.PayrollTransactions.ToListAsync();
            var sm = _mapper.Map<List<PayrollTransactionSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get All GeneratePayrollTransactions details in database based on Month.
        /// </summary>
        /// <param name="dateTime">Query String</param>
        /// <returns>Service Model of List of GeneratePayrollTransactions in database.</returns>

        public async Task<List<GeneratePayrollTransactionSM>> GetTotalPayrollTransactions(DateTime dateTime)
        {
            var monthFourDigit = dateTime.ToString("MMMM-yyyy");
            var monthThreeDigit = dateTime.ToString("MMM-yyyy");
            var monthTwoDigit = dateTime.ToString("MM-yy");

            var lastMonth = dateTime.AddMonths(-1);

            var employees = await _apiDbContext.ClientUsers
                .Where(x => x.DateOfJoining < lastMonth)
                .ToListAsync();

            var payrollTransactions = await _apiDbContext.PayrollTransactions
                .Where(x => x.PaymentFor.Contains(monthFourDigit) ||
                            x.PaymentFor.Contains(monthThreeDigit) ||
                            x.PaymentFor.Contains(monthTwoDigit))
                .ToListAsync();

            var generatePayrolls = new List<GeneratePayrollTransactionSM>();
            foreach (var employee in employees)
            {
                var matchingPayrollTransaction = payrollTransactions
                    .FirstOrDefault(x => x.ClientUserId == employee.Id);

                if (matchingPayrollTransaction != null)
                {
                    generatePayrolls.Add(new GeneratePayrollTransactionSM()
                    {
                        Id = matchingPayrollTransaction.Id,
                        EmployeeName = employee.LoginId,
                        EmployeeStatus = (EmployeeStatusSM)employee.EmployeeStatus,
                        Designation = employee.Designation,
                        ClientUserId = employee.Id,
                        PaymentAmount = matchingPayrollTransaction.PaymentAmount,
                        PaymentFor = monthFourDigit,
                        PaymentMode = (PaymentModeSM)matchingPayrollTransaction.PaymentMode,
                        PaymentType = (PaymentTypeSM)matchingPayrollTransaction.PaymentType,
                        PaymentPaid = matchingPayrollTransaction.PaymentPaid,
                        ClientEmployeeCTCDetailId = matchingPayrollTransaction.ClientEmployeeCTCDetailId,
                        CreatedBy = _loginUserDetail.LoginId,
                        CreatedOnUTC = DateTime.UtcNow
                    });
                }
                else
                {
                    generatePayrolls.Add(new GeneratePayrollTransactionSM()
                    {
                        EmployeeName = employee.LoginId,
                        EmployeeStatus = (EmployeeStatusSM)employee.EmployeeStatus,
                        Designation = employee.Designation,
                        ClientUserId = employee.Id,
                        ErrorInGeneration = true,
                        ErrorMessage = "CTC Details Not Found for the Employee."
                    });
                }
            }

            return generatePayrolls;
        }

        /// <summary>
        /// Get PayrollTransaction Details by Id
        /// </summary>
        /// <param name="id">Primary Key of PayrollTransaction</param>
        /// <returns>Service Model of PayrollTransaction in database of the id</returns>

        public async Task<PayrollTransactionSM> GetPayrollTransactionsById(int id)
        {
            PayrollTransactionDM payrollTransactionDM = await _apiDbContext.PayrollTransactions.FindAsync(id);
            if (payrollTransactionDM != null)
            {
                return _mapper.Map<PayrollTransactionSM>(payrollTransactionDM);
            }
            else
            {
                return null;
            }
        }

        public async Task<PayrollTransactionSM> GetPayrollTransactionsByUserIdBasedOnDateTime(int id, DateTime dateTime)
        {
            var monthFourDigit = dateTime.ToString("MMMM-yyyy");
            var monthThreeDigit = dateTime.ToString("MMM-yyyy");
            var monthTwoDigit = dateTime.ToString("MM-yy");
            PayrollTransactionDM payrollTransactionDM = await _apiDbContext.PayrollTransactions.Where(x => x.ClientUserId == id && (x.PaymentFor.Contains(monthTwoDigit) || x.PaymentFor.Contains(monthThreeDigit) || x.PaymentFor.Contains(monthFourDigit))).FirstOrDefaultAsync();
            if (payrollTransactionDM != null)
            {
                return _mapper.Map<PayrollTransactionSM>(payrollTransactionDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --My-Endpoints--

        /// <summary>
        /// Get All GeneratePayrollTransactions details in database based on Month.
        /// </summary>
        /// <param name="dateTime">Query String DateTime</param>
        /// <returns>Service Model of List of GeneratePayrollTransactions in database.</returns>

        public async Task<List<GeneratePayrollTransactionSM>> GetAllPayrollTransactionsOfMyCompany(int currentCompanyId, DateTime dateTime, int skip, int top)
        {
            var monthFourDigit = dateTime.ToString("MMMM-yyyy");
            var monthThreeDigit = dateTime.ToString("MMM-yyyy");
            var monthTwoDigit = dateTime.ToString("MM-yy");

            var lastMonth = dateTime.AddMonths(-1);

            List<ClientUserDM> clientUserDMs = new List<ClientUserDM>();
            if (skip != -1 && top != -1)
            {
                clientUserDMs = await _apiDbContext.ClientUsers
                .Where(x =>
                x.DateOfJoining < lastMonth &&
                x.ClientCompanyDetailId == currentCompanyId)
                .Skip(skip).Take(top).ToListAsync();
            }
            else
            {
                clientUserDMs = await _apiDbContext.ClientUsers
                .Where(x =>
                x.DateOfJoining < lastMonth &&
                x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            }


            var payrollTransactions = await _apiDbContext.PayrollTransactions
                .Where(x =>
                (x.PaymentFor.Contains(monthFourDigit) ||
                x.PaymentFor.Contains(monthThreeDigit) ||
                x.PaymentFor.Contains(monthTwoDigit)) &&
                (x.ClientUser.ClientCompanyDetailId == currentCompanyId))
                .ToListAsync();

            var generatePayrolls = new List<GeneratePayrollTransactionSM>();
            foreach (var employee in clientUserDMs)
            {
                var matchingPayrollTransaction = payrollTransactions
                    .FirstOrDefault(x => x.ClientUserId == employee.Id);

                if (matchingPayrollTransaction != null)
                {
                    generatePayrolls.Add(new GeneratePayrollTransactionSM()
                    {
                        Id = matchingPayrollTransaction.Id,
                        EmployeeName = employee.LoginId,
                        EmployeeStatus = (EmployeeStatusSM)employee.EmployeeStatus,
                        Designation = employee.Designation,
                        ClientUserId = employee.Id,
                        PaymentAmount = matchingPayrollTransaction.PaymentAmount,
                        PaymentFor = monthFourDigit,
                        PaymentMode = (PaymentModeSM)matchingPayrollTransaction.PaymentMode,
                        PaymentType = (PaymentTypeSM)matchingPayrollTransaction.PaymentType,
                        PaymentPaid = matchingPayrollTransaction.PaymentPaid,
                        ClientEmployeeCTCDetailId = matchingPayrollTransaction.ClientEmployeeCTCDetailId,
                        CreatedBy = _loginUserDetail.LoginId,
                        CreatedOnUTC = DateTime.UtcNow
                    });
                }
                else
                {
                    generatePayrolls.Add(new GeneratePayrollTransactionSM()
                    {
                        EmployeeName = employee.LoginId,
                        EmployeeStatus = (EmployeeStatusSM)employee.EmployeeStatus,
                        Designation = employee.Designation,
                        ClientUserId = employee.Id,
                        ErrorInGeneration = true,
                        ErrorMessage = "CTC Details Not Found for the Employee."
                    });
                }
            }

            return generatePayrolls;
        }

        #endregion --My-EndPoints--

        #region --Count--

        /// <summary>
        /// Get PayrollTransaction Count by CompanyId in database.
        /// </summary>
        /// <param name="currentCompanyId"></param>
        /// <returns>integer response based on CompanyId</returns>
        public async Task<int> GetPayrollTransactionCounts(int currentCompanyId, DateTime dateTime)
        {
            var lastMonth = dateTime.AddMonths(-1);
            int resp = _apiDbContext.ClientUsers.Where(x => x.DateOfJoining < lastMonth && x.ClientCompanyDetailId == currentCompanyId).Count();
            return resp;
        }

        /// <summary>
        /// Get All Payroll Transaction Count in database
        /// </summary>
        /// <param name="currentCompanyId"></param> <returns>integer response based on CompanyId</returns></returns>
        public async Task<int> GetPayrollReportCount(PayrollTransactionReportSM payrollTransactionReportSM, int currentCompanyId)
        {
            var reportCount = 0;
            var monthFourDigit = "";
            var monthThreeDigit = "";
            var monthTwoDigit = "";
            var yearlySearch = "";
            payrollTransactionReportSM.SearchString = String.IsNullOrWhiteSpace(payrollTransactionReportSM.SearchString) ? null : payrollTransactionReportSM.SearchString.Trim();
            if (!String.IsNullOrWhiteSpace(payrollTransactionReportSM.SearchString))
            {
                reportCount = _apiDbContext.PayrollTransactions.Count(x => x.ClientCompanyDetailId == currentCompanyId &&
                (x.ClientUser.LoginId.Contains(payrollTransactionReportSM.SearchString)) || (x.ClientUser.FirstName.Contains(payrollTransactionReportSM.SearchString)) || (x.ClientUser.LastName.Contains(payrollTransactionReportSM.SearchString)));
                return reportCount;
            }
            else
            {
                switch (payrollTransactionReportSM.DateFilterType)
                {
                    case DateFilterTypeSM.Monthly:
                        monthFourDigit = payrollTransactionReportSM.DateFrom.ToString("MMMM-yyyy");
                        monthThreeDigit = payrollTransactionReportSM.DateFrom.ToString("MMM-yyyy");
                        monthTwoDigit = payrollTransactionReportSM.DateFrom.ToString("MM-yy");
                        reportCount = _apiDbContext.PayrollTransactions.Where(x => (x.PaymentFor.Contains(monthFourDigit) || x.PaymentFor.Contains(monthThreeDigit) || x.PaymentFor.Contains(monthTwoDigit)) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
                        break;
                    case DateFilterTypeSM.Yearly:
                        yearlySearch = payrollTransactionReportSM.DateFrom.ToString("yyyy");
                        reportCount = _apiDbContext.PayrollTransactions.Where(x => (x.PaymentFor.Contains(yearlySearch)) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Count();
                        break;
                    default:
                        break;
                }
            }
            return reportCount;
        }

        #endregion -Count--

        #region --Add/Update--

        /// <summary>
        /// Add new PayrollTransaction
        /// </summary>
        /// <param name="payrollTransactionSM">PayrollTransaction object</param>
        /// <returns> the added record</returns>

        public async Task<PayrollTransactionSM> AddGeneratePayroll(PayrollTransactionSM payrollTransactionSM)
        {
            var payrollTransactionDM = _mapper.Map<PayrollTransactionDM>(payrollTransactionSM);
            var ctcDetails = await _clientEmployeeCTCDetailProcess.GetClientEmployeeActiveCtcDetailByEmpId(payrollTransactionDM.ClientUserId);
            decimal amt = 0;
            if (ctcDetails == null)
            {
                throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientEmployeeCtcDetail not found: {payrollTransactionDM.ClientUserId}", $"CTC Details Not Found for the Employee");
            }
            foreach (var item in ctcDetails.ClientEmployeePayrollComponents)
            {
                if (item.ComponentPeriodType == ComponentPeriodTypeSM.Monthly)
                {
                    amt = amt + ((item.AmountYearly) / 12);
                }

            }
            amt = Math.Round(amt, 2);
            payrollTransactionDM.CreatedBy = _loginUserDetail.LoginId;
            payrollTransactionDM.CreatedOnUTC = DateTime.UtcNow;
            payrollTransactionDM.ClientEmployeeCTCDetailId = ctcDetails.Id;
            payrollTransactionDM.PaymentAmount = (float)amt;

            await _apiDbContext.PayrollTransactions.AddAsync(payrollTransactionDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<PayrollTransactionSM>(payrollTransactionDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adding new PayrollTransaction List in a Database.
        /// </summary>
        /// <param name="payrollTransactionSM">List of PayrollTransaction object.</param>
        /// <returns>added list of record.</returns>

        public async Task<List<PayrollTransactionSM>> AddGenerateAllPayroll(List<PayrollTransactionSM> payrollTransactionSM)
        {
            foreach (PayrollTransactionSM item in payrollTransactionSM)
            {
                var ctcDetails = await _clientEmployeeCTCDetailProcess.GetClientEmployeeActiveCtcDetailByEmpId(item.ClientUserId);
                decimal amt = 0;
                if (ctcDetails == null)
                {
                    item.ErrorInGeneration = true;
                    item.ErrorMessage = $"CTC Details Not Found for the Employee";
                }
                else
                {
                    foreach (var items in ctcDetails.ClientEmployeePayrollComponents)
                    {
                        if (items.ComponentPeriodType == ComponentPeriodTypeSM.Monthly)
                        {
                            amt = amt + ((items.AmountYearly) / 12);
                        }
                    }
                    amt = Math.Round(amt, 2);
                    item.ClientEmployeeCTCDetailId = ctcDetails.Id;
                    item.PaymentAmount = (float)amt;
                    var dbItem = _mapper.Map<PayrollTransactionDM>(item);
                    dbItem.CreatedBy = _loginUserDetail.LoginId;
                    dbItem.CreatedOnUTC = DateTime.UtcNow;
                    await _apiDbContext.PayrollTransactions.AddAsync(dbItem);
                    item.Id = dbItem.Id;
                    if (await _apiDbContext.SaveChangesAsync() <= 0)
                    {
                        item.ErrorInGeneration = true;
                        item.ErrorMessage = $"Error in generation";
                    }
                }
            }

            return payrollTransactionSM;

        }
        #endregion --Add/Update--

        #region --Reports--

        /// <summary>
        /// Gets all GeneratePayrollTransaction Report 
        /// </summary>
        /// <param name="payrollTransactionReportSM">PayrollTransactionReportSM Object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail.</param>
        /// <returns>Service Model of List of GeneratePayrollTransaction in database.</returns>
        public async Task<List<GeneratePayrollTransactionSM>> GetTotalPayrollTransactionReport(PayrollTransactionReportSM payrollTransactionReportSM, int currentCompanyId, int skip, int top)
        {
            var monthFourDigit = "";
            var monthThreeDigit = "";
            var monthTwoDigit = "";
            var yearlySearch = "";
            payrollTransactionReportSM.SearchString = String.IsNullOrWhiteSpace(payrollTransactionReportSM.SearchString) ? null : payrollTransactionReportSM.SearchString.Trim();
            List<PayrollTransactionDM> payrollTransaction = new List<PayrollTransactionDM>();
            if (String.IsNullOrWhiteSpace(payrollTransactionReportSM.SearchString))
            {
                switch (payrollTransactionReportSM.DateFilterType)
                {
                    case DateFilterTypeSM.Monthly:
                        monthFourDigit = payrollTransactionReportSM.DateFrom.ToString("MMMM-yyyy");
                        monthThreeDigit = payrollTransactionReportSM.DateFrom.ToString("MMM-yyyy");
                        monthTwoDigit = payrollTransactionReportSM.DateFrom.ToString("MM-yy");
                        payrollTransaction = await _apiDbContext.PayrollTransactions.Where(x => (x.PaymentFor.Contains(monthFourDigit) || x.PaymentFor.Contains(monthThreeDigit) || x.PaymentFor.Contains(monthTwoDigit)) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
                        break;
                    case DateFilterTypeSM.Yearly:
                        yearlySearch = payrollTransactionReportSM.DateFrom.ToString("yyyy");
                        payrollTransaction = await _apiDbContext.PayrollTransactions.Where(x => (x.PaymentFor.Contains(yearlySearch)) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                payrollTransaction = await _apiDbContext.PayrollTransactions.Where(x => (((x.ClientUser.LoginId.Contains(payrollTransactionReportSM.SearchString)) || (x.ClientUser.FirstName.Contains(payrollTransactionReportSM.SearchString) || (x.ClientUser.LastName.Contains(payrollTransactionReportSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId)).Skip(skip).Take(top).ToListAsync();
            }
            List<GeneratePayrollTransactionSM> generatePayrollTransactions = new List<GeneratePayrollTransactionSM>();
            foreach (var item in payrollTransaction)
            {
                var empDetails = await _apiDbContext.ClientUsers.Where(x => x.Id == item.ClientUserId).Select(x => new { x.LoginId, x.EmployeeStatus, x.Designation }).FirstOrDefaultAsync();
                generatePayrollTransactions.Add(new GeneratePayrollTransactionSM()
                {
                    Id = item.Id,
                    EmployeeName = empDetails.LoginId,
                    ClientUserId = item.ClientUserId,
                    EmployeeStatus = (EmployeeStatusSM)empDetails.EmployeeStatus,
                    Designation = empDetails.Designation,
                    PaymentAmount = item.PaymentAmount,
                    PaymentMode = (PaymentModeSM)item.PaymentMode,
                    PaymentType = (PaymentTypeSM)item.PaymentType,
                    PaymentPaid = item.PaymentPaid,
                    PaymentFor = item.PaymentFor,
                    CreatedBy = item.CreatedBy,
                    CreatedOnUTC = item.CreatedOnUTC,
                });
            }
            return generatePayrollTransactions;
        }

        /// <summary>
        /// Gets Payroll Transaction Report by UserId in a database
        /// </summary>
        /// <param name="payrollTransactionReportSM">PayrollTransactionReport Object</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns></returns>
        public async Task<List<GeneratePayrollTransactionSM>> GetTotalPayrollTransactionReportByUserId(PayrollTransactionReportSM payrollTransactionReportSM, int currentCompanyId)
        {
            var monthFourDigit = "";
            var monthThreeDigit = "";
            var monthTwoDigit = "";
            var yearlySearch = "";
            payrollTransactionReportSM.SearchString = String.IsNullOrWhiteSpace(payrollTransactionReportSM.SearchString) ? null : payrollTransactionReportSM.SearchString.Trim();
            List<PayrollTransactionDM> payrollTransaction = new List<PayrollTransactionDM>();
            if (String.IsNullOrWhiteSpace(payrollTransactionReportSM.SearchString))
            {
                switch (payrollTransactionReportSM.DateFilterType)
                {
                    case DateFilterTypeSM.Monthly:
                        monthFourDigit = payrollTransactionReportSM.DateFrom.ToString("MMMM-yyyy");
                        monthThreeDigit = payrollTransactionReportSM.DateFrom.ToString("MMM-yyyy");
                        monthTwoDigit = payrollTransactionReportSM.DateFrom.ToString("MM-yy");
                        payrollTransaction = await _apiDbContext.PayrollTransactions.Where(x => (x.PaymentFor.Contains(monthFourDigit) || x.PaymentFor.Contains(monthThreeDigit) || x.PaymentFor.Contains(monthTwoDigit)) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId) && (x.ClientUserId == payrollTransactionReportSM.ClientUserId)).ToListAsync();
                        break;
                    case DateFilterTypeSM.Yearly:
                        yearlySearch = payrollTransactionReportSM.DateFrom.ToString("yyyy");
                        payrollTransaction = await _apiDbContext.PayrollTransactions.Where(x => (x.PaymentFor.Contains(yearlySearch)) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId) && (x.ClientUserId == payrollTransactionReportSM.ClientUserId)).ToListAsync();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                payrollTransaction = await _apiDbContext.PayrollTransactions.Where(x => (((x.ClientUser.LoginId.Contains(payrollTransactionReportSM.SearchString)) || (x.ClientUser.FirstName.Contains(payrollTransactionReportSM.SearchString) || (x.ClientUser.LastName.Contains(payrollTransactionReportSM.SearchString))))) && (x.ClientUser.ClientCompanyDetailId == currentCompanyId) && (x.ClientUserId == payrollTransactionReportSM.ClientUserId)).ToListAsync();
            }
            List<GeneratePayrollTransactionSM> generatePayrollTransactions = new List<GeneratePayrollTransactionSM>();
            foreach (var item in payrollTransaction)
            {
                var empDetails = await _apiDbContext.ClientUsers.Where(x => x.Id == item.ClientUserId).Select(x => new { x.LoginId, x.EmployeeStatus, x.Designation }).FirstOrDefaultAsync();
                generatePayrollTransactions.Add(new GeneratePayrollTransactionSM()
                {
                    Id = item.Id,
                    EmployeeName = empDetails.LoginId,
                    ClientUserId = item.ClientUserId,
                    EmployeeStatus = (EmployeeStatusSM)empDetails.EmployeeStatus,
                    Designation = empDetails.Designation,
                    PaymentAmount = item.PaymentAmount,
                    PaymentMode = (PaymentModeSM)item.PaymentMode,
                    PaymentType = (PaymentTypeSM)item.PaymentType,
                    PaymentPaid = item.PaymentPaid,
                    PaymentFor = item.PaymentFor,
                    CreatedBy = item.CreatedBy,
                    CreatedOnUTC = item.CreatedOnUTC,
                });
            }
            return generatePayrollTransactions;
        }




        #endregion --Reports--

        #region --Delete--

        /// <summary>
        /// Delete PayrollTransaction by  Id.
        /// </summary>
        /// <param name="id">primary key of PayrollTransaction</param>
        /// <returns>Service Model of DeleteResponseRoot in removing record.</returns>

        public async Task<DeleteResponseRoot> DeletePayrollTransaction(int id)
        {
            var isPresent = await _apiDbContext.PayrollTransactions.AnyAsync(x => x.Id == id);
            if (isPresent)
            {
                var dmToDelete = new PayrollTransactionDM() { Id = id };
                _apiDbContext.PayrollTransactions.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "PayrollTransaction Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete PayrollTransaction by  Id
        /// </summary>
        /// <param name="id">Primary key of ClientUser</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>The Service Model of DeleteResponseRoot from a database</returns>
        public async Task<DeleteResponseRoot> DeleteMyPayrollTransactionById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.PayrollTransactions.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            if (isPresent)
            {
                var dmToDelete = new PayrollTransactionDM() { Id = id };
                _apiDbContext.PayrollTransactions.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "PayrollTransaction Detail Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #region --Generate PaySlips--

        /// <summary>
        /// Generate Payslips
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<string> GeneratePaySlips(int userId, int companyId, DateTime dateTime)
        {
            ClientEmployeePaySlipsSM clientEmployeePaySlips = new ClientEmployeePaySlipsSM();
            var clientUserSm = await _clientUserProcess.GetClientUserById(userId);
            var clientUserAddressSm = await _clientUserAddressProcess.GetClientUserAddressByUserId(userId);
            var payrollTransaction = await GetPayrollTransactionsByUserIdBasedOnDateTime(userId, dateTime);
            var clientCompany = await _clientCompanyDetailProcess.GetClientCompanyDetailById(companyId);
            var clientCompanyAddress = await _clientCompanyAddressProcess.GetClientCompanyAddressById(companyId);
            var clientEmployeeCTCDetail = await _clientEmployeeCTCDetailProcess.GetClientEmployeeActiveCtcDetailByEmpId(userId);
            if (clientUserSm == null || payrollTransaction == null || clientCompany == null || clientEmployeeCTCDetail == null || clientCompanyAddress == null)
            {
                return null;
            }
            clientEmployeePaySlips = new ClientEmployeePaySlipsSM()
            {
                EmployeeName = clientUserSm?.LoginId,
                EmployeeCode = clientUserSm?.EmployeeCode,
                EmployeeEmail = clientUserSm?.PersonalEmailId,
                EmployeePhone = clientUserSm?.PhoneNumber,
                Designation = clientUserSm?.Designation,
                PaymentFor = payrollTransaction?.PaymentFor,
                PaymentAmount = (float)(payrollTransaction?.PaymentAmount),
                Name = clientCompany?.Name,
                CompanyMobileNumber = clientCompany?.CompanyMobileNumber,
                CompanyWebsite = clientCompany?.CompanyWebsite,
                CompanyContactEmail = clientCompany?.CompanyContactEmail,
                CompanyAddress = clientCompanyAddress?.Country + "-" + clientCompanyAddress?.City + "," + clientCompanyAddress?.State,
                CtcAmount = clientEmployeeCTCDetail.CtcAmount,
                ClientEmployeePayrollComponents = clientEmployeeCTCDetail.ClientEmployeePayrollComponents.ToList(),
                CompanyLogoPath = clientCompany.CompanyLogoPath == null ? clientCompany.CompanyLogoPath : Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(clientCompany.CompanyLogoPath)))
            };

            var currentDir = Environment.CurrentDirectory;
            string oldFilePath = Path.Combine(currentDir, @"wwwroot\Content\documents\payslip.html");

            string newFilePath = Path.Combine(currentDir, @"wwwroot\Content\documents\payslip_backup.html");

            System.IO.File.Copy(oldFilePath, newFilePath, true);
            var htmlContentNew = System.IO.File.ReadAllText(newFilePath);
            var htmlContentOld = System.IO.File.ReadAllText(oldFilePath);
            htmlContentNew = htmlContentNew.Replace("{{companyname}}", clientEmployeePaySlips.Name);
            htmlContentNew = htmlContentNew.Replace("{{companywebsite}}", clientEmployeePaySlips.CompanyWebsite);
            htmlContentNew = htmlContentNew.Replace("{{companymobile}}", clientEmployeePaySlips.CompanyMobileNumber);
            htmlContentNew = htmlContentNew.Replace("{{companyaddress}}", clientEmployeePaySlips.CompanyAddress);
            htmlContentNew = htmlContentNew.Replace("{{companymail}}", clientEmployeePaySlips.CompanyContactEmail);
            //htmlContentNew = htmlContentNew.Replace("{{paymentamount}}", clientEmployeePaySlips.PaymentAmount.ToString());
            htmlContentNew = htmlContentNew.Replace("{{ctcamount}}", clientEmployeePaySlips.CtcAmount.ToString());
            htmlContentNew = htmlContentNew.Replace("{{employeecode}}", clientEmployeePaySlips.EmployeeCode.ToString());
            htmlContentNew = htmlContentNew.Replace("{{name}}", clientEmployeePaySlips.EmployeeName);
            htmlContentNew = htmlContentNew.Replace("{{personalmail}}", clientEmployeePaySlips.EmployeeEmail);
            htmlContentNew = htmlContentNew.Replace("{{designation}}", clientEmployeePaySlips.Designation);
            htmlContentNew = htmlContentNew.Replace("{{doj}}", clientEmployeePaySlips.DateOfJoining.ToString());
            htmlContentNew = htmlContentNew.Replace("{{phonenumber}}", clientEmployeePaySlips.EmployeePhone);
            htmlContentNew = htmlContentNew.Replace("{{country}}", clientUserAddressSm.Country);
            htmlContentNew = htmlContentNew.Replace("{{state}}", clientUserAddressSm.State);
            htmlContentNew = htmlContentNew.Replace("{{city}}", clientUserAddressSm.City);
            htmlContentNew = htmlContentNew.Replace("{{address}}", clientUserAddressSm.Address1);
            decimal amt = 0;
            foreach (var payrollComponent in clientEmployeePaySlips.ClientEmployeePayrollComponents)
            {
                if (payrollComponent.Name == "Basic")
                {
                    htmlContentNew = htmlContentNew.Replace("{{basic}}", payrollComponent.AmountYearly.ToString());
                }
                else
                    if (payrollComponent.Name == "SA")
                {
                    htmlContentNew = htmlContentNew.Replace("{{sa}}", payrollComponent.AmountYearly.ToString());
                }
                else if (payrollComponent.Name == "HRA")
                {
                    htmlContentNew = htmlContentNew.Replace("{{hra}}", payrollComponent.AmountYearly.ToString());
                }
                else if (payrollComponent.Name == "CA")
                {
                    htmlContentNew = htmlContentNew.Replace("{{ca}}", payrollComponent.AmountYearly.ToString());
                }
                else if (payrollComponent.Name == "BONUS")
                {
                    htmlContentNew = htmlContentNew.Replace("{{bonus}}", payrollComponent.AmountYearly.ToString());
                }
                else if (payrollComponent.Name == "DA")
                {
                    htmlContentNew = htmlContentNew.Replace("{{da}}", payrollComponent.AmountYearly.ToString());
                }
                else if (payrollComponent.Name == "EPF")
                {
                    htmlContentNew = htmlContentNew.Replace("{{epf}}", payrollComponent.AmountYearly.ToString());
                    htmlContentNew = htmlContentNew.Replace("{{deductionamount}}", payrollComponent.AmountYearly.ToString());
                    var netAmount = ((clientEmployeePaySlips.CtcAmount - (float)payrollComponent.AmountYearly) / 12);
                    htmlContentNew = htmlContentNew.Replace("{{paymentamount}}", netAmount.ToString());
                }
            }
            using (StreamWriter writer = new StreamWriter(newFilePath))
            {
                writer.Write(htmlContentNew);
            }
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                Page = newFilePath,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);
            byte[] pdfBytes = file.ToArray();
            var documentNew = Convert.ToBase64String(pdfBytes);
            return documentNew;
        }

        #endregion --Generate PaySlips--

        #endregion CRUD

    }
}
