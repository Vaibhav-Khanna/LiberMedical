using System;
using System.Collections.Generic;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace libermedical.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get { return CrossSettings.Current; }
		}

       

        #region Setting Constants

	    private static readonly string stringDefault = string.Empty;
	    private static readonly int intDefault = 0;
	    public static readonly bool booleanDefault = false;

	    const bool NeedsSyncDefault = true;

        const string LastSyncKey = "last_sync";
        static readonly DateTime LastSyncDefault = DateTime.UtcNow.AddDays(-30);

	    private const string IsLoggedInKey = "IsLoggedIn";
	    private const string TokenKey = "Token";
	    private const string TokenExpirationKey = "TokenExpiration";
	    private const string UserKey = "UserKey";
        private const string AdvisorPhone = "AdvisorPhone";
        private const string AdvisorMail = "AdvisorMail";

        #endregion

        public static bool IsLoggedIn
	    {
	        get { return AppSettings.GetValueOrDefault(IsLoggedInKey, booleanDefault); }
	        set { AppSettings.AddOrUpdateValue(IsLoggedInKey, value); }
	    }

	    public static string Token
	    {
	        get { return AppSettings.GetValueOrDefault(TokenKey, stringDefault); }
	        set { AppSettings.AddOrUpdateValue(TokenKey, value); }
	    }

	    public static int TokenExpiration
	    {
	        get { return AppSettings.GetValueOrDefault(TokenExpirationKey, intDefault); }
	        set { AppSettings.AddOrUpdateValue(TokenExpirationKey, value); }
	    }

	    public static string CurrentUser
	    {
	        get { return AppSettings.GetValueOrDefault(UserKey, stringDefault); }
	        set { AppSettings.AddOrUpdateValue(UserKey, value); }
	    }

        public static string AdvisorContact
        {
            get { return AppSettings.GetValueOrDefault(AdvisorPhone, stringDefault); }
            set { AppSettings.AddOrUpdateValue(AdvisorPhone, value); }
        }

        public static string AdvisorEmail
        {
            get { return AppSettings.GetValueOrDefault(AdvisorMail, stringDefault); }
            set { AppSettings.AddOrUpdateValue(AdvisorMail, value); }
        }
              
    }
}
