using System;
using System.Collections.Generic;

namespace PointerSearcher
{
    class PointerInfo
    {
        public PointerInfo()
        {
            tmpDic = new Dictionary<Address, PointedAddress>();
        }
        public void AddPointer(Address from, Address to)
        {
            PointedAddress add;

            if (!tmpDic.ContainsKey(to))
            {
                add = new PointedAddress(to);

                tmpDic.Add(to, add);
            }
            tmpDic[to].pointedfrom.Add(from);
        }
        public int FindNearest(IComparable<Address> target)
        {
            if (pointedList.Count == 0)
            {
                return -1;
            }

            int l = 0;
            int r = pointedList.Count - 1;

            while (l <= r)
            {
                int m = l + (r - l) / 2;
                int result = target.CompareTo(pointedList[m].addr);
                if (result == 0)
                {
                    return m;
                }
                else if (result < 0)
                {
                    r = m - 1;
                }
                else
                {
                    l = m + 1;
                }
            }

            return r;
        }
        public void MakeList()
        {
            pointedList = new List<PointedAddress>(tmpDic.Values);
            tmpDic = null;
            pointedList.Sort();
        }
        private Dictionary<Address, PointedAddress> tmpDic;
        public List<PointedAddress> pointedList;

    }
}
