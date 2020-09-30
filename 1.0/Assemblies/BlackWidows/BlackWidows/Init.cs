using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace BlackWidows
{
    [StaticConstructorOnStartup]
    internal static class Init
    {
        static Init()
        {
            Harmony harmonyInstance = new Harmony("zamnath.BlackWidows");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
