using Microsoft.Extensions.DependencyInjection;
using MessageApp.Infrastructure.TwilioSMS;
using MessageApp.Application.Twilio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MessageApp.Domain.Entities.Concrete;
using MessageApp.Presistence.Data.Contexts;

namespace MessageApp.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddScoped<ISenderSMS, Sender>();
            //services.AddMemoryCache();
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //});
            //services.AddDatabaseDeveloperPageExceptionFilter();
            //services.AddIdentityCore<ApplicationUser>(options =>
            //{
            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            //    options.User.RequireUniqueEmail = true;
            //}).AddEntityFrameworkStores<MessageAppDbContext>();
        }
    }
}
