namespace AStarPathfinding 
{

    public interface IGridProvider 
    {
        Vector2Int Size { get; }
        Cell this[Vector2Int position] { get; }
        void Reset();
    }

}