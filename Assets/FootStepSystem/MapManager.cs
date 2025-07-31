using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileDatas> tileDatas;
    private Dictionary<TileBase, TileDatas> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileDatas>();
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                if (tile != null)
                {
                    dataFromTiles.Add(tile, tileData);
                }
            }
        }
    }

    // ���ʹ����: �֧ FloorType �ͧ��鹻Ѩ�غѹ
    public FloorType GetCurrentFloorType(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile != null && dataFromTiles.ContainsKey(tile))
        {
            return dataFromTiles[tile].floorType;
        }

        return FloorType.Grass; // default value
    }

    // ���ʹ����: �֧������ TileDatas ������
    public TileDatas GetCurrentTileData(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile != null && dataFromTiles.ContainsKey(tile))
        {
            return dataFromTiles[tile];
        }

        return null;
    }

    // ���ʹ����: ��Ǩ�ͺ����� tile �����˹觹���������
    public bool HasTileAtPosition(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);
        return tile != null && dataFromTiles.ContainsKey(tile);
    }

    // ���ʹ����: �֧������§�������ͧ��鹻Ѩ�غѹ
    public AudioClip[] GetCurrentFloorClips(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);
        if (tile != null && dataFromTiles.ContainsKey(tile))
        {
            return dataFromTiles[tile].clip;
        }
        return null;
    }

    // �����ʹ���������� backward compatibility
    public AudioClip GetCurrentFloorClip(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);
        if (tile != null && dataFromTiles.ContainsKey(tile))
        {
            var clips = dataFromTiles[tile].clip;
            if (clips != null && clips.Length > 0)
            {
                int index = Random.Range(0, clips.Length);
                return clips[index];
            }
        }
        return null;
    }

    // ���ʹ����: �֧��ª��� FloorType �����������
    public List<FloorType> GetAllFloorTypes()
    {
        List<FloorType> floorTypes = new List<FloorType>();
        foreach (var tileData in tileDatas)
        {
            if (!floorTypes.Contains(tileData.floorType))
            {
                floorTypes.Add(tileData.floorType);
            }
        }
        return floorTypes;
    }
}