using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
	// Token: 0x02000006 RID: 6
	public class WorkGiver_MilkHumanoidWidow : WorkGiver_Scanner
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002388 File Offset: 0x00000588
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023A1 File Offset: 0x000005A1
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				yield return pawns[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023B8 File Offset: 0x000005B8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool flag = pawn2 == null;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				Pawn pawn3 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
				bool flag3 = pawn3 == null;
				bool flag4 = flag3;
				if (flag4)
				{
					result = false;
				}
				else
				{
					bool flag5 = pawn.Faction != pawn2.Faction;
					bool flag6 = flag5;
					if (flag6)
					{
						result = false;
					}
					else
					{
						bool flag7 = pawn3 != pawn2;
						bool flag8 = flag7;
						if (flag8)
						{
							result = false;
						}
						else
						{
							bool drafted = pawn3.Drafted;
							bool flag9 = drafted;
							if (flag9)
							{
								result = false;
							}
							else
							{
								bool flag10 = pawn3.CurJob != null && pawn3.jobs.curDriver.asleep;
								bool flag11 = flag10;
								if (flag11)
								{
									result = false;
								}
								else
								{
									bool flag12 = pawn3.CurJob != null && pawn3.CurJob.playerForced;
									bool flag13 = flag12;
									if (flag13)
									{
										result = false;
									}
									else
									{
										CompWidowMilkableHumanoid compWidowMilkableHumanoid = pawn3.TryGetComp<CompWidowMilkableHumanoid>();
										bool flag14 = compWidowMilkableHumanoid == null;
										bool flag15 = flag14;
										if (flag15)
										{
											result = false;
										}
										else
										{
											bool flag16 = !compWidowMilkableHumanoid.ActiveAndCanBeMilked;
											bool flag17 = flag16;
											if (flag17)
											{
												result = false;
											}
											else
											{
												bool flag18 = pawn3.CurJob != null && compWidowMilkableHumanoid.MilkProps.forbiddenJobsToInterrupt.Count > 0 && compWidowMilkableHumanoid.MilkProps.forbiddenJobsToInterrupt.Contains(pawn3.CurJob.def);
												bool flag19 = flag18;
												if (flag19)
												{
													result = false;
												}
												else
												{
													bool flag20 = pawn3.Position.IsForbidden(pawn);
													bool flag21 = flag20;
													if (flag21)
													{
														result = false;
													}
													else
													{
														bool flag22 = !pawn.CanReserve(new LocalTargetInfo(pawn3), 1, -1, null, false);
														bool flag23 = flag22;
														if (flag23)
														{
															result = false;
														}
														else
														{
															bool flag24 = !pawn.CanReach(new LocalTargetInfo(pawn3), PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn);
															result = !flag24;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025BC File Offset: 0x000007BC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(WidowDefOf.WidowMilkyLover, new LocalTargetInfo(t));
		}
	}
}
