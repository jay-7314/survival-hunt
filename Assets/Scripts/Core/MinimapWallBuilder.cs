using UnityEngine;
using UnityEngine.Tilemaps;

public class MinimapWallBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap walltilemap;
    [SerializeField] private TileBase walltile;

    private Tilemap minimapTilemap;

    private void Awake()
    {
        minimapTilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        Build();
    }

    public void Build()
    {
        minimapTilemap.ClearAllTiles();     //이전에 찍어둔 벽은 모두 지움
        foreach(Vector3Int cell in walltilemap.cellBounds.allPositionsWithin)       //타일이 하나라도 칠해진곳을 모두 감싸는 상자, 
                                                                                    //allPositionsWithin 모든칸을 하나하나 알려줌
        {
            if (walltilemap.HasTile(cell))
            {
                minimapTilemap.SetTile(cell, walltile);
            }
        }
    }
}
