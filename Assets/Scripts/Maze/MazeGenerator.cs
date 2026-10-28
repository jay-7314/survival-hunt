using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int cellsWide = 5;     //칸이 가로세로 몇칸인지 보기
    [SerializeField] private int cellsHigh = 5;
    [SerializeField] private int corridorWidth = 2;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform goal;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private FlowField flowField;
    [SerializeField] private int roomCount = 3;
    [SerializeField] private int roomsize = 3;
    [SerializeField] private int seed = 0;
    [SerializeField] List<Vector2Int> pathCells = new List<Vector2Int>();

    private bool[,] isWall;         //true면 벽, false면 길
    private int width;
    private int height;
    private bool[,] visited;            //칸을 이미 가봤는지. 칸기준

    private void Awake()
    {
        int usedSeed = seed;
        if (usedSeed == 0) usedSeed = Random.Range(1, 1000000);
        Random.InitState(usedSeed);
        Debug.Log($"미로 시드{usedSeed}");
        Generate();
        PrintMaze();
        DrawMaze();
        SetCameraBounds();
        flowField.SetArea(-width / 2, -height / 2, width, height);
        Random.InitState(System.Environment.TickCount);
    }

    private readonly Vector2Int[] directions =
    {
        Vector2Int.up, Vector2Int.down,Vector2Int.left,Vector2Int.right
    };

    private int Stride => corridorWidth + 1;
    private Vector2Int CellToTile(Vector2Int cell)
    {
        return new Vector2Int(cell.x * Stride + 1, cell.y * Stride + 1);
    }

    private void SetCameraBounds()
    {
        Vector3 min = floorTilemap.CellToWorld(new Vector3Int(-width / 2, -height / 2, 0));             //미로의 왼쪽 아래끝
        Vector3 max = floorTilemap.CellToWorld(new Vector3Int(width - width / 2, height - height / 2, 0));      //미로의 오른쪽 위끝

        cameraFollow.SetBounds(min, max);
    }

    private void Generate()
    {
        width = cellsWide * Stride + 1;
        height = cellsHigh * Stride + 1;
        isWall = new bool[width, height];
        visited = new bool[cellsWide, cellsHigh];

        for (int x = 0; x < width; x++)     //모든타일을 벽으로 일단 채움
        {
            for (int y = 0; y < height; y++)
            {
                isWall[x, y] = true;
            }
        }

        for (int cx = 0; cx < cellsWide; cx++)          //칸이 있는 자리
        {
            for (int cy = 0; cy < cellsHigh; cy++)
            {
                Vector2Int tile = CellToTile(new Vector2Int(cx, cy));
                for (int dx = 0; dx < corridorWidth; dx++)
                {
                    for (int dy = 0; dy < corridorWidth; dy++)
                    {
                        isWall[tile.x + dx, tile.y + dy] = false;
                    }
                }
            }
        }
        Carvemaze();
        CarveRooms();
        Vector2Int startCell = new Vector2Int(0, 0);
        Vector2Int goalCell = FindFarthestCell(startCell);

        startPoint.position = CellToWorld(startCell);
        goal.position = CellToWorld(goalCell);
        pathCells = FindPath(startCell, goalCell);
        Debug.Log($"a->b 길 {pathCells.Count}칸");
    }

    private void PrintMaze()
    {
        string text = "";
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (isWall[x, y]) text += "1";
                else text += "0";
            }
            text += "\n";
        }
        Debug.Log(text);
    }

    private void Carve(Vector2Int from, Vector2Int to)          //두칸의 번호
    {
        Vector2Int tile = CellToTile(from);
        Vector2Int dir = to - from;

        for (int i = 0; i < corridorWidth; i++)
        {
            if (dir.x == 1) isWall[tile.x + corridorWidth, tile.y + i] = false;
            if (dir.x == -1) isWall[tile.x - 1, tile.y + i] = false;
            if (dir.y == 1) isWall[tile.x + i, tile.y + corridorWidth] = false;
            if (dir.y == -1) isWall[tile.x + i, tile.y - 1] = false;
        }
    }

    private void CarveRoom(Vector2Int bottomLeftCell, int sizeInCells)
    {
        Vector2Int from = CellToTile(bottomLeftCell);
        int tiles = sizeInCells * Stride - 1;

        for (int x = from.x; x < from.x + tiles; x++)
        {
            for (int y = from.y; y < from.y + tiles; y++)
            {
                isWall[x, y] = false;
            }
        }
    }

    private void CarveRooms()
    {
        for(int i = 0; i<roomCount; i++)
        {
            int x = Random.Range(0, cellsWide - roomsize + 1);
            int y = Random.Range(0, cellsHigh - roomsize + 1);

            if(x<2 && y<2) continue;
            CarveRoom(new Vector2Int(x, y), roomsize);
        }
    }

    private bool IsOpenBetween(Vector2Int a, Vector2Int b)
    {
        Vector2Int tile = CellToTile(a);
        Vector2Int dir = b - a;

        if (dir.x == 1) return !isWall[tile.x + corridorWidth, tile.y];
        if (dir.x == -1) return !isWall[tile.x - 1, tile.y];
        if (dir.y == 1) return !isWall[tile.x, tile.y + corridorWidth];
        return !isWall[tile.x, tile.y - 1];
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int next = cell + directions[i];     //지금 칸에서 한칸 이동한 이웃칸의 번호

            if (next.x < 0 || next.x >= cellsWide) continue;
            if (next.y < 0 || next.y >= cellsHigh) continue;
            if (visited[next.x, next.y]) continue;

            result.Add(next);
        }
        return result;
    }

    private void Carvemaze()
    {
        Stack<Vector2Int> path = new Stack<Vector2Int>();       //발자국 노트
        Vector2Int start = new Vector2Int(0, 0);
        visited[start.x, start.y] = true;                       //출발 칸을 가봤다고 표시함
        path.Push(start);

        while (path.Count > 0)
        {
            Vector2Int current = path.Peek();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count == 0)
            {
                path.Pop();
                continue;
            }

            Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
            Carve(current, next);
            visited[next.x, next.y] = true;
            path.Push(next);
        }
    }

    private void DrawMaze()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        int offsetX = width / 2;
        int offsetY = height / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cell = new Vector3Int(x - offsetX, y - offsetY, 0);
                if (isWall[x, y]) wallTilemap.SetTile(cell, wallTile);
                else floorTilemap.SetTile(cell, floorTile);
            }
        }
    }

    private Vector2Int FindFarthestCell(Vector2Int start)
    {
        int[,] distance = new int[cellsWide, cellsHigh];
        for (int x = 0; x < cellsWide; x++)
        {
            for (int y = 0; y < cellsHigh; y++)
            {
                distance[x, y] = -1;
            }
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        distance[start.x, start.y] = 0;
        queue.Enqueue(start);

        Vector2Int farthest = start;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (distance[current.x, current.y] > distance[farthest.x, farthest.y])
            {
                farthest = current;
            }

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2Int next = current + directions[i];

                if (next.x < 0 || next.x >= cellsWide) continue;
                if (next.y < 0 || next.y >= cellsHigh) continue;
                if (distance[next.x, next.y] != -1) continue;
                if (!IsOpenBetween(current, next)) continue;

                distance[next.x, next.y] = distance[current.x, current.y] + 1;
                queue.Enqueue(next);
            }
        }
        return farthest;
    }

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int goalCell)
    {
        Vector2Int[,] cameFrom = new Vector2Int[cellsWide, cellsHigh];      //어느 칸에서 왔는지 적어둠
        bool[,] seen = new bool[cellsWide, cellsHigh];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        seen[start.x, start.y] = true;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == goalCell) break;

            for(int i = 0; i<directions.Length; i++)
            {
                Vector2Int next = current + directions[i];
                if (next.x <0 || next.x >= cellsWide) continue;
                if(next.y <0 || next.y>= cellsHigh) continue;
                if (seen[next.x, next.y]) continue;
                if(!IsOpenBetween(current, next)) continue;

                seen[next.x, next.y] = true;
                cameFrom[next.x, next.y] = current;
                queue.Enqueue(next);
            }
        }
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int step = goalCell;
        while(step != start)
        {
            result.Add(step);
            step = cameFrom[step.x, step.y];
        }

        result.Add(start);
        result.Reverse();
        return result;

    }
    private Vector3 CellToWorld(Vector2Int cell)
    {
        Vector2Int tile = CellToTile(cell);
        Vector3Int first = new Vector3Int(tile.x - width / 2, tile.y - height / 2, 0);
        Vector3Int last = new Vector3Int(first.x + corridorWidth - 1, first.y + corridorWidth - 1, 0);

        Vector3 a = floorTilemap.GetCellCenterWorld(first);
        Vector3 b = floorTilemap.GetCellCenterWorld(last);
        return (a + b) / 2f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for(int i = 0; i<pathCells.Count; i++)
        {
            Gizmos.DrawSphere(CellToWorld(pathCells[i]), 0.5f);
        }
    }
}
