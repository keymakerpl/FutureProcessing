using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Wrapper;

namespace FutureProcessing.Wrapper
{
    public class ConfigWrapper : ModelWrapper<Config>, IConfigWrapper
    {
        public ConfigWrapper(Config model) : base(model)
        {
        }

        private string _candidatesURI;
        public string CandidatesURI
        {
            get { return GetValue<string>(); }
            set { SetProperty(ref _candidatesURI, value); }
        }

        private string _disallowedIDsURI;
        public string DisallowedIDsURI
        {
            get { return GetValue<string>(); }
            set { SetProperty(ref _disallowedIDsURI, value); }
        }

        private string _sQLString;
        public string SQLString
        {
            get { return GetValue<string>(); }
            set { SetProperty(ref _sQLString, value); }
        }
    }
}
