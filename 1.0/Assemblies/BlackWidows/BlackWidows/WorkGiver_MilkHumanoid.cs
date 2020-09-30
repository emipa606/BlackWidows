using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    public class WorkGiver_MilkHumanoidWidow : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
            }
        }

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

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = t as Pawn;
            bool flag = pawn2 == null;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                Pawn pawn3 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
                bool flag2 = pawn3 == null;
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = pawn.Faction != pawn2.Faction;
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = pawn3 != pawn2;
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            bool drafted = pawn3.Drafted;
                            if (drafted)
                            {
                                result = false;
                            }
                            else
                            {
                                bool flag5 = pawn3.CurJob != null && pawn3.jobs.curDriver.asleep;
                                if (flag5)
                                {
                                    result = false;
                                }
                                else
                                {
                                    bool flag6 = pawn3.CurJob != null && pawn3.CurJob.playerForced;
                                    if (flag6)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        CompWidowMilkableHumanoid compWidowMilkableHumanoid = pawn3.TryGetComp<CompWidowMilkableHumanoid>();
                                        bool flag7 = compWidowMilkableHumanoid == null;
                                        if (flag7)
                                        {
                                            result = false;
                                        }
                                        else
                                        {
                                            bool flag8 = !compWidowMilkableHumanoid.ActiveAndCanBeMilked;
                                            if (flag8)
                                            {
                                                result = false;
                                            }
                                            else
                                            {
                                                bool flag9 = pawn3.CurJob != null && compWidowMilkableHumanoid.MilkProps.forbiddenJobsToInterrupt.Count > 0 && compWidowMilkableHumanoid.MilkProps.forbiddenJobsToInterrupt.Contains(pawn3.CurJob.def);
                                                if (flag9)
                                                {
                                                    result = false;
                                                }
                                                else
                                                {
                                                    bool flag10 = pawn3.Position.IsForbidden(pawn);
                                                    if (flag10)
                                                    {
                                                        result = false;
                                                    }
                                                    else
                                                    {
                                                        bool flag11 = !pawn.CanReserve(new LocalTargetInfo(pawn3), 1, -1, null, false);
                                                        if (flag11)
                                                        {
                                                            result = false;
                                                        }
                                                        else
                                                        {
                                                            bool flag12 = !pawn.CanReach(new LocalTargetInfo(pawn3), PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn);
                                                            result = !flag12;
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

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {

                return new Job(WidowDefOf.WidowMilkyLover, new LocalTargetInfo(t));
        }
    }
}
