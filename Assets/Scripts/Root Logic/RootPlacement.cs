using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootPlacement : MonoBehaviour
{
    //all connecting root tilebase pieces
    [SerializeField] private TileBase verticalConnectorTile;
    [SerializeField] private TileBase horizonalConnectorTile;
    [SerializeField] private TileBase leftDownConnectorTile;
    [SerializeField] private TileBase leftUpConnectorTile;
    [SerializeField] private TileBase rightUpConnectorTile;
    [SerializeField] private TileBase rightDownConnectorTile;

    //all root tip tilebase pieces
    [SerializeField] private TileBase leftTipTile;
    [SerializeField] private TileBase rightTipTile;
    [SerializeField] private TileBase upTipTile;
    [SerializeField] private TileBase downTipTile;

    //reference to the tile the player is now moving to. This should always be a tip piece
    private Tile currentTile;

    //reference to the tile the player last resided in. This one needs to be changed from tip to connector
    private Tile prevTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
