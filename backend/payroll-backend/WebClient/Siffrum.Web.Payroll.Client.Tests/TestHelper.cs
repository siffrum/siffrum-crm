using Siffrum.Web.Payroll.ServiceModels.Enums;
using System.Reflection;

namespace Siffrum.Web.Payroll.Client.Tests
{

    public class TestHelper
    {

        public const string COMP_CODE = "123";
        public const string USERNAME = "clientadmin1";
        public const string PASSWORD = "pass123";
        //public const string APIBASE_URL = "http://local-boilerplate.renosoftwares.com/";// every dev shud map this 
        public const string APIBASE_URL = "http://localhost:11888/";
        public const string CLIENT_NAME = "test";
        public const RoleTypeSM USERTYPE = RoleTypeSM.ClientAdmin;


        public static AccessingClientDetails GetAccessingClientDetails()
        {
            AccessingClientDetails accessingClinetDetails = new AccessingClientDetails(GetCurrentClientAssemblyVersion())
            {
                AccessingClinetDetail = "TODO:PUT DETAIL HERE",
                ApiBaseUrl = APIBASE_URL,
                RequestTimeoutMs = 1000000
            };
            return accessingClinetDetails;
        }
        public static AuthDetails GetAuthClientDetails()
        {
            return new AuthDetails()
            {
                CompanyCode = COMP_CODE,
                ApiUserType = USERTYPE.ToString(),
                LoginId = USERNAME,
                Password = PASSWORD
            };
        }

        public static Func<Exception, bool> GetExceptionFunc()
        {

            Func<Exception, bool> onExceptionInClient = (exp) =>
            {
                return false;
            };
            return onExceptionInClient;
        }

        public static T RunFunctionAsAsyncTask<T>(Func<CancellationTokenSource, Task<T>> funcToRun)
        {
            var apiTimeout = 50000;// 50sec
            var cancellationTokenSource = new CancellationTokenSource(apiTimeout);

            var taskT = Task.Run(async () =>
            {
                var retVal = await funcToRun(cancellationTokenSource);
                return retVal;
            });
            return taskT.ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private static string GetCurrentClientAssemblyVersion() =>
            typeof(TestHelper).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
    }
}
