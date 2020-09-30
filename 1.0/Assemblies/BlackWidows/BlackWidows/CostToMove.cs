using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    [HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new Type[]
    {
        typeof(Pawn),
        typeof(IntVec3)
    })]
    internal class CostToMove
    {
        public static void Postfix(ref int __result, Pawn pawn, IntVec3 c)
        {
            TerrainCost modExtension = c.GetTerrain(pawn.Map).GetModExtension<TerrainCost>();
            List<string> list = (modExtension != null) ? modExtension.tags : null;
            bool flag = list == null;
            if (!flag)
            {
                CostToMove.tags.Clear();
                PawnTerrainHandler modExtension2 = pawn.def.GetModExtension<PawnTerrainHandler>();
                List<string> list2 = (modExtension2 != null) ? modExtension2.tags : null;
                bool flag2 = list2 != null;
                if (flag2)
                {
                    CostToMove.tags.AddRange(list2);
                }
                PawnKindDef kindDef = pawn.kindDef;
                List<string> list3;
                if (kindDef == null)
                {
                    list3 = null;
                }
                else
                {
                    PawnTerrainHandler modExtension3 = kindDef.GetModExtension<PawnTerrainHandler>();
                    list3 = ((modExtension3 != null) ? modExtension3.tags : null);
                }
                list2 = list3;
                bool flag3 = list2 != null;
                if (flag3)
                {
                    CostToMove.tags.AddRange(list2);
                }
                Faction faction = pawn.Faction;
                List<string> list4;
                if (faction == null)
                {
                    list4 = null;
                }
                else
                {
                    FactionDef def = faction.def;
                    if (def == null)
                    {
                        list4 = null;
                    }
                    else
                    {
                        PawnTerrainHandler modExtension4 = def.GetModExtension<PawnTerrainHandler>();
                        list4 = ((modExtension4 != null) ? modExtension4.tags : null);
                    }
                }
                list2 = list4;
                bool flag4 = list2 != null;
                if (flag4)
                {
                    CostToMove.tags.AddRange(list2);
                }
                List<Apparel> list5;
                if (pawn == null)
                {
                    list5 = null;
                }
                else
                {
                    Pawn_ApparelTracker apparel = pawn.apparel;
                    list5 = ((apparel != null) ? apparel.WornApparel : null);
                }
                List<Apparel> list6 = list5;
                bool flag5 = list6 != null;
                if (flag5)
                {
                    List<Apparel> list7;
                    if (pawn == null)
                    {
                        list7 = null;
                    }
                    else
                    {
                        Pawn_ApparelTracker apparel2 = pawn.apparel;
                        list7 = ((apparel2 != null) ? apparel2.WornApparel : null);
                    }
                    foreach (Apparel apparel3 in list7)
                    {
                        PawnTerrainHandler modExtension5 = apparel3.def.GetModExtension<PawnTerrainHandler>();
                        list2 = ((modExtension5 != null) ? modExtension5.tags : null);
                        bool flag6 = list2 != null;
                        if (flag6)
                        {
                            CostToMove.tags.AddRange(list2);
                        }
                    }
                }
                bool flag7 = CostToMove.tags.Count == 0;
                if (!flag7)
                {
                    foreach (string item in list)
                    {
                        bool flag8 = CostToMove.tags.Contains(item);
                        if (flag8)
                        {
                            __result -= modExtension.costToRefund;
                            bool flag9 = __result < 1;
                            if (flag9)
                            {
                                __result = 1;
                            }
                            return;
                        }
                    }
                    __result += modExtension.costToAdd;
                }
            }
        }

        private static List<string> tags = new List<string>();
    }
}
