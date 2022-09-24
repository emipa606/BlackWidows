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
            var harmony = new Harmony("zamnath.BlackWidows");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
