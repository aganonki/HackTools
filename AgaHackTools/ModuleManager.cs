using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Main
{
    public class ModuleManager : IModuleManager
    {

        public readonly ILog Logger = Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public  Type GetInstance(string fileName, string @interface)
        {
            try
            {
                var file = fileName.EndsWith(".dll") ? fileName : fileName + ".dll";
                if(!File.Exists(file))
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,file);
                /* Load in the assembly. */
                Logger.Info("Loading: " + file);
                var moduleAssembly = Assembly.LoadFile(file);

                /* Get the types of classes that are in this assembly. */
                var types = moduleAssembly.GetTypes();
                /* Loop through the types in the assembly until we find
                 * a class that implements a Module.
                 */
               
                var result = types.First(item => item.GetInterface(@interface) != null);

                return result;

            }
            catch (FileLoadException e)
            {
                Logger.Error("Module load assembly error " + e);
                return null;
            }
            catch (FileNotFoundException e)
            {
                Logger.Error("Module load assembly error " + e);
                return null;
            }
        }

        public T ActivateInstance<T>(Type instance)
        {

            return ActivateInstance<T>(instance,null);
        }
        public T ActivateInstance<T>(Type instance, params object[] args)
        {
            T result;
            try
            {
               result =  (T)Activator.CreateInstance(instance, args);
            }
            catch (Exception e)
            {

                Logger.Fatal(e);
                result = default(T);
            }
            return result;
        }


    }
}
