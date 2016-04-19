using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Globalization;

namespace Lightcore.Server.SitecoreHttp.Api.Actions
{
    public class Parameters
    {
        private readonly IEnumerable<Database> _databases;
        private readonly string _databaseName;
        private readonly string _deviceName;
        private readonly string _languageName;
        private readonly string _cdn;
        private readonly IEnumerable<string> _itemFields;
        private readonly IEnumerable<string> _childFields;

        public Parameters(HttpRequestBase request, IEnumerable<Database> databases)
        {
            _databases = databases;
            _databaseName = request.QueryString["sc_database"];
            _deviceName = request.QueryString["sc_device"];
            _languageName = request.QueryString["sc_lang"];
            _cdn = request.QueryString["cdn"];
            _itemFields = request.QueryString["itemfields"] != null ? request.QueryString["itemfields"].Split(',') : Enumerable.Empty<string>();
            _childFields = request.QueryString["childfields"] != null ? request.QueryString["childfields"].Split(',') : Enumerable.Empty<string>();
        }

        public string Cdn
        {
            get { return _cdn; }
        }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(_databaseName))
            {
                Database = _databases.FirstOrDefault(x => x.Name.Equals(_databaseName, StringComparison.OrdinalIgnoreCase));

                if (Database == null)
                {
                    return false;
                }
            }
            else
            {
                Database = _databases.First(x => x.Name.Equals("web", StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(_languageName))
            {
                Language = Database.Languages.FirstOrDefault(x => x.Name.Equals(_languageName, StringComparison.OrdinalIgnoreCase));

                if (Language == null)
                {
                    return false;
                }
            }
            else
            {
                Language = LanguageManager.DefaultLanguage;
            }

            if (!string.IsNullOrEmpty(_deviceName))
            {
                var device = Database.Items.GetItem(ItemIDs.DevicesRoot).Children[_deviceName];

                if (device == null)
                {
                    return false;
                }

                DeviceName = _deviceName;
            }
            else
            {
                DeviceName = "default";
            }

            return true;
        }

        public string DeviceName { get; private set; }
        public Database Database { get; private set; }
        public Language Language { get; private set; }

        public IEnumerable<string> ItemFields
        {
            get { return _itemFields; }
        }

        public IEnumerable<string> ChildFields
        {
            get { return _childFields; }
        }
    }
}