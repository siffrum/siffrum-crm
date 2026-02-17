using Siffrum.Web.Payroll.Client.Constants;

namespace Siffrum.Web.Payroll.Client.Base
{
    public class SiffrumPayrollApiClientBase : ApiClientRoot
    {
        public SiffrumPayrollApiClientBase(AccessingClientDetails accessingClinetDetails, Func<Exception, bool>? onExceptionInClient = null) : base(accessingClinetDetails, onExceptionInClient)
        {
        }

        public async Task<ApiResponse<List<T>>> GetServiceModelByOdata<T>(AuthClientWrapper authClientWrapper, OdataQueryFilter odataFilter, CancellationToken cancelToken)
            where T : class
        {
            var reqQueryFilterWithOdata = base.AddOdataQueryFiltersToQuery(odataFilter, $"{ApiUrls.DUMMY_SUBJECT_URL}/odata");
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            ApiResponse<List<T>> response = await base.GetResponseEntityAsync<string, List<T>>
                (reqQueryFilterWithOdata, HttpMethod.Get, null,
                cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return response;
        }

        public async Task<ApiResponse<List<T>>> GetServiceModelByOdata<T>(AuthClientWrapper authClientWrapper, string odataQuery, CancellationToken cancelToken)
            where T : class
        {
            var reqQueryFilterWithOdata = base.GetEndpointURL($"{ApiUrls.DUMMY_SUBJECT_URL}/odata", odataQuery);
            IDictionary<string, string> headers = base.CheckAuthInputsAndFormHeaders(authClientWrapper);
            ApiResponse<List<T>> response = await base.GetResponseEntityAsync<string, List<T>>
                (reqQueryFilterWithOdata, HttpMethod.Get, null,
                cancelToken, headers, true, authClientWrapper?.AuthDetails);

            return response;
        }
    }
}
