using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.General;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.General;

namespace Siffrum.Web.Payroll.BAL.General
{
    public class ContactUsProcess : SiffrumPayrollBalOdataBase<ContactUsSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;

        #endregion --Properties--

        #region --Constructor--

        public ContactUsProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<ContactUsSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.ContactUs;
            IQueryable<ContactUsSM> retSM = await MapEntityAsToQuerable<ContactUsDM, ContactUsSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Count--

        /// <summary>
        /// Get ContactUs Count in database.
        /// </summary>
        /// <returns>integer response</returns>

        public async Task<int> GetAllContactUsCountResponse()
        {
            int resp = _apiDbContext.ContactUs.Count();
            return resp;
        }

        #endregion --Count--

        #region Get All
        /// <summary>
        /// This methods gets all Contact Us (List of ContactUs)
        /// </summary>
        /// <returns>All Contact Us</returns>
        public async Task<List<ContactUsSM>> GetAllContactUs()
        {
            var contactUsDM = await _apiDbContext.ContactUs.ToListAsync();
            var contactUsSM = _mapper.Map<List<ContactUsSM>>(contactUsDM);
            return contactUsSM;
        }
        #endregion Get All

        #region Get Single
        /// <summary>
        /// This method gets a single contact us on id
        /// </summary>
        /// <param name="id">Contact Us Id</param>
        /// <returns>Single Contact Us</returns>
        public async Task<ContactUsSM?> GetSingleContactUsById(int id)
        {
            ContactUsDM? contactDM = await _apiDbContext.ContactUs.FindAsync(id);

            if (contactDM != null)
            {
                return _mapper.Map<ContactUsSM>(contactDM);
            }
            else
            {
                return null;
            }
        }
        #endregion Get Single

        #region Post
        /// <summary>
        /// This method add a new ContactUs into the database
        /// </summary>
        /// <param name="contactUsSM">ContactUs To Save</param>
        /// <returns>Newly Added ContactUs</returns>
        public async Task<ContactUsSM?> AddContactUs(ContactUsSM contactUsSM)
        {
            var contactUsDM = _mapper.Map<ContactUsDM>(contactUsSM);
            contactUsDM.CreatedBy = _loginUserDetail.LoginId;
            contactUsDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.ContactUs.AddAsync(contactUsDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<ContactUsSM>(contactUsDM);
            }
            else
            {
                return null;
            }
        }
        #endregion Post

        #region Delete
        /// <summary>
        /// This method deletes any existing ContactUs on Id
        /// </summary>
        /// <param name="id">ContactUs Id to delete</param>
        /// <returns>Status of deletion, true if deleted successfully otherwise false with a message.</returns>
        public async Task<DeleteResponseRoot> DeleteContactUsById(int id)
        {
            var isPresent = await _apiDbContext.ContactUs.AnyAsync(x => x.Id == id);

            if (isPresent)
            {
                var contactUsToDelete = new ContactUsDM() { Id = id };
                _apiDbContext.ContactUs.Remove(contactUsToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Contact Us details deleted successfully.");
                }
            }
            return new DeleteResponseRoot(false, "Contact Us Details not found");

        }

        #endregion Delete
    }
}
