using Auth0.AspNetCore.Authentication;

namespace ZMDB.BackHost.Configurations
{
    public static class Auth0Configurations
    {
        public static WebApplicationBuilder ConfigureAuth0(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            builder.Services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"];
                options.ClientId = builder.Configuration["Auth0:ClientId"];
            });

            builder.Services.AddControllersWithViews();

            return builder;
        }
    }
}
