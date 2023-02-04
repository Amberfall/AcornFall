using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private RootPlacement playerRoot;
    // Start is called before the first frame update
    private void OnEnable()
    {
        playerRoot = FindObjectOfType<RootPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCoord = new Vector3(0,0,-10);
        targetCoord.y = playerRoot.currentTileCoord.y;
        transform.position = Vector3.Lerp(transform.position, targetCoord, Time.deltaTime);
    }
}
