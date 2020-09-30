using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new Type[]
	{
		typeof(Pawn),
		typeof(IntVec3)
	})]
	internal class CostToMove
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
		public static void Postfix(ref int __result, Pawn pawn, IntVec3 c)
		{
			TerrainCost modExtension = c.GetTerrain(pawn.Map).GetModExtension<TerrainCost>();
			List<string> list = (modExtension != null) ? modExtension.tags : null;
			bool flag = list == null;
			bool flag2 = !flag;
			if (flag2)
			{
				CostToMove.tags.Clear();
				PawnTerrainHandler modExtension2 = pawn.def.GetModExtension<PawnTerrainHandler>();
				List<string> list2 = (modExtension2 != null) ? modExtension2.tags : null;
				bool flag3 = list2 != null;
				bool flag4 = flag3;
				if (flag4)
				{
					CostToMove.tags.AddRange(list2);
				}
				PawnKindDef kindDef = pawn.kindDef;
				bool flag5 = kindDef == null;
				List<string> list3;
				if (flag5)
				{
					list3 = null;
				}
				else
				{
					PawnTerrainHandler modExtension3 = kindDef.GetModExtension<PawnTerrainHandler>();
					list3 = ((modExtension3 != null) ? modExtension3.tags : null);
				}
				list2 = list3;
				bool flag6 = list2 != null;
				bool flag7 = flag6;
				if (flag7)
				{
					CostToMove.tags.AddRange(list2);
				}
				Faction faction = pawn.Faction;
				bool flag8 = faction == null;
				List<string> list4;
				if (flag8)
				{
					list4 = null;
				}
				else
				{
					FactionDef def = faction.def;
					bool flag9 = def == null;
					if (flag9)
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
				bool flag10 = list2 != null;
				bool flag11 = flag10;
				if (flag11)
				{
					CostToMove.tags.AddRange(list2);
				}
				bool flag12 = pawn == null;
				List<Apparel> list5;
				if (flag12)
				{
					list5 = null;
				}
				else
				{
					Pawn_ApparelTracker apparel = pawn.apparel;
					list5 = ((apparel != null) ? apparel.WornApparel : null);
				}
				List<Apparel> list6 = list5;
				bool flag13 = list6 != null;
				bool flag14 = flag13;
				if (flag14)
				{
					bool flag15 = pawn == null;
					List<Apparel> list7;
					if (flag15)
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
						bool flag16 = list2 != null;
						bool flag17 = flag16;
						if (flag17)
						{
							CostToMove.tags.AddRange(list2);
						}
					}
				}
				bool flag18 = CostToMove.tags.Count == 0;
				bool flag19 = !flag18;
				if (flag19)
				{
					foreach (string item in list)
					{
						bool flag20 = CostToMove.tags.Contains(item);
						bool flag21 = flag20;
						if (flag21)
						{
							__result -= modExtension.costToRefund;
							bool flag22 = __result < 1;
							bool flag23 = flag22;
							if (flag23)
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

		// Token: 0x04000005 RID: 5
		private static List<string> tags = new List<string>();
	}
}
