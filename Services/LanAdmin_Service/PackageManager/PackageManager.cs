namespace LanAdmin.PackageManager;
using LanAdmin.CommendRunner;

public static class PackageManager
{
    public struct RemotePackage
    {
        LocalPackage localData;// this datas can stores locally

        public string zipURL;// url of zip/rar installer
    }

    public struct LocalPackage
    {
        public enum PackageType { windowsFeature, globalPackage, localPackage }
        public enum LoadMode { avalibleRemote, avalibleLocal, installed }

        public string name;// name of package
        public List<string> dependencys;// which pakages this package need
        public Version version;// version of this package

        public PackageType type;// type of package
        public LoadMode mode;
        public string repo;// where to find info about this package

        public string displayName;// name of package gone shows in UI
        public string creator;// name of developer of this package.
        public string description;// a text about whats this package does.
        public List<string> notes;// a set of information and warning about this package.
    }

    #region windows feature

    public static List<LocalPackage> windowsFeature = new();

    public static void EnableFreature(LocalPackage windowsFreature)
    {
        var process = CommendRunner.StartPowerShell();

        windowsFeature.Clear();
        CommendRunner.RunCommend(process, "Enable-WindowsOptionalFeature -Online -NoRestart -All -FeatureName " + windowsFreature.name);

        CommendRunner.StopPowerShell(process);
    }// just remember the reset!
    public static void DisableFreature(LocalPackage windowsFreature)
    {
        var process = CommendRunner.StartPowerShell();
        if (process == null) throw new Exception("cant start powershell");

        windowsFeature.Clear();
        CommendRunner.RunCommend(process, "Disable-WindowsOptionalFeature -Online -NoRestart -FeatureName " + windowsFreature.name);

        CommendRunner.StopPowerShell(process);
    }// just remember the reset!
    public static void GetFreatures()
    {
        var process = CommendRunner.StartPowerShell();

        windowsFeature.Clear();
        var commendResult = CommendRunner.RunCommend(process, "Get-WindowsOptionalFeature -Online -FeatureName **");
        CommendRunner.CheckForError(commendResult);

        foreach (string i in commendResult)
        {
            if (i.StartsWith("FeatureName"))
            {
                windowsFeature.Add(new()
                {
                    name = i.Split(':')[1].Trim(),
                    type = LocalPackage.PackageType.windowsFeature
                });
                continue;
            }

            if (i.StartsWith("DisplayName"))
            {
                string tmp = i.Split(':')[1].Trim();

                var tmp0 = windowsFeature[^1];
                tmp0.displayName = tmp == "" ? windowsFeature[^1].name : tmp;
                windowsFeature[^1] = tmp0;

                continue;
            }

            // simple description
            if (i.StartsWith("Description"))
            {
                var tmp = windowsFeature[^1];
                tmp.description = i.Split(':')[1].Trim();
                windowsFeature[^1] = tmp;

                continue;
            }
            if (i.StartsWith("ServerComponent\\Description"))
            {
                var tmp = windowsFeature[^1];
                tmp.description = i.Split(':')[1].Trim();
                windowsFeature[^1] = tmp;

                continue;
            }

            if (i.StartsWith("State"))
            {
                var tmp = windowsFeature[^1];
                tmp.mode = i.Split(':')[1].Trim() == "Enabled" ? LocalPackage.LoadMode.installed : LocalPackage.LoadMode.avalibleLocal;
                windowsFeature[^1] = tmp;

                continue;
            }
        }

        CommendRunner.StopPowerShell(process);
    }

    #endregion
}
