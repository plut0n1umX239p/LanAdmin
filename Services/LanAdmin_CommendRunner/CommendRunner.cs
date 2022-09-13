using System.Diagnostics;
namespace LanAdmin_CommendRunner;

public class CommendRunner
{
    public static Process? StartPowerShell()
    {
        Process process = new();

        //create a cmd proccess
        try
        {
            process.StartInfo.FileName = "PowerShell.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            process.StandardInput.WriteLine("echo \"init-ok\"");
            while (process.StandardOutput.ReadLine() != "init-ok") ;

            return process;
        }
        catch
        {
            try { process.Close(); } catch { }
            return null;
        }
    }
    public static string[] RunCommend(Process process, string commend)
    {
        process.StandardInput.WriteLine(commend);
        process.StandardInput.WriteLine("echo \"commend-ok\"");

        List<string> output = new();
        while (true)
        {
            string? input = process.StandardOutput.ReadLine();
            if(input == null) continue;

            if(input.StartsWith("PS")) continue;
            if(input == "commend-ok") return output.ToArray();

            output.Add(input);
        }
    }
    public static int CheckForError(string[] commendResualt)
    {
        foreach (string i in commendResualt)
        {
            if (i.EndsWith("The requested operation requires elevation.")) return -1;// ERR: permision error
        }

        return 0;// OK: end of checks
    }
    public static void StopPowerShell(Process process)
    {
        process.Kill();
    }
}
