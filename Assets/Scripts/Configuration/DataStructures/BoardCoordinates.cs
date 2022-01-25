namespace Configuration.DataStructures
{
    public class BoardCoordinates
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public readonly int X;
        public readonly int Y;

        public BoardCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != GetType()) return false;

            return X == ((BoardCoordinates) other).X &&
                   Y == ((BoardCoordinates) other).Y;
        }

        public static bool operator ==(BoardCoordinates lhs, BoardCoordinates rhs)
        {
            var leftNull = ReferenceEquals(lhs, null);
            var rightNull = ReferenceEquals(rhs, null);
            if (leftNull && rightNull)
            {
                return true;
            }

            if (leftNull || rightNull)
            {
                return false;
            }

            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(BoardCoordinates lhs, BoardCoordinates rhs)
        {
            var leftNull = ReferenceEquals(lhs, null);
            var rightNull = ReferenceEquals(rhs, null);

            if (leftNull && rightNull)
            {
                return false;
            }

            if (leftNull || rightNull)
            {
                return true;
            }

            return lhs.X != rhs.X || lhs.Y != rhs.Y;
        }

        public static BoardCoordinates operator -(BoardCoordinates lhs, BoardCoordinates rhs)
        {
            return new BoardCoordinates(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public override string ToString()
        {
            return $"[{X} : {Y}]";
        }
    }
}