using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlackWidows
{
	// Token: 0x0200000B RID: 11
	public class JobDriver_MilkHumanoidWidow : JobDriver
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000027A0 File Offset: 0x000009A0
		protected float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

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
			Toil wait = new Toil();
			wait.initAction = delegate()
			{
				Pawn actor = wait.actor;
				Pawn pawn = (Pawn)wait.actor.CurJob.GetTarget(TargetIndex.A).Thing;
				actor.pather.StopDead();
				PawnUtility.ForceWait(pawn, 15000, null, true);
			};
			wait.tickAction = delegate()
			{
				Pawn actor = wait.actor;
				actor.skills.Learn(SkillDefOf.Social, 0.142999992f, false);
				this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed, true);
				bool flag = this.gatherProgress >= this.WorkTotal;
				bool flag2 = flag;
				if (flag2)
				{
					Pawn thing = (Pawn)((Thing)this.job.GetTarget(TargetIndex.A));
					thing.TryGetComp<CompWidowMilkableHumanoid>().GatherMilk(this.pawn);
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			wait.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)wait.actor.CurJob.GetTarget(TargetIndex.A).Thing;
				bool flag = pawn.jobs.curJob.def == JobDefOf.Wait_MaintainPosture;
				bool flag2 = flag;
				if (flag2)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			});
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.AddEndCondition(delegate
			{
				Pawn thing = (Pawn)((Thing)this.job.GetTarget(TargetIndex.A));
				bool flag = !thing.TryGetComp<CompWidowMilkableHumanoid>().ActiveAndCanBeMilked;
				bool flag2 = flag;
				JobCondition result;
				if (flag2)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, () => this.gatherProgress / this.WorkTotal, false, -0.5f);
			yield return wait;
			yield break;
		}

		// Token: 0x04000009 RID: 9
		private float gatherProgress;
	}
}
