namespace Thaumaturgia.Utils {
    public class BlockPos {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public BlockPos(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

        public float DistanceTo(BlockPos other) {
            return MathF.Sqrt(MathF.Pow(other.X - X, 2) + MathF.Pow(other.Y - Y, 2) + MathF.Pow(other.Z - Z, 2));
        }

        public override string ToString() {
            return $"BlockPos({X}, {Y}, {Z})";
        }

        public override bool Equals(object obj) {
            if (obj is BlockPos other) {
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return false;
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y, Z);
        }
    }
}