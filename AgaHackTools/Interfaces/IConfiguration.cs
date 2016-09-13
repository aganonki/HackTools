namespace AgaHackTools.Main.Configurations
{
    public interface IConfiguration
    {
        bool IsConfigurationLoaded { get; }
        void Save();
        void Load();
        void SaveToFile(string file);
        void LoadFromFile(string file);
    }
}