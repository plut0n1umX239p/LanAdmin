using System.Reflection;
using System.Runtime.Loader;

namespace LanAdmin;

public interface IPackage
{
    public string Name { get; }

    IGUI_responder GUI { get; }
    ICLI_responder CLI { get; }
}

public static class PackageManager
{
    public static List<string> packagesPaths = new() { @"C:\projectFiles\LanAdmin\Packages\LanAdminPackage_Tester\bin\Debug\net6.0\LanAdminPackage_Template.dll" };

    public static List<IPackage> packageList = new();
    public static void PopulatePackageList()
    {
        packageList.Clear();

        packageList = packagesPaths.SelectMany(_packagesPath =>
        { return new List<IPackage>() { LoadPackage(_packagesPath) }; }).ToList();
    }

    #region Load package context
#pragma warning disable CS8600
#pragma warning disable CS8603
    private class PackageLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver resolver;
        public PackageLoadContext(string _pluginPath)
        {
            resolver = new AssemblyDependencyResolver(_pluginPath);
        }
        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
#pragma warning restore CS8600
#pragma warning restore CS8603
    #endregion
    private static IPackage LoadPackage(string _path)
    {
        // get assembly with path
        Assembly packageAssembly = new PackageLoadContext(_path).LoadFromAssemblyName(
            new(Path.GetFileNameWithoutExtension(_path)));

        // find IPackage interface
        Type[] typesInPackage = packageAssembly.GetTypes();
        foreach (Type typeInPackage in typesInPackage)
            if (typeof(IPackage).IsAssignableFrom(typeInPackage))
            {
                IPackage? extarctedInterface = (IPackage?)Activator.CreateInstance(typeInPackage);
                if (extarctedInterface != null) return extarctedInterface;
            }

        // cant find any IPackage class
        throw new Exception("dll is not a package");
    }
}
