using System.Collections.Generic;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    [HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", typeof(Pawn), typeof(IntVec3))]
    internal class CostToMove
    {
        private static readonly List<string> tags = new List<string>();

        public static void Postfix(ref int __result, Pawn pawn, IntVec3 c)
        {
            var modExtension = c.GetTerrain(pawn.Map).GetModExtension<TerrainCost>();
            if (modExtension?.tags == null)
            {
                return;
            }

            tags.Clear();
            var modExtension2 = pawn.def.GetModExtension<PawnTerrainHandler>();
            var list2 = modExtension2?.tags;
            if (list2 != null)
            {
                tags.AddRange(list2);
            }

            var kindDef = pawn.kindDef;
            List<string> list3;
            if (kindDef == null)
            {
                list3 = null;
            }
            else
            {
                var modExtension3 = kindDef.GetModExtension<PawnTerrainHandler>();
                list3 = modExtension3?.tags;
            }

            list2 = list3;
            if (list2 != null)
            {
                tags.AddRange(list2);
            }

            var faction = pawn.Faction;
            List<string> list4;
            if (faction == null)
            {
                list4 = null;
            }
            else
            {
                var def = faction.def;
                if (def == null)
                {
                    list4 = null;
                }
                else
                {
                    var modExtension4 = def.GetModExtension<PawnTerrainHandler>();
                    list4 = modExtension4?.tags;
                }
            }

            list2 = list4;
            if (list2 != null)
            {
                tags.AddRange(list2);
            }

            var apparel = pawn.apparel;
            var list5 = apparel?.WornApparel;

            if (list5 != null)
            {
                var apparel2 = pawn.apparel;
                var list7 = apparel2?.WornApparel;

                if (list7 != null)
                {
                    foreach (var apparel3 in list7)
                    {
                        var modExtension5 = apparel3.def.GetModExtension<PawnTerrainHandler>();
                        list2 = modExtension5?.tags;
                        if (list2 != null)
                        {
                            tags.AddRange(list2);
                        }
                    }
                }
            }

            if (tags.Count == 0)
            {
                return;
            }

            foreach (var item in modExtension.tags)
            {
                if (!tags.Contains(item))
                {
                    continue;
                }

                __result -= modExtension.costToRefund;
                if (__result < 1)
                {
                    __result = 1;
                }

                return;
            }

            __result += modExtension.costToAdd;
        }
    }
}
