using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    public class JobDriver_MilkHumanoidWidow : JobDriver
    {
        private float gatherProgress;

        protected float WorkTotal => 400f;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnDowned(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            var wait = new Toil();
            wait.initAction = delegate
            {
                var actor = wait.actor;
                var thing = (Pawn) wait.actor.CurJob.GetTarget(TargetIndex.A).Thing;
                actor.pather.StopDead();
                PawnUtility.ForceWait(thing, 15000, null, true);
            };
            wait.tickAction = delegate
            {
                var actor = wait.actor;
                actor.skills.Learn(SkillDefOf.Social, 0.142999992f);
                gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed);
                if (!(gatherProgress >= WorkTotal))
                {
                    return;
                }

                var thing = (Pawn) (Thing) job.GetTarget(TargetIndex.A);
                thing.TryGetComp<CompWidowMilkableHumanoid>().GatherMilk(pawn);
                actor.jobs.EndCurrentJob(JobCondition.Succeeded);
            };
            wait.AddFinishAction(delegate
            {
                var thing = (Pawn) wait.actor.CurJob.GetTarget(TargetIndex.A).Thing;
                if (thing.jobs.curJob.def == JobDefOf.Wait_MaintainPosture)
                {
                    thing.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
            });
            wait.FailOnDespawnedOrNull(TargetIndex.A);
            wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            wait.AddEndCondition(delegate
            {
                var thing = (Pawn) (Thing) job.GetTarget(TargetIndex.A);
                var result = !thing.TryGetComp<CompWidowMilkableHumanoid>().ActiveAndCanBeMilked
                    ? JobCondition.Incompletable
                    : JobCondition.Ongoing;

                return result;
            });
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            wait.WithProgressBar(TargetIndex.A, () => gatherProgress / WorkTotal);
            yield return wait;
        }
    }
}
