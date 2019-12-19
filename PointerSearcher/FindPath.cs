using System;
using System.Collections.Generic;

namespace PointerSearcher
{
    abstract class IReverseOrderPath
    {
        public IReverseOrderPath()
        {
            addrMemo = new Dictionary<int, Address>();
        }
        public abstract String ToString(String org);
        public Dictionary<int, Address> addrMemo;   //keys = DumpData No.
        public abstract long ParseAddress(long currentAddress, long currentData);
    }
    class ReverseOrderPathOffset : IReverseOrderPath
    {
        public ReverseOrderPathOffset(long offsetAddress)
        {
            offset = offsetAddress;
        }
        private long offset;

        public override String ToString(String org)
        {
            return org + "+" + offset.ToString("X");
        }
        public override long ParseAddress(long currentAddress, long currentData)
        {
            return currentAddress + offset;
        }
    }
    class ReverseOrderPathPointerJump : IReverseOrderPath
    {
        public override String ToString(String org)
        {
            return "[" + org + "]";
        }
        public override long ParseAddress(long currentAddress, long currentData)
        {
            return currentData;
        }
    }
}
