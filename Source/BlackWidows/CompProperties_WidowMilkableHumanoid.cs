using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BlackWidows;

public class CompProperties_WidowMilkableHumanoid : CompProperties
{
    public bool canMilkThemselves = true;

    public bool firstResourceName;

    public List<JobDef> forbiddenJobsToInterrupt = new List<JobDef>();

    public int milkAmount = 15;

    public ThingDef milkDef;

    public string milkProgessKeyString = "WidowMilkProgress";

    public ThoughtDef milkThoughtMilked;

    public ThoughtDef milkThoughtMilkedSelf;

    public ThoughtDef milkThoughtMilker;

    public int minimumAgeToBeMilked;

    public bool onlyFemales = true;

    public bool onlyMales;

    public int ticksUntilMilking = 60000;

    public string widowMilkProgessKeyString = "WidowResourceProgress";

    public CompProperties_WidowMilkableHumanoid()
    {
        compClass = typeof(CompWidowMilkableHumanoid);
    }
}