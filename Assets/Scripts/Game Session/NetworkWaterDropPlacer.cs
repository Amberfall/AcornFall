using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkWaterDropPlacer : MonoBehaviour
{
    [SerializeField] TileBase deadPlayerWaterDrop;

    [SerializeField] Tilemap waterTileMap;

    public void PlaceWaterDrops(object data)
    {
        if(data is List<Vector3Int>)
        {
            UnityEngine.Debug.Log("network waterdrop placer placing drops...");
            List<Vector3Int> coordList = new List<Vector3Int>();
            coordList = (List<Vector3Int>)data;
            for(int i = 0; i < coordList.Count; i++) 
            {
                Vector3Int gridPlacementCoord = new Vector3Int(0,0,0);
                gridPlacementCoord.x = coordList[i].x;
                gridPlacementCoord.y = coordList[i].y;

                waterTileMap.SetTile(gridPlacementCoord, deadPlayerWaterDrop);
            }
        }
    }
}
