using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BlackWidows
{
    // Token: 0x0200000C RID: 12
    public class CompProperties_WidowMilkableHumanoid : CompProperties
    {
        // Token: 0x04000012 RID: 18
        public bool canMilkThemselves = true;

        // Token: 0x04000016 RID: 22
        public bool firstResourceName;

        // Token: 0x04000013 RID: 19
        public List<JobDef> forbiddenJobsToInterrupt = new List<JobDef>();

        // Token: 0x0400000B RID: 11
        public int milkAmount = 15;

        // Token: 0x0400000A RID: 10
        public ThingDef milkDef;

        // Token: 0x04000015 RID: 21
        public string milkProgessKeyString = "WidowMilkProgress";

        // Token: 0x0400000D RID: 13
        public ThoughtDef milkThoughtMilked;

        // Token: 0x0400000E RID: 14
        public ThoughtDef milkThoughtMilkedSelf;

        // Token: 0x0400000C RID: 12
        public ThoughtDef milkThoughtMilker;

        // Token: 0x04000014 RID: 20
        public int minimumAgeToBeMilked;

        // Token: 0x04000010 RID: 16
        public bool onlyFemales = true;

        // Token: 0x04000011 RID: 17
        public bool onlyMales;

        // Token: 0x0400000F RID: 15
        public int ticksUntilMilking = 60000;

        // Token: 0x04000017 RID: 23
        public string widowMilkProgessKeyString = "WidowResourceProgress";

        // Token: 0x06000016 RID: 22 RVA: 0x000027E4 File Offset: 0x000009E4
        public CompProperties_WidowMilkableHumanoid()
        {
            compClass = typeof(CompWidowMilkableHumanoid);
        }
    }
}