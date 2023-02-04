using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private RootPlacement playerRoot;

    [SerializeField]float smoothTime = 1f;
    [SerializeField] int verticalOffset = 3;

    Vector3 currentVelocity;
    private void OnEnable()
    {
        playerRoot = FindObjectOfType<RootPlacement>();
        currentVelocity = new Vector3(0, 0, 0);
    }

    void Update()
    {
        Vector3 targetCoord = new Vector3(0,0,-10);
        targetCoord.y = playerRoot.currentTileCoord.y - verticalOffset;
        //transform.position = Vector3.Lerp(transform.position, targetCoord, Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, targetCoord, ref currentVelocity, smoothTime);
    }
}
