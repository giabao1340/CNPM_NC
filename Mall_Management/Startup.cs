using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using System;

[assembly: OwinStartup(typeof(Mall_Management.Startup))]

namespace Mall_Management
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cấu hình xác thực Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });
        }
    }
}
