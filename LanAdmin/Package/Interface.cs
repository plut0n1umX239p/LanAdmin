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
public static class PackageLoader
{
    /// <summary> every time this changed PopulatePackageListFromFloder must called as well </summary>
    public static string packagesFolderPath = @"C:\projectFiles\LanAdmin\Packages\PackagesFolder";

    /// <summary> all package interfaces saves hare </summary>
    public static Dictionary<string, IPackage> packages = new();

    ///  <summary> load packages from packages folder if name be null </summary>
    /// <param name="_blackList"> key: name - value[0]: load blacklist - value[1]: listener blacklist - value[2]: gui blacklist </param>
    public static void LoadPackages(string? _name, Dictionary<string, bool[]> _blackList)
    {
        if (_name != null)
        {
            if (_blackList.ContainsKey(_name)) if (_blackList[_name][0]) { return; }// load blacklist
            packages.Add(_name, LoadInterfaceFromDLL($"{packagesFolderPath}\\{_name}\\LanAdminPackage_{_name}.dll"));

            if (_blackList.ContainsKey(_name)) if (_blackList[_name][1]) { return; }// listener blacklist
            StartListener(_name, _blackList.ContainsKey(_name));// gui blacklist

            return;
        }

        packages.Clear();
        string[] folders = Directory.GetDirectories(packagesFolderPath);

        foreach (string folderPath in folders)
        {
            string packageName = folderPath.Split('\\')[^1];

            if (_blackList.ContainsKey(packageName)) if (_blackList[packageName][0]) { continue; }// load blacklist
            packages.Add(packageName, LoadInterfaceFromDLL($"{folderPath}\\LanAdminPackage_{packageName}.dll"));

            if (_blackList.ContainsKey(packageName)) if (_blackList[packageName][1]) { continue; }// listener blacklist
            StartListener(packageName, _blackList.ContainsKey(packageName));// gui blacklist
        }
    }
    ///  <summary> unload packages and if name be null then it will unload all </summary>
    public static void UnloadPackage(string? _name)
    {
        if (_name != null)
        {
            StopListener(_name);
            packages.Remove(_name);

            return;
        }

        StopListener(null);
        packages.Clear();
    }

    /// <summary> load package from dll path </summary>
    private static IPackage LoadInterfaceFromDLL(string _path)
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

    public static Dictionary<string, GUI_listener> GUI_listeners = new();
    public static Dictionary<string, CLI_listener> CLI_listeners = new();

    /// <summary> stop listeners on given package name. null means all </summary>
    public static void StopListener(string? _name)
    {
        if (_name == null)
        {
            foreach (GUI_listener i in GUI_listeners.Values) i.Stop();
            GUI_listeners.Clear();
            CLI_listeners.Clear();

            return;
        }

        if (GUI_listeners.ContainsKey(_name))
        {
            GUI_listeners[_name].Stop();
            GUI_listeners.Remove(_name);
            CLI_listeners.Remove(_name);
        }
    }
    /// <summary> start listeners on given package name. null means all </summary>
    public static void StartListener(string? _name, bool _ignoreGUI)
    {
        if (_name == null)
        {
            foreach (IPackage i in packages.Values)
            {
                if (!_ignoreGUI)
                {
                    GUI_listeners.Add(i.Name, new(i.GUI));
                    GUI_listeners[i.Name].Start();
                }
                CLI_listeners.Add(i.Name, new(i.CLI));
            }
            return;
        }

        if (GUI_listeners.ContainsKey(_name)) return;
        if (!_ignoreGUI)
        {
            GUI_listeners.Add(_name, new(packages[_name].GUI));
            GUI_listeners[_name].Start();
        }
        CLI_listeners.Add(_name, new(packages[_name].CLI));
    }

    /// <summary> return cli task from package if listener be active. </summary>
    public static Task? CLICommend(string _commend, string[] _args)
    {
        foreach (CLI_listener i in CLI_listeners.Values)
        {
            Task? task = i.Check(_commend, _args);
            if (task != null) return task;
        }

        return null;
    }
}
