using RimWorld;
using Verse;

namespace BlackWidows
{
    // Token: 0x0200000D RID: 13
    public class CompWidowMilkableHumanoid : ThingComp
    {
        // Token: 0x04000018 RID: 24
        public int milkProgress;

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000017 RID: 23 RVA: 0x0000284C File Offset: 0x00000A4C
        public CompProperties_WidowMilkableHumanoid MilkProps => props as CompProperties_WidowMilkableHumanoid;

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000018 RID: 24 RVA: 0x0000286C File Offset: 0x00000A6C
        public bool Active
        {
            get
            {
                var pawn = parent as Pawn;
                var onlyFemales = MilkProps.onlyFemales;
                var result = false;
                if (onlyFemales)
                {
                    if (pawn != null)
                    {
                        result = pawn.gender == Gender.Female && IsOfProperAge;
                    }
                }
                else
                {
                    if (pawn != null)
                    {
                        result = !MilkProps.onlyMales || pawn.gender == Gender.Male && IsOfProperAge;
                    }
                }

                return result;
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000019 RID: 25 RVA: 0x000028D8 File Offset: 0x00000AD8
        public bool IsOfProperAge => ((Pawn) parent).ageTracker.AgeBiologicalYears >= MilkProps.minimumAgeToBeMilked;

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600001A RID: 26 RVA: 0x00002910 File Offset: 0x00000B10
        public bool ActiveAndCanBeMilked => Active && CanBeMilked;

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x0600001B RID: 27 RVA: 0x00002934 File Offset: 0x00000B34
        public bool CanBeMilked => milkProgress >= MilkProps.ticksUntilMilking;

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600001C RID: 28 RVA: 0x0000295C File Offset: 0x00000B5C
        public float MilkProgressPercent => milkProgress / (float) MilkProps.ticksUntilMilking;

        // Token: 0x0600001D RID: 29 RVA: 0x00002984 File Offset: 0x00000B84
        public Thing Milk(Pawn milker)
        {
            Thing result;
            if (!(parent is Pawn pawn))
            {
                result = null;
            }
            else
            {
                var thing = ThingMaker.MakeThing(MilkProps.milkDef);
                thing.stackCount = MilkProps.milkAmount;
                if (milker == null)
                {
                    if (MilkProps.milkThoughtMilkedSelf != null)
                    {
                        pawn.needs.mood.thoughts.memories.TryGainMemory(MilkProps.milkThoughtMilkedSelf);
                    }
                }
                else
                {
                    if (MilkProps.milkThoughtMilker != null)
                    {
                        milker.needs.mood.thoughts.memories.TryGainMemory(MilkProps.milkThoughtMilker, pawn);
                    }

                    if (MilkProps.milkThoughtMilked != null)
                    {
                        pawn.needs.mood.thoughts.memories.TryGainMemory(MilkProps.milkThoughtMilked, milker);
                    }
                }

                milkProgress = 0;
                result = thing;
            }

            return result;
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002AAC File Offset: 0x00000CAC
        public void GatherMilk(Pawn milker)
        {
            var thing = Milk(milker);
            if (thing != null)
            {
                GenSpawn.Spawn(thing, milker.Position, milker.Map);
            }
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002AE0 File Offset: 0x00000CE0
        public void GatherMilkSelf()
        {
            var thing = Milk(null);
            if (thing != null)
            {
                GenSpawn.Spawn(thing, parent.Position, parent.Map);
            }
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00002B1E File Offset: 0x00000D1E
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref milkProgress, "milkProgress");
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002B3C File Offset: 0x00000D3C
        public override string CompInspectStringExtra()
        {
            var active = Active;
            bool factionIsPlayer;
            if (active)
            {
                var thingWithComps = parent;
                factionIsPlayer = thingWithComps != null && thingWithComps.Faction.IsPlayer;
            }
            else
            {
                factionIsPlayer = false;
            }

            string result;
            if (factionIsPlayer)
            {
                var firstResourceName = MilkProps.firstResourceName;
                result = firstResourceName
                    ? MilkProps.milkProgessKeyString.Translate(MilkProgressPercent.ToStringPercent())
                    : MilkProps.widowMilkProgessKeyString.Translate(MilkProgressPercent.ToStringPercent());
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
            if (!Active)
            {
                return;
            }

            if (parent is not Pawn pawn)
            {
                return;
            }

            var hasFood = true;
            Need_Food food;
            if ((food = pawn.needs.food) != null && food.Starving)
            {
                hasFood = false;
            }

            if (hasFood && milkProgress < MilkProps.ticksUntilMilking)
            {
                milkProgress++;
            }
        }
    }
}