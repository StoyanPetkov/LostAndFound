﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using LF.Models;
using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;

namespace LF
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "5eab00ca-63bd-495e-bb08-a352ff8babfe",
            //    clientSecret: "mihHHH2zRFHiTeZfi7PURoe");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = "402893966546204",
                AppSecret = "a820ff454227672c3195b75d984a3714",
                Scope = { "email", "public_profile" },//, "first_name","last_name","location", "picture" 
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        foreach (var claim in context.User)
                        {
                            var claimType = string.Format("urn:facebook:{0}", claim.Key);
                            string claimValue = claim.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                                context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                        }
                        return Task.FromResult(true);
                    }
                },
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie
            });

            var googleOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "839729734397-2uma1s2ui6dic8m0a8ev8prvapeldjpe.apps.googleusercontent.com",
                ClientSecret = "CNB9ARiLLkY4LxbLuOhp38fu",

            };
            googleOptions.Scope.Add("https://www.googleapis.com/auth/plus.profile.emails.read");
            app.UseGoogleAuthentication(googleOptions);

        }
    }
}