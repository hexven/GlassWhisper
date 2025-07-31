using UnityEngine;
using UnityEngine.Tilemaps;

public enum FloorType
{
    Grass,
    Wood,
}

[CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile System/Tile Data")]
public class TileDatas : ScriptableObject
{
    public TileBase[] tiles;
    public AudioClip[] clip; // ���������§����� ���ç���
    public FloorType floorType;
}