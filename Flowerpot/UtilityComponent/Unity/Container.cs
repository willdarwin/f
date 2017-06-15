using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace GeneralUtilities.Utilities.Unity
{
    /// <summary>
    /// This class handles the configuration of the Dependeny injection functionality using Unity application block from EntLib
    /// </summary>
    public sealed class Container
    {
        private static readonly IUnityContainer container = new UnityContainer().LoadConfiguration();

        private Container()
        {
            //Configure Unity
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container);
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static IUnityContainer UnityContainer
        {
            get
            {
                return container;
            }
        }
    }
}
