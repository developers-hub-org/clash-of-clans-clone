using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarPathfinding
{
    public class Grid : IGridProvider
    {

        private readonly Cell[,] _cells;
        public int Width { get; set; }
        public int Height { get; set; }
        public Cell this[int x, int y] => _cells[x, y];
        public Vector2Int Size => new Vector2Int(Width, Height);
        public Cell this[Vector2Int location] => _cells[location.X, location.Y];

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new Cell[width, height];
            Reset();
        }

        public void Reset()
        {
            for (var x = 0; x <= _cells.GetUpperBound(0); x++)
            {
                for (var y = 0; y <= _cells.GetUpperBound(1); y++)
                {
                    var cell = _cells[x, y];
                    if (cell == null)
                    {
                        _cells[x, y] = new Cell(new Vector2Int(x, y));
                    }
                    else
                    {
                        cell.G = 0;
                        cell.H = 0;
                        cell.F = 0;
                        cell.Closed = false;
                        cell.Parent = null;
                    }
                }
            }
        }

        public int GetNodeId(Vector2Int location) => location.X * Width + location.Y;
    }
}