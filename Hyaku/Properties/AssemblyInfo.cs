using System.Reflection;
using MelonLoader;
[assembly: AssemblyTitle(Hyaku.BuildInfo.Description)]
[assembly: AssemblyDescription(Hyaku.BuildInfo.Description)]
[assembly: AssemblyCompany(Hyaku.BuildInfo.Company)]
[assembly: AssemblyProduct(Hyaku.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + Hyaku.BuildInfo.Author)]
[assembly: AssemblyTrademark(Hyaku.BuildInfo.Company)]
[assembly: AssemblyVersion(Hyaku.BuildInfo.Version)]
[assembly: AssemblyFileVersion(Hyaku.BuildInfo.Version)]
[assembly: MelonInfo(typeof(Hyaku.Hyaku), Hyaku.BuildInfo.Name, Hyaku.BuildInfo.Version, Hyaku.BuildInfo.Author, Hyaku.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

[assembly: MelonGame("Pixelatto", "Reventure")]