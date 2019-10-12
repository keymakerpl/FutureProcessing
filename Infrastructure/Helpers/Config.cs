using Infrastructure.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Infrastructure.Helpers
{
    public class Config : IConfig
    {
        public Config()
        {
            Initailize();
        }

        public string CandidatesURI { get; set; }

        public string DisallowedIDsURI { get; set; }

        public string SQLString { get; set; }

        public bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(CandidatesURI)) return false;
            if (String.IsNullOrWhiteSpace(DisallowedIDsURI)) return false;
            if (String.IsNullOrWhiteSpace(SQLString)) return false;

            return true;
        }

        public void SaveConfig()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

                var customSettings = ConfigurationManager.GetSection("customAppSettingsGroup/customAppSettings")
                as NameValueCollection;

                if (customSettings != null)
                {
                    customSettings["CandidatesURI"] = CandidatesURI;
                    customSettings["DisallowedIDsURI"] = DisallowedIDsURI;
                }

                connectionStringsSection.ConnectionStrings["FPDb"].ConnectionString = SQLString;

                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }
        }

        private void Initailize()
        {
            try
            {
                var sql = ConfigurationManager.ConnectionStrings["FPDb"].ConnectionString;
                SQLString = sql;

                var settings =
                ConfigurationManager.GetSection("customAppSettingsGroup/customAppSettings")
                as NameValueCollection;

                if (settings != null)
                {
                    var candidatesURI = settings["CandidatesURI"];
                    CandidatesURI = candidatesURI;

                    var disallowedIDsURI = settings["DisallowedIDsURI"];
                    DisallowedIDsURI = disallowedIDsURI;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }
        }
    }
}