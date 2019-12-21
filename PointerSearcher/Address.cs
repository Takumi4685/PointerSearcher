using System;

namespace PointerSearcher
{
    class Address : IComparable<Address>
    {

        public override int GetHashCode()
        {
            return this.type.GetHashCode() ^ this.offset.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // asでは、objがnullでも例外は発生せずにnullが入ってくる
            var other = obj as Address;
            if (other == null) return false;

            // 何が同じときに、「同じ」と判断してほしいかを記述する
            return this.type == other.type &&
                    this.offset == other.offset;
        }
        public Address(MemoryType memType, long addrOffset)
        {
            this.type = memType;
            this.offset = addrOffset;
        }
        public long OffsetFrom(Address src)
        {
            if (src.type != this.type)
            {
                return long.MaxValue;
            }
            return this.offset - src.offset;
        }

        int IComparable<Address>.CompareTo(Address obj)
        {
            if (this.type != obj.type)
            {
                if (this.type == MemoryType.MAIN)
                {
                    return int.MinValue;
                }
                return int.MaxValue;
            }
            return this.offset.CompareTo(obj.offset);
        }

        static public Address operator +(Address src, long addrOffset)
        {
            return new Address(src.type, src.offset + addrOffset);
        }
        static public Address operator -(Address src, long addrOffset)
        {
            return new Address(src.type, src.offset - addrOffset);
        }
        public MemoryType type { get; set; }
        public long offset { get; set; }
    }
}
