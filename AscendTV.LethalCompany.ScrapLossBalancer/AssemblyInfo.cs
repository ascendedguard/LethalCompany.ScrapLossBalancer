using System.Reflection;
using AscendTV.LethalCompany.ScrapLossBalancer;

// AssemblyInfo is manually generated so we can store the version
// number in a const and reuse it for Harmony and BepInEx registration.
// Plus only one place to remember to change it.

[assembly: AssemblyCompany("AscendTV.LethalCompany.ScrapLossBalancer")]
[assembly: AssemblyProduct("AscendTV.LethalCompany.ScrapLossBalancer")]
[assembly: AssemblyTitle("AscendTV.LethalCompany.ScrapLossBalancer")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyFileVersion(PluginInfo.Version)]
[assembly: AssemblyInformationalVersion(PluginInfo.ShortVersion)]
[assembly: AssemblyVersion(PluginInfo.Version)]
