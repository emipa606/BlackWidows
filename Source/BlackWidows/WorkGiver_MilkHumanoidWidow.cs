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
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

        // Token: 0x06000008 RID: 8 RVA: 0x000023A1 File Offset: 0x000005A1
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            var pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
            int num;
            for (var i = 0; i < pawns.Count; i = num + 1)
            {
                yield return pawns[i];
                num = i;
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000023B8 File Offset: 0x000005B8
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is Pawn pawn2))
            {
                return false;
            }

            var pawn3 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
            if (pawn3 == null)
            {
                return false;
            }

            if (pawn.Faction != pawn2.Faction)
            {
                return false;
            }

            if (pawn3 != pawn2)
            {
                return false;
            }

            var drafted = pawn3.Drafted;
            if (drafted)
            {
                return false;
            }

            if (pawn3.CurJob != null && pawn3.jobs.curDriver.asleep)
            {
                return false;
            }

            if (pawn3.CurJob is {playerForced: true})
            {
                return false;
            }

            var compWidowMilkableHumanoid = pawn3.TryGetComp<CompWidowMilkableHumanoid>();
            if (compWidowMilkableHumanoid == null)
            {
                return false;
            }

            if (!compWidowMilkableHumanoid.ActiveAndCanBeMilked)
            {
                return false;
            }

            if (pawn3.CurJob != null &&
                compWidowMilkableHumanoid.MilkProps
                    .forbiddenJobsToInterrupt.Count > 0 &&
                compWidowMilkableHumanoid.MilkProps
                    .forbiddenJobsToInterrupt.Contains(pawn3.CurJob.def))
            {
                return false;
            }

            if (pawn3.Position.IsForbidden(pawn))
            {
                return false;
            }

            if (!pawn.CanReserve(new LocalTargetInfo(pawn3)))
            {
                return false;
            }

            return pawn.CanReach(new LocalTargetInfo(pawn3),
                PathEndMode.ClosestTouch, Danger.Deadly);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000025BC File Offset: 0x000007BC
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(WidowDefOf.WidowMilkyLover, new LocalTargetInfo(t));
        }
    }
}