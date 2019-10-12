using Infrastructure.Helpers;
using Infrastructure.Wrapper;

namespace Infrastructure.Interfaces
{
    public interface IConfigWrapper : IModelWrapper<Config>
    {
        string CandidatesURI { get; set; }
        string DisallowedIDsURI { get; set; }
        string SQLString { get; set; }
    }
}