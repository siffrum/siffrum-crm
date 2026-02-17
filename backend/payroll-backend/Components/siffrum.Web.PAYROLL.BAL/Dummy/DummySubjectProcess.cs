using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1;

namespace Siffrum.Web.Payroll.BAL.Dummy
{
    public partial class DummySubjectProcess : SiffrumPayrollBalOdataBase<DummySubjectSM>
    {
        private readonly ILoginUserDetail _loginUserDetail;

        public DummySubjectProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #region Odata
        public override async Task<IQueryable<DummySubjectSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.DummySubjects;
            IQueryable<DummySubjectSM> retSM = await MapEntityAsToQuerable<DummySubjectDM, DummySubjectSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region CRUD 

        #region Get
        public async Task<List<DummySubjectSM>> GetAllDummySubjects()
        {
            var dm = await _apiDbContext.DummySubjects.ToListAsync();
            var sm = _mapper.Map<List<DummySubjectSM>>(dm);
            return sm;
        }
        public async Task<DummySubjectSM> GetDummySubjectById(int id)
        {
            DummySubjectDM dummySubjectDM = await _apiDbContext.DummySubjects.FindAsync(id);

            if (dummySubjectDM != null)
            {
                return _mapper.Map<DummySubjectSM>(dummySubjectDM);
            }
            else
            {
                return null;
            }
        }

        #endregion Get

        #region Add Update
        public async Task<DummySubjectSM> AddDummySubject(DummySubjectSM dummySubjectSM)
        {
            var objDM = _mapper.Map<DummySubjectDM>(dummySubjectSM);
            objDM.CreatedBy = _loginUserDetail.LoginId;
            objDM.CreatedOnUTC = DateTime.UtcNow;
            await _apiDbContext.DummySubjects.AddAsync(objDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<DummySubjectSM>(objDM);
            }
            else
            {
                return null;
            }
        }
        public async Task<DummySubjectSM> UpdateDummySubject(int objIdToUpdate, DummySubjectSM dummySubjectSM)
        {
            if (dummySubjectSM != null && objIdToUpdate > 0)
            {
                DummySubjectDM objDM = await _apiDbContext.DummySubjects.FindAsync(objIdToUpdate);
                if (objDM != null)
                {
                    dummySubjectSM.Id = objIdToUpdate;
                    _mapper.Map(dummySubjectSM, objDM);

                    objDM.LastModifiedBy = _loginUserDetail.LoginId;
                    objDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<DummySubjectSM>(objDM);
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
        public async Task<DeleteResponseRoot> DeleteDummySubjectById(int id)
        {
            var isPresent = await _apiDbContext.DummySubjects.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new DummySubjectDM() { Id = id };
                _apiDbContext.DummySubjects.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Subject with Id " + id + " deleted successfully!");
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
