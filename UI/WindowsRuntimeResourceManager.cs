using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Resources;
using System.Resources;

namespace Particle.UI
{
    public class WindowsRuntimeResourceManager : ResourceManager
    {
        // copied from https://blogs.msdn.microsoft.com/philliphoff/2014/11/19/missingmanifestresourceexception-when-using-portable-class-libraries-within-winrt/#sthash.J4SUaiqL.dpuf

        private readonly ResourceLoader resourceLoader;

        private WindowsRuntimeResourceManager(string baseName, Assembly assembly) : base(baseName, assembly)
        {
            this.resourceLoader = ResourceLoader.GetForViewIndependentUse(baseName);
        }

        public static void InjectIntoResxGeneratedApplicationResourcesClass(Type resxGeneratedApplicationResourcesClass)
        {
            resxGeneratedApplicationResourcesClass.GetRuntimeFields()
              .First(m => m.Name == "resourceMan")
              .SetValue(null, new WindowsRuntimeResourceManager(resxGeneratedApplicationResourcesClass.FullName, resxGeneratedApplicationResourcesClass.GetTypeInfo().Assembly));
        }

        public override string GetString(string name, CultureInfo culture)
        {
            return this.resourceLoader.GetString(name);
        }
    }
}
