namespace Infrastructure.Interfaces
{
    public interface IConfig
    {
        string CandidatesURI { get; set; }
        string DisallowedIDsURI { get; set; }
        string SQLString { get; set; }

        void SaveConfig();
        bool IsValid();
    }
}