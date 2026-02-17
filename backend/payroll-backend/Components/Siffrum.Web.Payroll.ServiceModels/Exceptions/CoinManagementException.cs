
namespace Siffrum.Web.Payroll.ServiceModels.Exceptions
{
    public class SiffrumPayrollException : ApiExceptionRoot
    {
        public SiffrumPayrollException(ApiErrorTypeSM exceptionType, string devMessage,
           string displayMessage = "", Exception innerException = null)
            : base(exceptionType, devMessage, displayMessage, innerException)
        { }
    }
}
