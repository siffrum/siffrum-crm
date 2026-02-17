using Siffrum.Web.Payroll.DAL.Contexts;

namespace Siffrum.Web.Payroll.BAL.Base
{
    public abstract class SiffrumPayrollBalBase : BalRoot
    {
        protected readonly IMapper _mapper;
        protected readonly ApiDbContext _apiDbContext;

        public SiffrumPayrollBalBase(IMapper mapper, ApiDbContext apiDbContext)
        {
            _mapper = mapper;
            _apiDbContext = apiDbContext;
        }
    }
    public abstract class SiffrumPayrollBalOdataBase<T> : BalOdataRoot<T>
    {
        protected readonly IMapper _mapper;
        protected readonly ApiDbContext _apiDbContext;

        protected SiffrumPayrollBalOdataBase(IMapper mapper, ApiDbContext apiDbContext)
        {
            _mapper = mapper;
            _apiDbContext = apiDbContext;
        }
    }
}
