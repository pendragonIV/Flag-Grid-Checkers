using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
    Left,
    Right,
    Down,
    LeftDown,
    RightDown,
    LeftTop,
    RightTop
}

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField] 
    private List<Vector3Int> tileLocations = new List<Vector3Int>();
    [SerializeField]
    private Dictionary<Vector3Int, GameObject> placedCells = new Dictionary<Vector3Int, GameObject>();
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        GetMoveAbleCell();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseCellPos = tileMap.WorldToCell(mouse);
            Debug.Log(mouseCellPos);
        }
    }
    private void GetMoveAbleCell()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);
                if (tileMap.HasTile(localLocation))
                {
                    tileLocations.Add(localLocation);
                }
            }
        }
    }

    public void SetTileMap(Tilemap tilemap)
    {
        this.tileMap = tilemap;
    }
    public bool IsPlaceableArea(Vector3Int mouseCellPos)
    {
        if (tileMap.GetTile(mouseCellPos) == null)
        {
            return false;
        }
        return true;
    }

    public void SetPlacedCell(Vector3Int cellPosition, GameObject ball)
    {
        if (placedCells.ContainsKey(cellPosition))
        {
            return;
        }
        placedCells.Add(cellPosition, ball);
    }

    public void RemovePlacedCell(Vector3Int cellPosition)
    {
        if (!placedCells.ContainsKey(cellPosition))
        {
            return;
        }
        placedCells.Remove(cellPosition);
    }

    public bool IsPlacedCell(Vector3Int cellPosition)
    {
        if (placedCells.ContainsKey(cellPosition))
        {
            return true;
        }
        return false;
    }

    public bool IsFull()
    {
        if (placedCells.Count == tileLocations.Count)
        {
            return true;
        }
        return false;
    }

    #region Get

    public Vector3Int GetDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            case Direction.Down:
                return Vector3Int.down;
            case Direction.LeftDown:
                return Vector3Int.left + Vector3Int.down;
            case Direction.RightDown:
                return Vector3Int.right + Vector3Int.down;
            case Direction.LeftTop:
                return Vector3Int.left + Vector3Int.up;
            case Direction.RightTop:
                return Vector3Int.right + Vector3Int.up;
            default:
                return Vector3Int.zero;
        }
    }

    public GameObject GetPlacedCell(Vector3Int cellPosition)
    {
        if (!placedCells.ContainsKey(cellPosition))
        {
            return null;
        }
        return placedCells[cellPosition];
    }

    public List<Vector3Int>  GetCellsPosition()
    {
        return tileLocations;
    }

    public Vector3Int GetEdgeCellLeft()
    {
        Vector3Int cellPosition = tileMap.WorldToCell(tileMap.localBounds.min);
        return cellPosition;
    }
    public Vector3Int GetEdgeCellRight()
    {
        Vector3Int cellPosition = tileMap.WorldToCell(tileMap.localBounds.max);
        return cellPosition;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }
    #endregion
}
