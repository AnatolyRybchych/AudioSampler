﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSampler.WaveMath
{
    //double between 0.0 and 1.0
    public class ClamedFloat:IComparable<ClamedFloat>, IComparable<double>
    {
        public const double MinValue = 0.0;
        public const double MaxValue = 1.0;
        private double value;
        public double Value
        {
            get => value;
            set
            {
                if (value < MinValue) this.value = MinValue;
                else if (value > MaxValue) this.value = MaxValue;
                else this.value = value;
            }
        }

        public ClamedFloat(double value)
        {
            Value = value;
        }

        public static implicit operator double(ClamedFloat val) => val.Value;
        public static implicit operator ClamedFloat(double val) => new ClamedFloat(val);
        public static bool operator ==(ClamedFloat left, ClamedFloat right) => left.Equals(right);
        public static bool operator!=(ClamedFloat left, ClamedFloat right) => !left.Equals(right);
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (obj is ClamedFloat) return ((ClamedFloat)(obj)).Value == Value;
            else if(obj is double) return ((double)(obj)) == Value;
            else if (obj is float) return ((double)(obj)) == Value;
            else return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        int IComparable<ClamedFloat>.CompareTo(ClamedFloat? other)
        {
            if (other is null) return 9999;
            else return (int)((Value - other.Value) * 0.5 * (double)Int32.MaxValue);
        }

        public int CompareTo(double other)
        {
            return value.CompareTo(other);
        }
    }
}
