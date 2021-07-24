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
            if (pawn.AnimalOrWildMan())
            {
                return null;
            }

            if (!pawn.IsColonist)
            {
                return null;
            }

            var drafted = pawn.Drafted;
            if (drafted)
            {
                return null;
            }

            var downed = pawn.Downed;
            if (downed)
            {
                return null;
            }

            if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
            {
                return null;
            }

            var compWidowMilkableHumanoid = pawn.TryGetComp<CompWidowMilkableHumanoid>();
            if (compWidowMilkableHumanoid == null)
            {
                return null;
            }

            if (!compWidowMilkableHumanoid.ActiveAndCanBeMilked)
            {
                return null;
            }

            if (!compWidowMilkableHumanoid.MilkProps.canMilkThemselves)
            {
                return null;
            }

            var pawn2 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
            if (pawn2 == null)
            {
                return new Job(WidowDefOf.WidowMilkySelf);
            }

            if (pawn.Faction != pawn2.Faction)
            {
                return new Job(WidowDefOf.WidowMilkySelf);
            }

            if (!pawn2.Drafted && !pawn2.Downed &&
                !HealthAIUtility.ShouldSeekMedicalRest(pawn2))
            {
                return null;
            }

            return new Job(WidowDefOf.WidowMilkySelf);
        }
    }
}