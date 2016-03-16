using System;
using AgaHackTools.Main.Helpers;

namespace AgaHackTools.Main.Configurations
{
    public abstract class XmlConfiguration<T> : IConfiguration
    {
        public event EventHandler<T> ConfigurationChanged; 
        protected XmlConfiguration(string defaultFilePath)
        {
            DefaultFilePath = defaultFilePath;
        }

        public T CurrentConfiguration { get; set; }

        public string DefaultFilePath { get; }
        public bool IsConfigurationLoaded { get; set; }

        public virtual void Save()
        {
            SaveToFile(DefaultFilePath);
        }

        public virtual void Load()
        {
            var newSettings = XmlHelper.ImportFromXmlFile<T>(DefaultFilePath);
            CurrentConfiguration = newSettings;
            IsConfigurationLoaded = true;
            OnConfigurationChanged(CurrentConfiguration);
        }

        public virtual void SaveToFile(string file)
        {
            XmlHelper.ExportToXmlFile(CurrentConfiguration, file);
        }

        public virtual void LoadFromFile(string file)
        {
            var newSettings = XmlHelper.ImportFromXmlFile<T>(file);
            CurrentConfiguration = newSettings;
            IsConfigurationLoaded = true;
            OnConfigurationChanged(CurrentConfiguration);
        }

        protected virtual void OnConfigurationChanged(T e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}