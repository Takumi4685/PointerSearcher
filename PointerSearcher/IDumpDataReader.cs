using System.Collections.Generic;

namespace PointerSearcher
{
    internal interface IDumpDataReader
    {
        PointerInfo Read();
        long TryToParseAbs(List<IReverseOrderPath> path);
        Address TryToParseRel(List<IReverseOrderPath> path);
    }
}
