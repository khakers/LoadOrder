﻿using Mono.Cecil;
using System.Runtime.CompilerServices;
using ILogger = Patch.API.ILogger;
using System;
using System.Diagnostics;
using System.Linq;

namespace LoadOrderIPatch {
    internal static class Commons {
        internal const string InjectionsDLL = InjectionsAssemblyName + ".dll";
        internal const string InjectionsAssemblyName = "LoadOrderInjections";
        internal static AssemblyDefinition GetInjectionsAssemblyDefinition(string dir)
            => CecilUtil.GetAssemblyDefinition(dir, InjectionsDLL);

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void LogSucessfull(this ILogger logger)
        {
            string caller = new StackFrame(1).GetMethod().Name;
            logger.Info($"[LoadOrderIPatch] Sucessfully applied {caller}!");
                //+ "\n------------------------------------------------------------------------");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void LogStartPatching(this ILogger logger)
        {
            string caller = new StackFrame(1).GetMethod().Name;
            logger.Info($"[LoadOrderIPatch] {caller} started ...");
        }

        public static bool HasArg(string arg) =>
            Environment.GetCommandLineArgs().Any(_arg => _arg == arg);
        public static bool breadthFirst = HasArg("-phased");
        public static bool poke = HasArg("-poke");
    }
}
