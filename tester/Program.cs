foreach (var item in LanAdmin_CommendRunner.WindowsFeatures.GetWindowsFeatures())
{
    var s = LanAdmin_CommendRunner.WindowsFeatures.GetWindowsFeature_MoreInfo(item);
    Console.WriteLine($"{s[0]}/{s[1]}");
}
