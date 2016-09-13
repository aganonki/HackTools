namespace AgaHackTools.Main.Interfaces
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// If config file does not exists creates configuration with default values.
        /// </summary>
        /// <typeparam name="T">Configuration object type</typeparam>
        /// <param name="configName">Name or path to configuration file</param>
        /// <param name="objToWrite">Configuration object</param>
        void SaveDefault<T>(string configName, T objToWrite);
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <typeparam name="T">Configuration object type</typeparam>
        /// <param name="configName">Name or path to configuration file</param>
        /// <returns>Configuration object with values from file</returns>
        T Read<T>(string configName) where T : new();
        /// <summary>
        /// Save configuration to file
        /// </summary>
        /// <typeparam name="T">Configuration object type</typeparam>
        /// <param name="configFileName">Name or path to configuration file</param>
        /// <param name="configs">Configuration object</param>
        void Save<T>(string configFileName, T configs);
        string ToString<T>(T configs);
        T FromString<T>(string configs) where T : new();
    }
}
