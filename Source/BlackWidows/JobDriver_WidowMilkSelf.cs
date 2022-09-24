using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    public class JobDriver_WidowMilkSelf : JobDriver
    {
        private float gatherProgress;

        protected float WorkTotal => 600f;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            var wait = new Toil();
            wait.tickAction = delegate
            {
                var actor = wait.actor;
                gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed);
                if (!(gatherProgress >= WorkTotal))
                {
                    return;
                }

                actor.TryGetComp<CompWidowMilkableHumanoid>().GatherMilkSelf();
                actor.jobs.EndCurrentJob(JobCondition.Succeeded);
            };
            wait.AddEndCondition(delegate
            {
                var actor = wait.actor;
                var result = !actor.TryGetComp<CompWidowMilkableHumanoid>().ActiveAndCanBeMilked
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
