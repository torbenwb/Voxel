using System.Collections.Generic;
using UnityEngine;

// Chunks help us break apart the world so we can dynamically load in the environment.
// Each chunk will be loaded as a single mesh to reduce the number of meshes and colliders in the scene.
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
  public const int width = 16;
  public const int height = 64;
  public Vector2Int chunkCell;

  static Dictionary<Vector2Int, Chunk> allChunks = new Dictionary<Vector2Int, Chunk>();

  // Blocks have a buffer on the x and z axes for calculating the mesh.
  public VoxelType[,,] blocks = new VoxelType[width, height, width];
  public static Dictionary<Vector2Int, Dictionary<Vector3Int, VoxelType>> deltas = new Dictionary<Vector2Int, Dictionary<Vector3Int, VoxelType>>();

  private void Start()
  {
    
    chunkCell = new Vector2Int(
      Mathf.RoundToInt(transform.position.x / width),
      Mathf.RoundToInt(transform.position.z / width)
    );
    Debug.Log(chunkCell);
    allChunks.Add(chunkCell, this);
    deltas.Add(chunkCell, new Dictionary<Vector3Int, VoxelType>());

    Test();
    BuildMesh();   
  }

  public void Test(){

    for(int x = 0; x < width; x++)
      for(int z = 0; z < width; z++)
        blocks[x,1,z] = VoxelType.Dirt;
  }

  public static Vector2Int GetChunkCell(Vector3Int voxelCoordinates){
    Vector2Int cell = Vector2Int.zero;
    cell.x = voxelCoordinates.x;
    cell.y = voxelCoordinates.z;
    cell.x /= width;
    cell.y /= width;
    if (voxelCoordinates.x < 0) cell.x -= 1;
    if (voxelCoordinates.z < 0) cell.y -= 1;
    return cell;
  }

  // Add to deltas dont do what you are doing
  public static void SetVoxelType(int x, int y, int z, VoxelType newVoxelType){
    Vector2Int chunkCell = GetChunkCell(new Vector3Int(x,y,z));
    x -= (chunkCell.x * width);
    z -= (chunkCell.y * width);
    Vector3Int localVoxel = new Vector3Int(x,y,z);
    if (deltas[chunkCell].ContainsKey(localVoxel)) deltas[chunkCell][localVoxel] = newVoxelType;
    else deltas[chunkCell].Add(localVoxel, newVoxelType);

    allChunks[chunkCell].BuildMesh();
  }
  static int failedToFind = 0;
  public static VoxelType GetVoxelType(int x, int y, int z){
    //Debug.Log($"Get Voxel Type: {x},{y},{z}");
    Vector2Int chunkCell = GetChunkCell(new Vector3Int(x,y,z));
    //Debug.Log($"get chunk cell: {chunkCell}");
    x -= (chunkCell.x * width);
    z -= (chunkCell.y * width);
    Vector3Int localVoxel = new Vector3Int(x,y,z);
    if (!allChunks.ContainsKey(chunkCell)) {
      
      return VoxelType.Air;
    }
    if (deltas[chunkCell].ContainsKey(localVoxel)) {
      
      return deltas[chunkCell][localVoxel];
    }
    else{
      
      return allChunks[chunkCell].blocks[x,y,z];
    }
  }

  public void BuildMesh()
  {
    Mesh mesh = new Mesh();
    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    for (int x = 0; x < width; x++)
      for (int z = 0; z < width; z++)
        for (int y = 0; y < height; y++)
        {
          
          if (GetVoxelType(x + chunkCell.x * width,y,z + chunkCell.y * width) == VoxelType.Air) {
            continue;
          }
          Vector3 blockPos = new Vector3(x - 1, y, z - 1);

          //blockPos += Vector3.one * 0.5f;
          blockPos.x += 0.5f;
          blockPos.z += 0.5f;
          blockPos.y -= 0.5f;

          int faceCount = 0;
          // If no land above, build top face.
          if (y < height - 1 && GetVoxelType(x + chunkCell.x * width,y + 1, z + chunkCell.y * width) == VoxelType.Air)
          {
            foreach (Vector3 v in Voxel.topVerts) verts.Add(blockPos + v);
            faceCount++;
            uvs.AddRange(Voxel.voxels[GetVoxelType(x + chunkCell.x * width, y, z + chunkCell.y * width)].topTexture.GetUVs());
          }
          // If no land below, build bottom face.
          if (y > 0 && GetVoxelType(x + chunkCell.x * width, y - 1, z + chunkCell.y * width) == VoxelType.Air)
          {
            foreach (Vector3 v in Voxel.bottomVerts) verts.Add(blockPos + v);
            faceCount++;
            uvs.AddRange(Voxel.voxels[GetVoxelType(x + chunkCell.x * width, y, z + chunkCell.y * width)].bottomTexture.GetUVs());
          }
          // If no land in front, build front face.
          if (GetVoxelType(x + chunkCell.x * width, y, z - 1 + chunkCell.y * width) == VoxelType.Air)
          {
            foreach (Vector3 v in Voxel.frontVerts) verts.Add(blockPos + v);
            faceCount++;
            uvs.AddRange(Voxel.voxels[GetVoxelType(x + chunkCell.x * width, y, z + chunkCell.y * width)].sideTexture.GetUVs());
          }
          // If no land to right, build right face.
          if (GetVoxelType(x + 1 + chunkCell.x * width, y, z + chunkCell.y * width) == VoxelType.Air)
          {
            foreach (Vector3 v in Voxel.rightVerts) verts.Add(blockPos + v);
            faceCount++;
            uvs.AddRange(Voxel.voxels[GetVoxelType(x + chunkCell.x * width, y, z + chunkCell.y * width)].sideTexture.GetUVs());
          }
          // If no land behind, build back face.
          if (GetVoxelType(x + chunkCell.x * width, y, z + 1 + chunkCell.y * width) == VoxelType.Air)
          {
            foreach (Vector3 v in Voxel.backVerts) verts.Add(blockPos + v);
            faceCount++;
            uvs.AddRange(Voxel.voxels[GetVoxelType(x + chunkCell.x * width, y, z + chunkCell.y * width)].sideTexture.GetUVs());
          }
          // If no land to left, build left face.
          if (GetVoxelType(x - 1 + chunkCell.x * width, y, z + chunkCell.y * width) == VoxelType.Air)
          {
            foreach (Vector3 v in Voxel.leftVerts) verts.Add(blockPos + v);
            faceCount++;
            uvs.AddRange(Voxel.voxels[GetVoxelType(x + chunkCell.x * width, y, z + chunkCell.y * width)].sideTexture.GetUVs());
          }
          int tl = verts.Count - 4 * faceCount;
          for (int i = 0; i < faceCount; i++)
          {
            tris.AddRange(new int[] {
              tl + i * 4,
              tl + i * 4 + 1,
              tl + i * 4 + 2,
              tl + i * 4,
              tl + i * 4 + 2,
              tl + i * 4 + 3
            });
          }
        }
    mesh.vertices = verts.ToArray();
    mesh.triangles = tris.ToArray();
    mesh.uv = uvs.ToArray();

    mesh.RecalculateNormals();
    GetComponent<MeshFilter>().mesh = mesh;
    GetComponent<MeshCollider>().sharedMesh = mesh;
  }
}
