using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GateController : MonoBehaviour
{
    private Tilemap wallTilemap;
    private TileBase gateTile;
    private FlowField flowField;
    private List<Vector3Int> cells = new List<Vector3Int>();       // 막을 칸들
    private MinimapWallBuilder minimap;

    public bool IsOpen { get; private set; } = true;

    public void Init(Tilemap wall, TileBase tile, FlowField field, List<Vector3Int> gateCells)
    {
        wallTilemap = wall;
        gateTile = tile;
        flowField = field;
        cells = gateCells;
        minimap = FindFirstObjectByType<MinimapWallBuilder>();      //프리팹은 씬 물건을 들고 있을수 없어서 생성할떄 찾게함.
    }

    [ContextMenu("Close")]
    public void Close()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            wallTilemap.SetTile(cells[i], gateTile);
        }

        IsOpen = false;
        flowField.MarkDirty();
        if (minimap != null) minimap.Build();           //타일맵의 현재 모양으로 미니맵 벽을 전부 다시 그림.
    }

    [ContextMenu("Open")]
    public void Open()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            wallTilemap.SetTile(cells[i], null);
        }

        IsOpen = true;
        flowField.MarkDirty();
        if(minimap!= null) minimap.Build();
    }
}