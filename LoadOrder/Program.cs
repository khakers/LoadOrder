namespace LoadOrderTool {
    using CO.IO;
    using CO.Plugins;
    using LoadOrderTool.Util;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Threading;
    using System.Diagnostics;

    static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Console.WriteLine("Hello!");

                AppDomain.CurrentDomain.TypeResolve += BuildConfig.CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve += BuildConfig.CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.AssemblyResolve += ResolveInterface;
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
                Application.ThreadException += UnhandledThreadExceptionHandler;

                Application.Run(new UI.LoadOrderWindow());
            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }


        static void LoadManagedDLLs() {
            var dlls = Directory.GetFiles(DataLocation.ManagedDLL, "*.dll");
            foreach (var dll in dlls) {
                try {
                    var asm = AssemblyUtil.LoadDLL(dll);
                    Log.Info($"Assembly loaded: {asm}");
                } catch (Exception ex) {
                    Log.Exception(new Exception($"the dll {dll} failed to load", ex));
                }
            }
        }

        private static void UnhandledThreadExceptionHandler(object sender, ThreadExceptionEventArgs args) {
            Exception ex = (Exception)args.Exception;
            Log.Exception(ex, "Unhandled Exception Occured.");
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
            Exception ex = (Exception)args.ExceptionObject;
            Log.Exception(ex, "Unhandled Exception Occured.");
        }

        private static Assembly ResolveInterface(object sender, ResolveEventArgs args) {
            Log.Info("Resolving Assembly " + args.Name);
            string file = Path.Combine(
                DataLocation.DataPath,
                "Managed",
                new AssemblyName(args.Name).Name + ".dll"); // parse name
            if (!File.Exists(file))
                return null;
            return AssemblyUtil.LoadDLL(file);
        }
    }
}
