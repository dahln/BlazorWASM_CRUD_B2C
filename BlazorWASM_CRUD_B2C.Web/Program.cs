using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using BlazorSpinner;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorWASM_CRUD_B2C.Web.Services;

namespace BlazorWASM_CRUD_B2C.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("BlazorWASM_CRUD_B2CAPI", client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("API_URL")))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { builder.Configuration.GetValue<string>("API_URL") }, //<--- The URI used by the Server project.
                            scopes: new[] { builder.Configuration.GetValue<string>("Scope") });
                    return handler;
                });

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorWASM_CRUD_B2CAPI"));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetValue<string>("Scope"));

                options.ProviderOptions.LoginMode = "redirect";
            });

            builder.Services.AddScoped<API>();
            builder.Services.AddScoped<SpinnerService>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
