using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows;

public class WorkGiver_MilkHumanoidWidow : WorkGiver_Scanner
{
    public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

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

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        if (t is not Pawn pawn2)
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

        if (pawn3.CurJob is { playerForced: true })
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

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        return new Job(WidowDefOf.WidowMilkyLover, new LocalTargetInfo(t));
    }
}