using System.Reflection;
using System.Runtime.Loader;
namespace LanAdmin;

/// <summary> every package must have one class inhrait this interface </summary>
public interface IPackage
{
    public string Name { get; }

    IGUI_responder GUI { get; }
    ICLI_responder CLI { get; }
}

/// <summary> this class uses to load packages and call interfaces functions </summary>
public static class PackageManager
{
    /// <summary> every time this changed PopulatePackageListFromFloder must called as well </summary>
    public static string packagesPath = @"C:\projectFiles\LanAdmin\Packages\PackagesFolder";

    /// <summary> all package interfaces saves hare </summary>
    public static Dictionary<string, IPackage> packages = new();
    /// <summary> load packages from packages folder </summary>
    public static void PopulatePackageListFromFloder()
    {
        packages.Clear();
        string[] folders = Directory.GetDirectories(packagesPath);

        foreach (string folderPath in folders)
        {
            string packageName = folderPath.Split('\\')[^1];
            packages.Add(packageName, LoadPackage($"{folderPath}\\LanAdminPackage_{packageName}.dll"));
        }
    }

    /// <summary> load package from dll path </summary>
    public static IPackage LoadPackage(string _path)
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
        throw new Exception("dll is not a LanAdmin package");
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
}
