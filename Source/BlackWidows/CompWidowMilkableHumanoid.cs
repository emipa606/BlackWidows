using RimWorld;
using Verse;

namespace BlackWidows
{
    public class CompWidowMilkableHumanoid : ThingComp
    {
        public int milkProgress;

        public CompProperties_WidowMilkableHumanoid MilkProps => props as CompProperties_WidowMilkableHumanoid;

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

        public bool IsOfProperAge => ((Pawn) parent).ageTracker.AgeBiologicalYears >= MilkProps.minimumAgeToBeMilked;

        public bool ActiveAndCanBeMilked => Active && CanBeMilked;

        public bool CanBeMilked => milkProgress >= MilkProps.ticksUntilMilking;

        public float MilkProgressPercent => milkProgress / (float) MilkProps.ticksUntilMilking;

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

        public void GatherMilk(Pawn milker)
        {
            var thing = Milk(milker);
            if (thing != null)
            {
                GenSpawn.Spawn(thing, milker.Position, milker.Map);
            }
        }

        public void GatherMilkSelf()
        {
            var thing = Milk(null);
            if (thing != null)
            {
                GenSpawn.Spawn(thing, parent.Position, parent.Map);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref milkProgress, "milkProgress");
        }

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
