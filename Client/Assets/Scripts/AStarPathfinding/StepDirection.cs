namespace AStarPathfinding 
{

    public readonly struct StepDirection 
    {

        public readonly int X;
        public readonly int Y;
        
        public StepDirection(int x, int y) 
        {
            X = x;
            Y = y;
        }
    }

}