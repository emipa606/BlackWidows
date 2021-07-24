using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
    // Token: 0x0200000A RID: 10
    public class JobDriver_WidowMilkSelf : JobDriver
    {
        // Token: 0x04000008 RID: 8
        private float gatherProgress;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600000E RID: 14 RVA: 0x0000275C File Offset: 0x0000095C
        protected float WorkTotal => 600f;

        // Token: 0x0600000F RID: 15 RVA: 0x00002774 File Offset: 0x00000974
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002787 File Offset: 0x00000987
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