using System.Reflection;
using HarmonyLib;
using Verse;

namespace BlackWidows
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    internal static class Init
    {
        // Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
        static Init()
        {
            var harmony = new Harmony("zamnath.BlackWidows");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}