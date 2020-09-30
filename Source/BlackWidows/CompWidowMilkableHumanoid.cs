using System;
using RimWorld;
using Verse;

namespace BlackWidows
{
	// Token: 0x0200000D RID: 13
	public class CompWidowMilkableHumanoid : ThingComp
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000284C File Offset: 0x00000A4C
		public CompProperties_WidowMilkableHumanoid MilkProps
		{
			get
			{
				return this.props as CompProperties_WidowMilkableHumanoid;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000286C File Offset: 0x00000A6C
		public bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				bool onlyFemales = this.MilkProps.onlyFemales;
				bool result;
				if (onlyFemales)
				{
					result = (pawn.gender == Gender.Female && this.IsOfProperAge);
				}
				else
				{
					result = (!this.MilkProps.onlyMales || (pawn.gender == Gender.Male && this.IsOfProperAge));
				}
				return result;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000028D8 File Offset: 0x00000AD8
		public bool IsOfProperAge
		{
			get
			{
				return (this.parent as Pawn).ageTracker.AgeBiologicalYears >= this.MilkProps.minimumAgeToBeMilked;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002910 File Offset: 0x00000B10
		public bool ActiveAndCanBeMilked
		{
			get
			{
				return this.Active && this.CanBeMilked;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002934 File Offset: 0x00000B34
		public bool CanBeMilked
		{
			get
			{
				return this.milkProgress >= this.MilkProps.ticksUntilMilking;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000295C File Offset: 0x00000B5C
		public float MilkProgressPercent
		{
			get
			{
				return (float)this.milkProgress / (float)this.MilkProps.ticksUntilMilking;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002984 File Offset: 0x00000B84
		public Thing Milk(Pawn milker)
		{
			Pawn pawn = this.parent as Pawn;
			bool flag = pawn == null;
			Thing result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(this.MilkProps.milkDef, null);
				thing.stackCount = this.MilkProps.milkAmount;
				bool flag2 = milker == null;
				if (flag2)
				{
					bool flag3 = this.MilkProps.milkThoughtMilkedSelf != null;
					if (flag3)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(this.MilkProps.milkThoughtMilkedSelf, null);
					}
				}
				else
				{
					bool flag4 = this.MilkProps.milkThoughtMilker != null;
					if (flag4)
					{
						milker.needs.mood.thoughts.memories.TryGainMemory(this.MilkProps.milkThoughtMilker, pawn);
					}
					bool flag5 = this.MilkProps.milkThoughtMilked != null;
					if (flag5)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(this.MilkProps.milkThoughtMilked, milker);
					}
				}
				this.milkProgress = 0;
				result = thing;
			}
			return result;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002AAC File Offset: 0x00000CAC
		public void GatherMilk(Pawn milker)
		{
			Thing thing = this.Milk(milker);
			bool flag = thing != null;
			if (flag)
			{
				GenSpawn.Spawn(thing, milker.Position, milker.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public void GatherMilkSelf()
		{
			Thing thing = this.Milk(null);
			bool flag = thing != null;
			if (flag)
			{
				GenSpawn.Spawn(thing, this.parent.Position, this.parent.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002B1E File Offset: 0x00000D1E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.milkProgress, "milkProgress", 0, false);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B3C File Offset: 0x00000D3C
		public override string CompInspectStringExtra()
		{
			bool active = this.Active;
			bool flag;
			if (active)
			{
				ThingWithComps parent = this.parent;
				flag = (parent != null && parent.Faction.IsPlayer);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			string result;
			if (flag2)
			{
				bool firstResourceName = this.MilkProps.firstResourceName;
				if (firstResourceName)
				{
					result = this.MilkProps.milkProgessKeyString.Translate(new object[]
					{
						this.MilkProgressPercent.ToStringPercent()
					});
				}
				else
				{
					result = this.MilkProps.widowMilkProgessKeyString.Translate(new object[]
					{
						this.MilkProgressPercent.ToStringPercent()
					});
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002BEC File Offset: 0x00000DEC
		public override void CompTick()
		{
			bool active = this.Active;
			if (active)
			{
				Pawn pawn = this.parent as Pawn;
				bool flag = pawn != null;
				if (flag)
				{
					bool flag2 = true;
					Need_Food food;
					bool flag3 = (food = pawn.needs.food) != null && food != null && food.Starving;
					if (flag3)
					{
						flag2 = false;
					}
					bool flag4 = flag2 && this.milkProgress < this.MilkProps.ticksUntilMilking;
					if (flag4)
					{
						this.milkProgress++;
					}
				}
			}
		}

		// Token: 0x04000018 RID: 24
		public int milkProgress;
	}
}
