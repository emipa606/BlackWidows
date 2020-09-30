using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    public class JobGiver_WidowMilkSelf : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            bool flag = pawn.AnimalOrWildMan();
            Job result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = !pawn.IsColonist;
                if (flag2)
                {
                    result = null;
                }
                else
                {
                    bool drafted = pawn.Drafted;
                    if (drafted)
                    {
                        result = null;
                    }
                    else
                    {
                        bool downed = pawn.Downed;
                        if (downed)
                        {
                            result = null;
                        }
                        else
                        {
                            bool flag3 = HealthAIUtility.ShouldSeekMedicalRest(pawn);
                            if (flag3)
                            {
                                result = null;
                            }
                            else
                            {
                                CompWidowMilkableHumanoid compWidowMilkableHumanoid = pawn.TryGetComp<CompWidowMilkableHumanoid>();
                                bool flag4 = compWidowMilkableHumanoid == null;
                                if (flag4)
                                {
                                    result = null;
                                }
                                else
                                {
                                    bool flag5 = !compWidowMilkableHumanoid.ActiveAndCanBeMilked;
                                    if (flag5)
                                    {
                                        result = null;
                                    }
                                    else
                                    {
                                        bool flag6 = !compWidowMilkableHumanoid.MilkProps.canMilkThemselves;
                                        if (flag6)
                                        {
                                            result = null;
                                        }
                                        else
                                        {
                                            Pawn pawn2 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
                                            bool flag7 = pawn2 != null;
                                            if (flag7)
                                            {
                                                bool flag8 = pawn.Faction == pawn2.Faction;
                                                if (flag8)
                                                {
                                                    bool flag9 = !pawn2.Drafted && !pawn2.Downed && !HealthAIUtility.ShouldSeekMedicalRest(pawn2);
                                                    if (flag9)
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
