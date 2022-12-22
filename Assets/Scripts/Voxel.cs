using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
  public TileTexture.Type top, side, bottom;
  public TileTexture topTexture, sideTexture, bottomTexture;
  public Voxel(TileTexture.Type tile)
  {
    top = side = bottom = tile;
    InitTextureCoordinates();
  }
  public Voxel(TileTexture.Type top, TileTexture.Type side, TileTexture.Type bottom)
  {
    this.top = top;
    this.side = side;
    this.bottom = bottom;
    InitTextureCoordinates();
  }
  void InitTextureCoordinates()
  {
    topTexture = TileTexture.tiles[top];
    sideTexture = TileTexture.tiles[side];
    bottomTexture = TileTexture.tiles[bottom];
  }
  public static Dictionary<VoxelType, Voxel> voxels = new Dictionary<VoxelType, Voxel>() {
      {VoxelType.Grass, new Voxel(TileTexture.Type.Grass, TileTexture.Type.GrassSide, TileTexture.Type.Dirt)},
      {VoxelType.Dirt, new Voxel(TileTexture.Type.Dirt)},
      {VoxelType.Stone, new Voxel(TileTexture.Type.Stone)},
      {VoxelType.Trunk, new Voxel(TileTexture.Type.TreeRings, TileTexture.Type.TreeSide, TileTexture.Type.TreeRings)},
      {VoxelType.Leaves, new Voxel(TileTexture.Type.Leaves)},
  };
  public static Vector3[] topVerts
  {
    get => new Vector3[] {
        new Vector3(0, 1, 0),
        new Vector3(0, 1, 1),
        new Vector3(1, 1, 1),
        new Vector3(1, 1, 0)
      };
  }
  public static Vector3[] bottomVerts
  {
    get => new Vector3[] {
      new Vector3(0, 0, 0),
      new Vector3(1, 0, 0),
      new Vector3(1, 0, 1),
      new Vector3(0, 0, 1)
    };
  }
  public static Vector3[] frontVerts
  {
    get => new Vector3[] {
      new Vector3(0, 0, 0),
      new Vector3(0, 1, 0),
      new Vector3(1, 1, 0),
      new Vector3(1, 0, 0)
    };
  }
  public static Vector3[] rightVerts
  {
    get => new Vector3[] {
      new Vector3(1, 0, 0),
      new Vector3(1, 1, 0),
      new Vector3(1, 1, 1),
      new Vector3(1, 0, 1)
    };
  }
  public static Vector3[] backVerts
  {
    get => new Vector3[] {
      new Vector3(1, 0, 1),
      new Vector3(1, 1, 1),
      new Vector3(0, 1, 1),
      new Vector3(0, 0, 1)
    };
  }
  public static Vector3[] leftVerts
  {
    get => new Vector3[] {
      new Vector3(0, 0, 1),
      new Vector3(0, 1, 1),
      new Vector3(0, 1, 0),
      new Vector3(0, 0, 0)
    };
  }
}

public enum VoxelType
{
  Air = 0,
  Dirt,
  Grass,
  Stone,
  Trunk,
  Leaves
}
