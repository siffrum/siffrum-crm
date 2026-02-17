using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1;

namespace Siffrum.Web.Payroll.BAL.Dummy
{
    public partial class DummyTeacherProcess : SiffrumPayrollBalOdataBase<DummyTeacherSM>
    {
        private readonly ILoginUserDetail _loginUserDetail;

        public DummyTeacherProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext) : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #region Odata
        public override async Task<IQueryable<DummyTeacherSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.DummyTeachers;
            IQueryable<DummyTeacherSM> retSM = await MapEntityAsToQuerable<DummyTeacherDM, DummyTeacherSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD 

        #region Get
        public async Task<List<DummyTeacherSM>> GetAllDummyTeachers()
        {
            var dm = await _apiDbContext.DummyTeachers.ToListAsync();
            var sm = _mapper.Map<List<DummyTeacherSM>>(dm);
            return sm;
        }
        public async Task<DummyTeacherSM> GetDummyTeacherById(int id)
        {
            DummyTeacherDM DummyTeacherDM = await _apiDbContext.DummyTeachers.FindAsync(id);

            if (DummyTeacherDM != null)
            {
                return _mapper.Map<DummyTeacherSM>(DummyTeacherDM);
            }
            else
            {
                return null;
            }
        }

        #endregion Get

        #region Add Update
        public async Task<DummyTeacherSM> AddDummyTeacher(DummyTeacherSM DummyTeacherSM)
        {
            var objDM = _mapper.Map<DummyTeacherDM>(DummyTeacherSM);
            objDM.CreatedBy = _loginUserDetail.LoginId;
            objDM.CreatedOnUTC = DateTime.UtcNow;
            await _apiDbContext.DummyTeachers.AddAsync(objDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<DummyTeacherSM>(objDM);
            }
            else
            {
                return null;
            }
        }
        public async Task<DummyTeacherSM> UpdateDummyTeacher(int objIdToUpdate, DummyTeacherSM DummyTeacherSM)
        {
            if (DummyTeacherSM != null && objIdToUpdate > 0)
            {
                DummyTeacherDM objDM = await _apiDbContext.DummyTeachers.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    DummyTeacherSM.Id = objIdToUpdate;
                    _mapper.Map<DummyTeacherSM, DummyTeacherDM>(DummyTeacherSM, objDM);

                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<DummyTeacherSM>(objDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"DummySubject not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion Add Update

        #region Delete
        public async Task<DeleteResponseRoot> DeleteDummyTeacherById(int id)
        {
            var isPresent = await _apiDbContext.DummyTeachers.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummyTeachers  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new DummyTeacherDM() { Id = id };
                _apiDbContext.DummyTeachers.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true);
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion Delete

        #endregion CRUD

        #region Private Functions
        #endregion Private Functions

    }
}
