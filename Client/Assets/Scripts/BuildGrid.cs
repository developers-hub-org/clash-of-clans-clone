namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class BuildGrid : MonoBehaviour
    {

        [Header("Adjustments")]
        public bool debug = true;
        public float right = 10;
        public float left = 10;
        public float up = 10;
        public float down = 10;

        public GridLayout grid = null;

        private static BuildGrid _instance = null; public static BuildGrid instanse { get { return _instance; } set { _instance = value; } }
        [HideInInspector] public List<Building> buildings = new List<Building>();
        [HideInInspector] public List<Building> unidentifiedBuildings = new List<Building>();
        [HideInInspector] public float cellSize = 1;
        [HideInInspector] public Vector2 xDirection = Vector2.right;
        [HideInInspector] public Vector2 yDirection = Vector2.up;

        public void RefreshBuildings()
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                buildings[i].AdjustUI(true);
            }
            for (int i = 0; i < unidentifiedBuildings.Count; i++)
            {
                unidentifiedBuildings[i].AdjustUI(true);
            }
        }

        public Building GetBuilding(long databaseID)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].databaseID == databaseID)
                {
                    return buildings[i];
                }
            }
            return null;
        }

        public Building GetBuilding(Data.BuildingID id, int x, int y)
        {
            for (int i = 0; i < unidentifiedBuildings.Count; i++)
            {
                if (unidentifiedBuildings[i].databaseID <= 0 && unidentifiedBuildings[i].id == id && unidentifiedBuildings[i].currentX == x & unidentifiedBuildings[i].currentY == y)
                {
                    return unidentifiedBuildings[i];
                }
            }
            return null;
        }

        public void AddUnidentifiedBuilding(Building building)
        {
            unidentifiedBuildings.Add(building);
        }

        public void RemoveUnidentifiedBuilding(Building building)
        {
            unidentifiedBuildings.Remove(building);
        }

        private void Awake()
        {
            cellSize = Mathf.Sqrt(Mathf.Pow(grid.cellSize.x / 2f, 2) + Mathf.Pow((grid.cellSize.y / 2f), 2));
            xDirection = new Vector2(grid.cellSize.x, grid.cellSize.y).normalized;
            yDirection = new Vector2(-grid.cellSize.x, grid.cellSize.y).normalized;
        }

        private void Start()
        {
            Building.buildInstanse = null;
            Building.selectedInstanse = null;
        }

        public void Clear()
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i])
                {
                    Destroy(buildings[i].gameObject);
                }
            }
            for (int i = 0; i < unidentifiedBuildings.Count; i++)
            {
                if (unidentifiedBuildings[i])
                {
                    Destroy(unidentifiedBuildings[i].gameObject);
                }
            }
            buildings.Clear();
            unidentifiedBuildings.Clear();
        }

        public Vector3 GetStartPosition(int x, int y)
        {
            //Vector3 position = transform.position;
            //position += (transform.right.normalized * x * _cellSize) + (transform.forward.normalized * y * _cellSize);
            //return position;
            return grid.CellToWorld(new Vector3Int(x, y, 0));
        }

        public Vector3 CellToWorld(int x, int y)
        {
            Vector3 position = grid.CellToWorld(new Vector3Int(x, y, 0));
            position.z = 0;
            return position;
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            Vector3Int cell = grid.WorldToCell(position);
            return new Vector2Int(cell.x, cell.y);
        }

        public Vector3 GetCenterPosition(int x, int y, int rows, int columns)
        {
            //Vector3 position = GetStartPosition(x, y);
            //position += (transform.right.normalized * columns * _cellSize / 2f) + (transform.forward.normalized * rows * _cellSize / 2f);
            //return position;
            Vector3 start = grid.CellToWorld(new Vector3Int(x, y, 0));
            Vector3 end = grid.CellToWorld(new Vector3Int(x + columns, y + rows, 0));
            return Vector3.Lerp(start, end, 0.5f);
        }

        public Vector3 GetEndPosition(int x, int y, int rows, int columns)
        {
            // Vector3 position = GetStartPosition(x, y);
            // position += (transform.right.normalized * columns * _cellSize) + (transform.forward.normalized * rows * _cellSize);
            // return position;
            return CellToWorld(x + columns, y + rows);
        }

        public Vector3 GetEndPosition(Building building)
        {
            return GetEndPosition(building.currentX, building.currentY, building.columns, building.rows);
        }

        public bool IsWorldPositionIsOnPlane(Vector3 position, int x, int y, int rows, int columns)
        {/*
            position = transform.InverseTransformPoint(position);
            Rect rect = new Rect(x, y, columns, rows);
            if(rect.Contains(new Vector2(position.x, position.z)))
            {
                return true;
            }*/
            return false;
        }

        public bool IsGridPositionIsOnBuilding(Vector2Int position, int x, int y, int rows, int columns)
        {
            Rect rect = new Rect(x, y, columns, rows);
            if (rect.Contains(new Vector2(position.x, position.y)))
            {
                return true;
            }
            return false;
        }

        public bool CanPlaceBuilding(Building building, int x, int y)
        {
            if (building.currentX < 0 || building.currentY < 0 || building.currentX + building.columns > Data.gridSize || building.currentY + building.rows > Data.gridSize)
            {
                return false;
            }
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i] != building)
                {
                    Rect rect1 = new Rect(buildings[i].currentX, buildings[i].currentY, buildings[i].columns, buildings[i].rows);
                    Rect rect2 = new Rect(building.currentX, building.currentY, building.columns, building.rows);
                    if (rect2.Overlaps(rect1))
                    {
                        return false;
                    }
                }
            }
            for (int i = 0; i < unidentifiedBuildings.Count; i++)
            {
                if (unidentifiedBuildings[i] != building)
                {
                    Rect rect1 = new Rect(unidentifiedBuildings[i].currentX, unidentifiedBuildings[i].currentY, unidentifiedBuildings[i].columns, unidentifiedBuildings[i].rows);
                    Rect rect2 = new Rect(building.currentX, building.currentY, building.columns, building.rows);
                    if (rect2.Overlaps(rect1))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Vector2Int GetBestBuildingPlace(int rows, int columns)
        {
            Vector2 center = new Vector2(Data.gridSize, Data.gridSize) * 0.5f;
            Vector2Int point = new Vector2Int(Mathf.FloorToInt((float)Data.gridSize / 2f), Mathf.FloorToInt((float)Data.gridSize / 2f));
            float distance = Mathf.Infinity;
            for (int x = 0; x < Data.gridSize; x++)
            {
                for (int y = 0; y < Data.gridSize; y++)
                {
                    if (x > (Data.gridSize - columns) || y > (Data.gridSize - rows) || (x == center.x && y == center.y)) { continue; }
                    float d = Vector2.Distance(center, new Vector2(x, y));
                    if (d < distance && CanPlaceBuilding(x, y, rows, columns))
                    {
                        distance = d;
                        point = new Vector2Int(x, y);
                    }
                }
            }
            return point;
        }

        public bool CanPlaceBuilding(int x, int y, int rows, int columns)
        {
            if (x < 0 || y < 0 || x + columns > Data.gridSize || y + rows > Data.gridSize)
            {
                return false;
            }
            for (int i = 0; i < buildings.Count; i++)
            {
                Rect rect1 = new Rect(buildings[i].currentX, buildings[i].currentY, buildings[i].columns, buildings[i].rows);
                Rect rect2 = new Rect(x, y, columns, rows);
                if (rect2.Overlaps(rect1))
                {
                    return false;
                }
            }
            for (int i = 0; i < unidentifiedBuildings.Count; i++)
            {
                Rect rect1 = new Rect(unidentifiedBuildings[i].currentX, unidentifiedBuildings[i].currentY, unidentifiedBuildings[i].columns, unidentifiedBuildings[i].rows);
                Rect rect2 = new Rect(x, y, columns, rows);
                if (rect2.Overlaps(rect1))
                {
                    return false;
                }
            }
            return true;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!debug) { return; }
            if (grid == null) { return; }
            Vector3 bl = grid.transform.position + Vector3.down * down + Vector3.left * left;
            Vector3 br = grid.transform.position + Vector3.down * down + Vector3.right * right;
            Vector3 tr = grid.transform.position + Vector3.up * up + Vector3.right * right;
            Vector3 tl = grid.transform.position + Vector3.up * up + Vector3.left * left;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(bl, br);
            Gizmos.DrawLine(br, tr);
            Gizmos.DrawLine(tr, tl);
            Gizmos.DrawLine(tl, bl);
            Vector3 start = grid.CellToWorld(new Vector3Int(0, 0, 0));
            start.z = 0;
            Vector3 end = grid.CellToWorld(new Vector3Int(Data.gridSize, Data.gridSize));
            end.z = 0;
            Vector3 side1 = grid.CellToWorld(new Vector3Int(Data.gridSize, 0));
            side1.z = 0;
            Vector3 side2 = grid.CellToWorld(new Vector3Int(0, Data.gridSize));
            side2.z = 0;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(start, 0.1f);
            Gizmos.DrawLine(start, side2);
            Gizmos.DrawLine(side2, end);
            Gizmos.DrawLine(end, side1);
            Gizmos.DrawLine(side1, start);
        }
        #endif

    }
}