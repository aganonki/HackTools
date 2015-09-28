using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Default;
using AgaHackTools.Interfaces;

namespace AgaHackTools
{
    public class ModuleManager : IModuleManager
    {

        public readonly ILog Logger = Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public  Type GetInstance(string fileName, string @interface)
        {
            try
            {
                /* Load in the assembly. */
                var moduleAssembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + fileName + ".dll");

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

        public T ActivateInstance<T>(Type instance) where T : new()
        {

            return ActivateInstance<T>(instance,null);
        }
        public T ActivateInstance<T>(Type instance, params object[] args) where T : new()
        {
            T result;
            try
            {
               result =  (T)Activator.CreateInstance(instance, args);
            }
            catch (Exception e)
            {

                Logger.Fatal(e);
                result = new T();
            }
            return result;
        }


    }
}
