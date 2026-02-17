using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class ClientCompanyAttendanceShiftProcess : SiffrumPayrollBalOdataBase<ClientCompanyAttendanceShiftSM>
    {
        #region --Properties--
        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ClientCompanyAttendanceShiftProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ClientCompanyAttendanceShiftSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ClientCompanyAttendanceShifts;
            IQueryable<ClientCompanyAttendanceShiftSM> retSM = await MapEntityAsToQuerable<ClientCompanyAttendanceShiftDM, ClientCompanyAttendanceShiftSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All ClientCompanyAttendanceShift details in database
        /// </summary>
        /// <returns>Service Model of List of ClientCompanyAttendanceShift in database</returns>
        public async Task<List<ClientCompanyAttendanceShiftSM>> GetAllClientCompanyAttendanceShift()
        {
            var dm = await _apiDbContext.ClientCompanyAttendanceShifts.ToListAsync();
            var sm = _mapper.Map<List<ClientCompanyAttendanceShiftSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get ClientCompanyAttendanceShift Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientCompanyAttendanceShift</param>
        /// <returns>Service Model of List of ClientCompanyAttendanceShift in database</returns>

        public async Task<ClientCompanyAttendanceShiftSM> GetClientCompanyAttendanceShiftById(int id)
        {
            var clientCompanyAttendanceShiftDM = await _apiDbContext.ClientCompanyAttendanceShifts.FindAsync(id);
            if (clientCompanyAttendanceShiftDM != null)
            {
                return _mapper.Map<ClientCompanyAttendanceShiftSM>(clientCompanyAttendanceShiftDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Get--

        #region --My-EndPoints--

        /// <summary>
        /// Get ClientCompanyAttendanceShift Details by Id
        /// </summary>
        /// <param name="id">Primary Key of ClientCompanyAttendanceShift</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of List of ClientCompanyAttendanceShift in database</returns>

        public async Task<List<ClientCompanyAttendanceShiftSM>> GetMyClientCompanyAttendanceShiftById(int currentCompanyId)
        {
            var clientCompanyAttendanceShiftDM = await _apiDbContext.ClientCompanyAttendanceShifts.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            if (clientCompanyAttendanceShiftDM != null)
            {
                return _mapper.Map<List<ClientCompanyAttendanceShiftSM>>(clientCompanyAttendanceShiftDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --My-EndPoints

        #region --Add/Update--

        /// <summary>
        /// Add new ClientCompanyAttendanceShift
        /// </summary>
        /// <param name="clientCompanyAttendanceShiftSM">ClientCompanyAttendanceShift object</param>
        /// <returns> Service Model of List of ClientCompanyAttendanceShift in database</returns>

        public async Task<ClientCompanyAttendanceShiftSM> AddClientCompanyAttendanceShift(ClientCompanyAttendanceShiftSM clientCompanyAttendanceShiftSM)
        {
            var clientCompanyAttendanceShiftDM = _mapper.Map<ClientCompanyAttendanceShiftDM>(clientCompanyAttendanceShiftSM);
            clientCompanyAttendanceShiftDM.CreatedBy = _loginUserDetail.LoginId;
            clientCompanyAttendanceShiftDM.CreatedOnUTC = DateTime.UtcNow;
            DateTime timeOnlyFrom = new DateTime(1, 1, 1, clientCompanyAttendanceShiftSM.ShiftFrom.Hour, clientCompanyAttendanceShiftSM.ShiftFrom.Minute, clientCompanyAttendanceShiftSM.ShiftFrom.Second);
            DateTime timeOnlyTo = new DateTime(1, 1, 1, clientCompanyAttendanceShiftSM.ShiftTo.Hour, clientCompanyAttendanceShiftSM.ShiftTo.Minute, clientCompanyAttendanceShiftSM.ShiftTo.Second);
            clientCompanyAttendanceShiftDM.ShiftFrom = timeOnlyFrom;
            clientCompanyAttendanceShiftDM.ShiftTo = timeOnlyTo;
            await _apiDbContext.ClientCompanyAttendanceShifts.AddAsync(clientCompanyAttendanceShiftDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ClientCompanyAttendanceShiftSM>(clientCompanyAttendanceShiftDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update ClientCompanyAttendanceShift of added record
        /// </summary>
        /// <param name="objIdToUpdate">Id to update</param>
        /// <param name="clientCompanyAttendanceShiftSM">ClientCompanyAttendanceShift object to update</param>
        /// <returns>Service Model of List of ClientCompanyAttendanceShift in database</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<ClientCompanyAttendanceShiftSM> UpdateClientCompanyAttendanceShift(int objIdToUpdate, ClientCompanyAttendanceShiftSM clientCompanyAttendanceShiftSM)
        {
            if (clientCompanyAttendanceShiftSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.ClientCompanyAttendanceShifts.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    clientCompanyAttendanceShiftSM.Id = objIdToUpdate;

                    ClientCompanyAttendanceShiftDM dbDM = await _apiDbContext.ClientCompanyAttendanceShifts.FindAsync(objIdToUpdate);
                    _mapper.Map(clientCompanyAttendanceShiftSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;
                    DateTime timeOnlyFrom = new DateTime(1, 1, 1, clientCompanyAttendanceShiftSM.ShiftFrom.Hour, clientCompanyAttendanceShiftSM.ShiftFrom.Minute, clientCompanyAttendanceShiftSM.ShiftFrom.Second);
                    DateTime timeOnlyTo = new DateTime(1, 1, 1, clientCompanyAttendanceShiftSM.ShiftTo.Hour, clientCompanyAttendanceShiftSM.ShiftTo.Minute, clientCompanyAttendanceShiftSM.ShiftTo.Second);
                    dbDM.ShiftFrom = timeOnlyFrom;
                    dbDM.ShiftTo = timeOnlyTo;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<ClientCompanyAttendanceShiftSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"ClientCompanyAttendanceShift not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion --Add/Update--

        #region --Delete--

        /// <summary>
        /// Delete ClientCompanyAttendanceShift by  Id
        /// </summary>
        /// <param name="id">primary key of ClientCompanyAttendanceShift</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteClientCompanyAttendanceShiftById(int id)
        {
            var isPresent = await _apiDbContext.ClientCompanyAttendanceShifts.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyAttendanceShiftDM() { Id = id };
                _apiDbContext.ClientCompanyAttendanceShifts.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Company Attendance-Shift  Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --Delete--

        #region --My Delete EndPoint--

        /// <summary>
        /// Delete ClientCompanyAttendanceShift by  Id
        /// </summary>
        /// <param name="id">primary key of ClientCompanyAttendanceShift</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteMyClientCompanyAttendanceShiftById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.ClientCompanyAttendanceShifts.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new ClientCompanyAttendanceShiftDM() { Id = id };
                _apiDbContext.ClientCompanyAttendanceShifts.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Company Attendance-Shift  Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion --My Delete EndPoint--

        #region --Mine Attendance-Shift--

        public async Task<ClientCompanyAttendanceShiftSM> GetMineClientCompanyAttendanceShiftById(int id)
        {
            //var clientUserDM = await _apiDbContext.ClientUsers.Where(x => x.Id == id && x.ClientCompanyAttendanceShiftId != null).FirstOrDefaultAsync();
            var result = await (
                                 from clientUser in _apiDbContext.ClientUsers
                                 join attendanceShift in _apiDbContext.ClientCompanyAttendanceShifts
                                     on clientUser.ClientCompanyAttendanceShiftId equals attendanceShift.Id
                                 where clientUser.Id == id
                                 select new
                                 {
                                     AttendanceShift = attendanceShift
                                 }
                                ).FirstOrDefaultAsync();
            var clientCompanyAttendanceShiftDM = result?.AttendanceShift;
            if (clientCompanyAttendanceShiftDM != null)
            {
                return _mapper.Map<ClientCompanyAttendanceShiftSM>(clientCompanyAttendanceShiftDM);
            }
            else
            {
                return null;
            }
        }

        #endregion --Mine Attendance-Shift--

    }
}
