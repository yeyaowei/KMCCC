namespace KMCCC.Simple
{
	#region

	using System;
	using System.IO;
	using System.Threading;
	using Authentication;
	using Launcher;

	#endregion

	internal class Program
	{
		private static FileStream _fs;

		private static TextWriter _tw;

		private static readonly AutoResetEvent Are = new AutoResetEvent(false);

		private static void Main()
		{
			using (_fs = new FileStream("mc.log", FileMode.Create))
			{
				using (_tw = new StreamWriter(_fs))
				{
					//这里图方便没有检验LauncherCoreCreationOption.Create()返回的是不是null
					var core = LauncherCore.Create();
                    Console.WriteLine("列出游戏版本：");
                    foreach(var version in core.GetVersions())
                    {
                        Console.WriteLine(version.Id);
                    }
                    Console.WriteLine("列出丢失lib：");
                    var ver = core.GetVersion("1.8.9");
                    foreach(var lib in ver.Libraries)
                    {
                        Console.WriteLine(lib.Name + " : " + core.GetLibPath(lib));
                    }
                    Console.WriteLine("列出丢失native：");
                    foreach (var native in ver.Natives)
                    {
                        Console.WriteLine(native.Name + " : " + core.GetNativePath(native));
                    }
                    Console.ReadKey();
                }
			}
		}

		private static void core_GameLog(LaunchHandle handle, string line)
		{
			Console.WriteLine(line);
			_tw.WriteLine(line);

			handle.SetTitle("啦啦啦");
		}

		private static void core_GameExit(LaunchHandle handle, int code)
		{
			Are.Set();
		}
	}
}