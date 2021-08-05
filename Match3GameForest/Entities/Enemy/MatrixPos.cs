namespace Match3GameForest.Entities
{
    public struct MatrixPos
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public MatrixPos(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static bool operator ==(MatrixPos lhs, MatrixPos rhs) => lhs.Equals(rhs);

        public static bool operator !=(MatrixPos lhs, MatrixPos rhs) => !(lhs == rhs);

        public override bool Equals(object obj) => obj is MatrixPos pos &&
                   X == pos.X &&
                   Y == pos.Y;

        public override int GetHashCode()
        {
            int hashCode = 490714174;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
