using System.Collections.Generic;
using UnityEngine;

public class TileTexture
{
  int xPos, yPos;
  const float resolution = 16f;
  const float buffer = .001f;

  private Vector2[] _uvs;

  public TileTexture(int xPos, int yPos)
  {
    this.xPos = xPos;
    this.yPos = yPos;
    _uvs = new Vector2[]
    {
      new Vector2(xPos / resolution + buffer, yPos / resolution + buffer),
      new Vector2(xPos / resolution + buffer, (yPos + 1) / resolution - buffer),
      new Vector2((xPos + 1) / resolution - buffer, (yPos + 1) / resolution - buffer),
      new Vector2((xPos + 1) / resolution - buffer, yPos / resolution+ buffer),
    };
  }

  public Vector2[] GetUVs()
  {
    return _uvs;
  }

  public static Dictionary<TileTexture.Type, TileTexture> tiles = new Dictionary<TileTexture.Type, TileTexture>()
  {
    {TileTexture.Type.Dirt, new TileTexture(0,0)},
    {TileTexture.Type.Grass, new TileTexture(1,0)},
    {TileTexture.Type.GrassSide, new TileTexture(0,1)},
    {TileTexture.Type.Stone, new TileTexture(0,2)},
    {TileTexture.Type.TreeSide, new TileTexture(0,4)},
    {TileTexture.Type.TreeRings, new TileTexture(0,3)},
    {TileTexture.Type.Leaves, new TileTexture(0,5)},
  };

  public enum Type { Dirt, Grass, GrassSide, Stone, TreeSide, TreeRings, Leaves };
}
