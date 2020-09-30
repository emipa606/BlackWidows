using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BlackWidows
{
    public class CompProperties_WidowMilkableHumanoid : CompProperties
    {
        public CompProperties_WidowMilkableHumanoid()
        {
            this.compClass = typeof(CompWidowMilkableHumanoid);
        }
        public ThingDef milkDef;

        public int milkAmount = 15;

        public ThoughtDef milkThoughtMilker;

        public ThoughtDef milkThoughtMilked;

        public ThoughtDef milkThoughtMilkedSelf;

        public int ticksUntilMilking = 60000;

        public bool onlyFemales = true;

        public bool onlyMales;

        public bool canMilkThemselves = true;

        public List<JobDef> forbiddenJobsToInterrupt = new List<JobDef>();

        public int minimumAgeToBeMilked;

        public string milkProgessKeyString = "WidowMilkProgress";

        public bool firstResourceName;

        public string widowMilkProgessKeyString = "WidowResourceProgress";
    }
}
