namespace LanAdmin_CommendRunner;

public class WindowsFeatures : CommendRunner
{
    public struct WindowsFeature
    {
        public string name;
        public bool isActive;
    }

    public static WindowsFeature[]? GetWindowsFeatures()
    {
        var process = StartPowerShell();
        if (process == null) return null;// ERR: cant start PS

        var commendResult = RunCommend(process, "Get-WindowsOptionalFeature -Online");
        if (CheckForError(commendResult) != 0) return null;// ERR: error in commend resualt

        List<WindowsFeature> output = new() { new WindowsFeature() };
        foreach (string i in commendResult)
        {
            if (i.StartsWith("FeatureName"))
            {
                output.Add(new());
                WindowsFeature tmp = output[^1];

                tmp.name = i.Split(':')[1].Trim();

                output[^1] = tmp;
                continue;
            }

            if (i.StartsWith("State"))
            {
                WindowsFeature tmp = output[^1];

                tmp.isActive = i.Split(':')[1].Trim() == "Enabled";

                output[^1] = tmp;
                continue;
            }
        }

        StopPowerShell(process);
        return output.ToArray();
    }
    // [0]->friendly name / [1]->discraption
    public static string[]? GetWindowsFeature_MoreInfo(WindowsFeature feature)
    {
        var process = StartPowerShell();
        if (process == null) return null;// ERR: cant start PS

        var commendResult = RunCommend(process, "Get-WindowsOptionalFeature -Online -FeatureName " + feature.name);
        if (CheckForError(commendResult) != 0) return null;// ERR: error in commend resualt

        string[] output = new[] { "", "" };
        foreach (string i in commendResult)
        {
            if (i.StartsWith("DisplayName"))
            {
                output[0] = i.Split(':')[1].Trim();
                continue;
            }

            if (i.StartsWith("Description"))
            {
                output[1] = i.Split(':')[1].Trim();
                continue;
            }
        }

        StopPowerShell(process);
        return output;
    }
}
