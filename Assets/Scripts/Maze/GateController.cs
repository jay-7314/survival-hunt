using UnityEngine;
using UnityEngine.Tilemaps;

public class GateController : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase gateTile;
    [SerializeField] private FlowField flowField;
    [SerializeField] private Transform marker;
    [SerializeField]
    private Vector3Int[] offsets =          //어느칸을 막을지
    {
        new Vector3Int(0,0,0),
        new Vector3Int(1,0,0)
    };

    private Vector3Int[] closedCells;       //닫을때 실제로 칠할곳

    public bool IsOpen { get; private set; } = true;

    [ContextMenu("Close")]
    public void Close()
    {
        Vector3Int origin = wallTilemap.WorldToCell(marker.position);
        closedCells = new Vector3Int[offsets.Length];

        for(int i = 0; i<offsets.Length; i++)
        {
            closedCells[i] = origin+offsets[i];
            wallTilemap.SetTile(closedCells[i], gateTile);
        }

        IsOpen = false;
        flowField.MarkDirty();
    }

    [ContextMenu("Open")]
    public void Open()
    {
        if (closedCells == null) return;

        for(int i = 0; i<closedCells.Length; i++)
        {
            wallTilemap.SetTile(closedCells[i], null);
        }

        IsOpen = true;
        flowField.MarkDirty();
    }
}
