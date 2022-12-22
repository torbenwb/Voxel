using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCursor : MonoBehaviour
{
    public Vector3Int targetBlock;
    public Vector3Int addBlock;
    public Vector2Int targetChunkCell;
    public VoxelType setVoxelType = VoxelType.Dirt;
    public float targetUpdateInterval = 0.5f;


    KeyCode[] keyMap = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5};
    VoxelType[] voxelTypes = {VoxelType.Dirt, VoxelType.Grass, VoxelType.Stone, VoxelType.Trunk, VoxelType.Leaves};

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TargetUpdate());
    }

    IEnumerator TargetUpdate(){
        while (true){
            if (FindTargetBlock()) yield return new WaitForSeconds(targetUpdateInterval);
            else yield return null;
        }
    }

    bool FindTargetBlock(){
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            Chunk chunk = hit.collider.gameObject.GetComponent<Chunk>();

            Vector3 position = hit.point;
            position -= hit.normal * 0.5f;
            Vector3Int tempTarget = new Vector3Int(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y),
                Mathf.RoundToInt(position.z)
            );

            if (Chunk.GetVoxelType(tempTarget.x, tempTarget.y, tempTarget.z) == VoxelType.Air) return false;

            targetBlock = tempTarget;

            Vector3Int newAddBlockCoordinates = targetBlock;
            if (hit.normal.x > 0) newAddBlockCoordinates.x += 1;
            else if (hit.normal.x < 0) newAddBlockCoordinates.x -= 1;
            else if (hit.normal.y > 0) newAddBlockCoordinates.y += 1;
            else if (hit.normal.y < 0) newAddBlockCoordinates.y -= 1;
            else if (hit.normal.z > 0) newAddBlockCoordinates.z += 1;
            else if (hit.normal.z < 0) newAddBlockCoordinates.z -= 1;
            SetAddBlock(newAddBlockCoordinates);

            transform.position = targetBlock;
            transform.rotation = Quaternion.LookRotation(addBlock - targetBlock);
            
        }
        return true;
    }

    void SetAddBlock(Vector3Int newCoordinates){
        if (newCoordinates == addBlock) return;
        addBlock = newCoordinates;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < keyMap.Length; i++){
            if (Input.GetKeyDown(keyMap[i])) setVoxelType = voxelTypes[i];
        }
    
        if (Input.GetMouseButtonDown(0)){
            Chunk.SetVoxelType(addBlock.x, addBlock.y, addBlock.z, setVoxelType);
        }

        if (Input.GetMouseButtonDown(1)){
            Chunk.SetVoxelType(targetBlock.x, targetBlock.y, targetBlock.z, VoxelType.Air);
        }

        
    }
}
