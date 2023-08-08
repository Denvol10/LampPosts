using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampPosts.Models
{
    public class LampPostLocationIEqualityComparer : IEqualityComparer<LampPostLocation>
    {
        public bool Equals(LampPostLocation x, LampPostLocation y)
        {
            return x.Point.IsAlmostEqualTo(y.Point);
        }

        public int GetHashCode(LampPostLocation obj)
        {
            return HashCode.Combine(obj.Point.X, obj.Point.Y, obj.Point.Z);
        }
    }
}
