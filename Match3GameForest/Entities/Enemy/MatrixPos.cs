using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3GameForest.Entities
{
    public struct MatrixPos
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public bool IsSet { get; private set; }

        public MatrixPos(int X, int Y)
        {
            this.X = X;
            this.Y = Y;

            IsSet = true;
        }

        public static bool operator ==(MatrixPos lhs, MatrixPos rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MatrixPos lhs, MatrixPos rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is MatrixPos pos &&
                   X == pos.X &&
                   Y == pos.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 490714174;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
