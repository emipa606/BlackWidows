using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    // Token: 0x0200000B RID: 11
    public class JobDriver_MilkHumanoidWidow : JobDriver
    {
        // Token: 0x04000009 RID: 9
        private float gatherProgress;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000012 RID: 18 RVA: 0x000027A0 File Offset: 0x000009A0
        protected float WorkTotal => 400f;

        // Token: 0x06000013 RID: 19 RVA: 0x000027B8 File Offset: 0x000009B8
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000027CB File Offset: 0x000009CB
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