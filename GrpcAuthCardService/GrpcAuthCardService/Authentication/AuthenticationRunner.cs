using Microsoft.Identity.Client;
using MSCardAccessRequestService.Authentication;
using up_console;

namespace MSCardAccessRequestService.ServiceExtension
{
    public class AuthenticationRunner
    {
        public static async Task<AuthenticationStatuses> RunAsync(CredentialsRequest request)
        {
            SampleConfiguration config = SampleConfiguration.ReadFromJsonFile("appsettings.json");
            var appConfig = config.PublicClientApplicationOptions;
            var app = PublicClientApplicationBuilder.CreateWithApplicationOptions(appConfig)
                                                    .Build();
            var httpClient = new HttpClient();

            MyInformation myInformation = new MyInformation(app, httpClient, config.MicrosoftGraphBaseEndpoint);
            return await myInformation.DisplayMeAndMyManagerRetryingWhenWrongCredentialsAsync(request);
        }
    }
}
