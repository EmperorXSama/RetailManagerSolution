using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using active_directory_b2c_wpf;
using Microsoft.Identity.Client;

namespace RMWPFUserInterfice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Azure Link Information 

        private static readonly string TenantName = "RetailManage";
        private static readonly string Tenant = $"{TenantName}.onmicrosoft.com";
        private static readonly string AzureAdB2CHostname = $"{TenantName}.b2clogin.com";
        private static readonly string ClientId = "a985b751-a830-4700-8f0f-c3b2fc1a73fe";
        private static readonly string RedirectUri = $"http://localhost";

        public static string PolicySignUpSignIn = "B2C_1_si";
        private static string PolicyEditProfile = "B2C_1_edit_profile";
        private static string PolicyResetPassword = "B2C_1_reset_password";

        public static readonly string[] ApiScopes = {           
            " https://RetailManage.onmicrosoft.com/RetailManagerApi/data.view",
            "https://RetailManage.onmicrosoft.com/RetailManagerApi/data.write"
            
        };
        public static string ApiEndpoint = "https://localhost:7145/";
        
        
        // Shouldn't need to change these:
        private static readonly string AuthorityBase = $"https://{AzureAdB2CHostname}/tfp/{Tenant}/";
        private static readonly string AuthoritySignUpSignIn = $"{AuthorityBase}{PolicySignUpSignIn}";
        public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        public static readonly string AuthorityResetPassword = $"{AuthorityBase}{PolicyResetPassword}";
        
        public static IPublicClientApplication PublicClientApp { get; private set; }


        #endregion
        
        static App()
        {
            PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithB2CAuthority(AuthoritySignUpSignIn)
                .WithRedirectUri(RedirectUri)
                .WithLogging(Log, LogLevel.Info, false) // don't log P(ersonally) I(dentifiable) I(nformation) details on a regular basis
                .Build();

            TokenCacheHelper.Bind(PublicClientApp.UserTokenCache);
        }

        private static void Log(LogLevel level, string message, bool containsPii)
        {
            string logs = $"{level} {message}{Environment.NewLine}";
            File.AppendAllText(System.Reflection.Assembly.GetExecutingAssembly().Location + ".msalLogs.txt", logs);
        }
    }
}