using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlowField : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Transform player;
    [SerializeField] private int minX = -12;
    [SerializeField] private int minY = -7;
    [SerializeField] private int width = 24;
    [SerializeField] private int height = 14;


    private int[,] distance;

    private void Awake()
    {
        distance = new int[width, height];
    }

    private void Update()
    {
        Rebuild(player.position);
    }

    private static readonly Vector2Int[] Directions =
    {
        new Vector2Int (1,0),
        new Vector2Int (-1,0),
        new Vector2Int (0,1),
        new Vector2Int (0,-1)
    };

    private Vector2Int WorldToGrid(Vector2 worldPos)
    {
        Vector3Int cell = wallTilemap.WorldToCell(worldPos);
        return new Vector2Int(cell.x - minX, cell.y - minY);
    }

    private bool InBounds(Vector2Int g)
    {
        return g.x >= 0 && g.x < width && g.y >= 0 && g.y < height;
    }

    private void ResetDistance()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                distance[x, y] = -1;
            }
        }
    }

    private bool IsWall(Vector2Int g)
    {
        Vector3Int cell = new Vector3Int(g.x + minX, g.y + minY, 0);
        return wallTilemap.HasTile(cell);
    }

    public void Rebuild(Vector2 playerPos)              //playerPos => 플레이어의 월드 좌표
    {
        ResetDistance();

        Vector2Int start = WorldToGrid(playerPos);          //플레이어의 서있는 좌표 구하기
        if (!InBounds(start)) return;

        distance[start.x, start.y] = 0;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();       //지금 이웃을 살필 칸

            for(int i = 0; i<Directions.Length; i++)        //Directions의 네 방향을 차례로 돈다.
            {
                Vector2Int next = current + Directions[i];

                if(!InBounds(next)) continue;               //맵 밖이면 건너뜀
                if(IsWall(next)) continue;                      //벽이면 건너뜀
                if (distance[next.x, next.y] != -1) continue;       //이미 번호가 붙은칸이면 건너뜀, 안전장치임

                distance[next.x, next.y] = distance[current.x, current.y] + 1;
                queue.Enqueue(next);
            }
        }
    }

    //적의 월드좌표를 받아서 걸어가야 할 방향을 돌려줌
    public Vector2 GetDirection(Vector2 worldPos)
    {
        Vector2Int g = WorldToGrid(worldPos);           //적이 서있는칸

        if(!InBounds(g) || distance[g.x, g.y] <0) return Vector2 .zero;     //맵 밖이거나, 벽위이면 방향없음을 돌려줌
        int best = distance[g.x, g.y];              //지금까지 본 가장 작은 거리
        Vector2Int bestCell = g;

        for(int i = 0; i< Directions.Length; i++)
        {
            Vector2Int next = g+ Directions[i];
            if(!InBounds(next)) continue;

            int d = distance[next.x, next.y];
            if(d<0) continue;

            if (d < best)
            {
                best = d;
                bestCell = next;
            }
        }
        if (bestCell == g) return Vector2.zero;

        Vector3 center = wallTilemap.GetCellCenterWorld(new Vector3Int(bestCell.x + minX, bestCell.y + minY, 0));
        return ((Vector2)center-worldPos).normalized;
    }

    //그리기 함수
    private void OnDrawGizmos()
    {
        if (distance == null) return;

        for(int y = 0; y<height; y++)
        {
            for(int x = 0; x <width; x++)
            {
                int d = distance[x, y];
                if(d<0) continue;               //모든칸을 돌고, 벽이거나 닿지 못한 칸은 건너뜀

                Vector3 center = wallTilemap.GetCellCenterWorld(new Vector3Int(x + minX, y + minY, 0));         //칸 중심의 월드좌표
                Gizmos.color = Color.Lerp(Color.green, Color.red, d / 40f);         //가까우면 초록, 멀면 빨강
                Gizmos.DrawCube(center, Vector3.one * 0.3f);
            }
        }
    }
}
