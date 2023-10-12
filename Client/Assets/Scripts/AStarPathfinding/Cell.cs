namespace AStarPathfinding 
{
    public class Cell 
    {
        public bool Blocked;
        public bool Closed;
        public double F;
        public double G;
        public double H;

        public Vector2Int Location;
        public Cell Parent;
        public int QueueIndex;

        public Cell(Vector2Int location) => Location = location;
        public override string ToString() => $"[{Location.X},{Location.Y}]";
    }
}