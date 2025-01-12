using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using CO;
//using ColossalFramework.Globalization;
//using ColossalFramework.Packaging;
using LoadOrderTool;

public class BuildConfig
{
	//public static string applicationVersion
	//{
	//	get
	//	{
	//		return BuildConfig.VersionToString(172090642u, false);
	//	}
	//}

	//public static string applicationVersionFull
	//{
	//	get
	//	{
	//		return BuildConfig.VersionToString(172090642u, true);
	//	}
	//}

	public static uint MakeVersionNumber(uint a, uint b, uint c, BuildConfig.ReleaseType releaseType, uint buildNumber, BuildConfig.BuildType buildType)
	{
		return (uint)((a << 27) + (b << 22) + (c << 17) + ((uint)releaseType << 13) + (buildNumber << 8) + (uint)buildType);
	}

	public static bool SupportsVersion(uint version)
	{
		return 172090624u >= (version & 4294967040u);
	}

	//public static string VersionToString(uint version, bool full)
	//{
	//	if (version < 2000000u)
	//	{
	//		string text = string.Empty;
	//		uint num = version % 100u;
	//		if (num != 0u)
	//		{
	//			text = ((char)(97u + num)).ToString();
	//		}
	//		return StringUtils.SafeFormat("{0}.{1}.{2}{3}", new object[]
	//		{
	//			version / 1000000u,
	//			version / 10000u % 100u,
	//			version / 100u % 100u,
	//			text
	//		});
	//	}
	//	uint num2 = version >> 27 & 31u;
	//	uint num3 = version >> 22 & 31u;
	//	uint num4 = version >> 17 & 31u;
	//	BuildConfig.ReleaseType releaseType = (BuildConfig.ReleaseType)(version >> 13 & 15u);
	//	uint num5 = version >> 8 & 31u;
	//	BuildConfig.BuildType buildType = (BuildConfig.BuildType)(version & 255u);
	//	string text2 = string.Empty;
	//	if (releaseType != BuildConfig.ReleaseType.Prototype)
	//	{
	//		if (releaseType != BuildConfig.ReleaseType.Alpha)
	//		{
	//			if (releaseType != BuildConfig.ReleaseType.Beta)
	//			{
	//				if (releaseType == BuildConfig.ReleaseType.Final)
	//				{
	//					text2 = "-f";
	//				}
	//			}
	//			else
	//			{
	//				text2 = "-beta";
	//			}
	//		}
	//		else
	//		{
	//			text2 = "-alpha";
	//		}
	//	}
	//	else
	//	{
	//		text2 = "-proto";
	//	}
	//	string text3 = string.Empty;
	//	if (full)
	//	{
	//		switch (buildType)
	//		{
	//		case BuildConfig.BuildType.SteamWin:
	//			text3 = "-steam-win";
	//			break;
	//		case BuildConfig.BuildType.SteamOSX:
	//			text3 = "-steam-osx";
	//			break;
	//		case BuildConfig.BuildType.SteamLinux:
	//			text3 = "-steam-linux";
	//			break;
	//		default:
	//			if (buildType != BuildConfig.BuildType.EditorWin)
	//			{
	//				if (buildType == BuildConfig.BuildType.EditorOSX)
	//				{
	//					text3 = "-editor-osx";
	//				}
	//			}
	//			else
	//			{
	//				text3 = "-editor-win";
	//			}
	//			break;
	//		}
	//	}
	//	return StringUtils.SafeFormat("{0}.{1}.{2}{3}{4}{5}", new object[]
	//	{
	//		num2,
	//		num3,
	//		num4,
	//		text2,
	//		num5,
	//		text3
	//	});
	//}

	internal static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		Log.Info("CurrentDomain_AssemblyResolve() called");
		var name0 = args.Name;
		if (kIgnoreAssemblies.Contains(name0))
			return null;

		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly ret = null;
		foreach(var asm in assemblies) { 
			var name = asm.GetName().Name;
			if (name0.StartsWith(name))
			{
				// get latest assembly
				if (ret == null || ret.GetName().Version < asm.GetName().Version)
					ret = asm;
			}
		}
        if (ret != null)
            Log.Info($"Assembly '{name0}' resolved to '{ret}'");
        else {
            if (name0 == "Mono.Runtime") {
                Log.Info($"[harmless] Assembly resolution failure. No assembly named '{name0}' was found.");
            } else {
                Log.Error($"Assembly resolution failure. No assembly named '{name0}' was found.");
            }
        }
		return ret;
	}

	//public static Type ResolveLegacyType(string type)
	//{
	//	if (type == "Locale")
	//	{
	//		return typeof(Locale);
	//	}
	//	if (type == "RoadManager+Data")
	//	{
	//		return typeof(NetManager.Data);
	//	}
	//	if (type == "GameMaterialManager+Data")
	//	{
	//		return typeof(TransferManager.Data);
	//	}
	//	if (type.Contains("AutomaticMilestone+AutomaticData"))
	//	{
	//		return typeof(CombinedMilestone.CombinedData);
	//	}
	//	if (type.Contains("WindManager+Data"))
	//	{
	//		return typeof(WeatherManager.Data);
	//	}
	//	if (type.Contains("Manager+Data"))
	//	{
	//		type += ", Assembly-CSharp";
	//	}
	//	return Type.GetType(type);
	//}

	//public static string ResolveLegacyPrefab(string name)
	//{
	//	if (name == "Basic Road2")
	//	{
	//		return "Basic Road Decoration Trees";
	//	}
	//	return BuildConfig.ResolveCustomAssetName(name);
	//}

	//private static string ResolveCustomAssetName(string name)
	//{
	//	foreach (Package.Asset current in PackageManager.FilterAssets(new Package.AssetType[]
	//	{
	//		Package.AssetType.Object
	//	}))
	//	{
	//		if (current.get_isEnabled() && current.get_name() == name)
	//		{
	//			return current.get_package().get_packageName() + "." + name;
	//		}
	//	}
	//	return name;
	//}

	public const uint APPLICATION_VERSION_A = 1u;

	public const uint APPLICATION_VERSION_B = 9u;

	public const uint APPLICATION_VERSION_C = 0u;

	public const BuildConfig.ReleaseType APPLICATION_RELEASE_TYPE = BuildConfig.ReleaseType.Final;

	public const uint APPLICATION_BUILD_NUMBER = 5u;

	public const uint APPLICATION_VERSION = 172090642u;

	public const uint DATA_FORMAT_VERSION = 109011u;

	internal static string[] kIgnoreAssemblies = new string[]
	{
		"2.5.29.31",
		"2.5.29.32",
		"2.5.29.35",
		"2.5.29.17",
		"1.3.6.1.5.5.7.1.1",
		"MoneyPanel"
	};

	public enum ReleaseType : uint
	{
		Prototype = 1u,
		Alpha = 5u,
		Beta = 10u,
		Final = 15u
	}

	public enum BuildType : uint
	{
		Unknown,
		SteamWin = 16u,
		SteamOSX,
		SteamLinux,
		EditorWin = 240u,
		EditorOSX
	}
}
