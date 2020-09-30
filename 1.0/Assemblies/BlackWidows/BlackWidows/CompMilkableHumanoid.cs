using System;
using RimWorld;
using Verse;

namespace BlackWidows
{
    public class CompWidowMilkableHumanoid : ThingComp
    {

        public CompProperties_WidowMilkableHumanoid MilkProps
        {
            get
            {
                return this.props as CompProperties_WidowMilkableHumanoid;
            }
        }
        public bool Active
        {
            get
            {
                Pawn pawn = this.parent as Pawn;
                bool result;
                if (this.MilkProps.onlyFemales)
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

        public bool IsOfProperAge
        {
            get
            {
                return (this.parent as Pawn).ageTracker.AgeBiologicalYears >= this.MilkProps.minimumAgeToBeMilked;
            }
        }

        public bool ActiveAndCanBeMilked
        {
            get
            {
                return this.Active && this.CanBeMilked;
            }
        }

        public bool CanBeMilked
        {
            get
            {
                return this.milkProgress >= this.MilkProps.ticksUntilMilking;
            }
        }

        public float MilkProgressPercent
        {
            get
            {
                return (float)this.milkProgress / (float)this.MilkProps.ticksUntilMilking;
            }
        }

        public Thing Milk(Pawn milker)
        {
            Pawn pawn = this.parent as Pawn;
            Thing result;
            if (pawn == null)
            {
                result = null;
            }
            else
            {
                Thing thing = ThingMaker.MakeThing(this.MilkProps.milkDef, null);
                thing.stackCount = this.MilkProps.milkAmount;
                if (milker == null)
                {
                    if (this.MilkProps.milkThoughtMilkedSelf != null)
                    {
                        pawn.needs.mood.thoughts.memories.TryGainMemory(this.MilkProps.milkThoughtMilkedSelf, null);
                    }
                }
                else
                {
                    if (this.MilkProps.milkThoughtMilker != null)
                    {
                        milker.needs.mood.thoughts.memories.TryGainMemory(this.MilkProps.milkThoughtMilker, pawn);
                    }
                    if (this.MilkProps.milkThoughtMilked != null)
                    {
                        pawn.needs.mood.thoughts.memories.TryGainMemory(this.MilkProps.milkThoughtMilked, milker);
                    }
                }
                this.milkProgress = 0;
                result = thing;
            }
            return result;
        }

        public void GatherMilk(Pawn milker)
        {
            Thing thing = this.Milk(milker);
            if (thing != null)
            {
                GenSpawn.Spawn(thing, milker.Position, milker.Map, WipeMode.Vanish);
            }
        }

        public void GatherMilkSelf()
        {
            Thing thing = this.Milk(null);
            if (thing != null)
            {
                GenSpawn.Spawn(thing, this.parent.Position, this.parent.Map, WipeMode.Vanish);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref this.milkProgress, "milkProgress", 0, false);
        }

        public override string CompInspectStringExtra()
        {
            bool flag;
            if (this.Active)
            {
                ThingWithComps parent = this.parent;
                flag = (parent != null && parent.Faction.IsPlayer);
            }
            else
            {
                flag = false;
            }
            string result;
            if (flag)
            {
                if (this.MilkProps.firstResourceName)
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
        public override void CompTick()
        {
            if (this.Active)
            {
                Pawn pawn = this.parent as Pawn;
                if (pawn != null)
                {
                    bool flag = true;
                    Need_Food food;
                    if ((food = pawn.needs.food) != null && food != null && food.Starving)
                    {
                        flag = false;
                    }
                    if (flag && this.milkProgress < this.MilkProps.ticksUntilMilking)
                    {
                        this.milkProgress++;
                    }
                }
            }
        }
        public int milkProgress;
    }
}
