using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
	// Token: 0x02000009 RID: 9
	public class JobGiver_WidowMilkSelf : ThinkNode_JobGiver
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000025E8 File Offset: 0x000007E8
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = pawn.AnimalOrWildMan();
			bool flag2 = flag;
			Job result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				bool flag3 = !pawn.IsColonist;
				bool flag4 = flag3;
				if (flag4)
				{
					result = null;
				}
				else
				{
					bool drafted = pawn.Drafted;
					bool flag5 = drafted;
					if (flag5)
					{
						result = null;
					}
					else
					{
						bool downed = pawn.Downed;
						bool flag6 = downed;
						if (flag6)
						{
							result = null;
						}
						else
						{
							bool flag7 = HealthAIUtility.ShouldSeekMedicalRest(pawn);
							bool flag8 = flag7;
							if (flag8)
							{
								result = null;
							}
							else
							{
								CompWidowMilkableHumanoid compWidowMilkableHumanoid = pawn.TryGetComp<CompWidowMilkableHumanoid>();
								bool flag9 = compWidowMilkableHumanoid == null;
								bool flag10 = flag9;
								if (flag10)
								{
									result = null;
								}
								else
								{
									bool flag11 = !compWidowMilkableHumanoid.ActiveAndCanBeMilked;
									bool flag12 = flag11;
									if (flag12)
									{
										result = null;
									}
									else
									{
										bool flag13 = !compWidowMilkableHumanoid.MilkProps.canMilkThemselves;
										bool flag14 = flag13;
										if (flag14)
										{
											result = null;
										}
										else
										{
											Pawn pawn2 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
											bool flag15 = pawn2 != null;
											bool flag16 = flag15;
											if (flag16)
											{
												bool flag17 = pawn.Faction == pawn2.Faction;
												bool flag18 = flag17;
												if (flag18)
												{
													bool flag19 = !pawn2.Drafted && !pawn2.Downed && !HealthAIUtility.ShouldSeekMedicalRest(pawn2);
													bool flag20 = flag19;
													if (flag20)
													{
														return null;
													}
												}
											}
											result = new Job(WidowDefOf.WidowMilkySelf);
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
	}
}
