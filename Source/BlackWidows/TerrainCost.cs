using System.Collections.Generic;
using Verse;

namespace BlackWidows;

public class TerrainCost : DefModExtension
{
    public int costToAdd = 0;

    public int costToRefund = 0;

    public List<string> tags;
}