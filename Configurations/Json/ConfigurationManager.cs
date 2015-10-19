using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Main.Interfaces;
using Newtonsoft.Json;

namespace Json
{
    public class ConfigurationManager : IConfigurationManager
    {
        public void DefaultConfig<T>(string configName, T objToWrite)
        {
            if (File.Exists(configName)) return;
            JsonConfig.WriteFile(objToWrite, configName);
        }

        public T ReadConfiguration<T>(string configName) where T : new()
        {
            return JsonConfig.ReadFile<T>(configName);
        }

        public void SaveConfiguration<T>(string configFileName, T configs)
        {
            JsonConfig.WriteFile(configs, configFileName);
        }


    }

    public static class ConfigurationManagerStatic
    {
        public static void DefaultConfig<T>(string configName, T objToWrite)
        {
            if (File.Exists(configName)) return;
            JsonConfig.WriteFile(objToWrite, configName);
        }

        public static T GetConfiguration<T>(string configName) where T : new()
        {
            return JsonConfig.ReadFile<T>(configName);
        }
        public static void RewriteConfiguration<T>(T configs, string configFileName)
        {
            JsonConfig.WriteFile(configs, configFileName);
        }
    }

    public static class JsonConfig
    {
        public static T ReadFile<T>(string filename) where T : new()
        {
            string data;
            try
            {
                using (
                    var sr = new StreamReader(File.Open(filename,
                            FileMode.Open, FileAccess.Read)))
                {
                    data = sr.ReadToEnd();
                }
                Console.WriteLine(data);
                var temp = JsonConvert.DeserializeObject<T>(data);
                return temp;
            }
            catch (Exception e)
            {
                data = null;
                return new T();
            }
        }
        public static void WriteFile<T>(T objToWrite, string fileName)
        {
            var temp = JsonConvert.SerializeObject(objToWrite, Formatting.Indented);
            Console.WriteLine(temp);
            using (var file = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(temp);
                }
            }
        }
    }
}
