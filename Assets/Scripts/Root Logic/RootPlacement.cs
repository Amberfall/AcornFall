using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootPlacement : MonoBehaviour
{
    /***** TILEMAP AND TILEBASE PIECES ****/

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

    //reference to the root tilemap
    [SerializeField] private Tilemap rootTilemap;

    
    /*****  MOVEMENT VARIABLES  ******/
    
    //reference to the tile the player is now moving to. This should always be a tip piece
    private Vector3Int currentTileCoord = new Vector3Int(0, -4, 0);

    //reference to the tile the player last resided in. This one needs to be changed from tip to connector
    private Vector3Int prevTileCoord;

    //how many seconds until the player moves a tile
    [SerializeField] private float speed = 1f;
    private float timeUntilMovement;

    DIRECTION currentDirection = DIRECTION.DOWN;
    DIRECTION prevDirection = DIRECTION.DOWN;

    
    /***** ENUM FOR MOVEMENT LOGIC *****/
    public enum DIRECTION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    

    // Start is called before the first frame update
    void Start()
    {
        rootTilemap.SetTile(currentTileCoord, downTipTile);
        timeUntilMovement = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    private void Move()
    {
        if (timeUntilMovement <= 0)
        {
            prevTileCoord = currentTileCoord;

            switch (currentDirection)
            {
                case DIRECTION.UP:
                    
                    currentTileCoord += new Vector3Int(0, 1, 0);

                    //set newest tile
                    rootTilemap.SetTile(currentTileCoord, upTipTile);

                    //change previous tile to correct connector piece
                    switch(prevDirection)
                    {
                        case DIRECTION.DOWN:
                            //this shouldn't happen. you cannot go this way.
                            break;
                        case DIRECTION.UP:
                            rootTilemap.SetTile(prevTileCoord, verticalConnectorTile);
                            break;
                        case DIRECTION.LEFT:
                            rootTilemap.SetTile(prevTileCoord, leftUpConnectorTile);
                            break;
                        case DIRECTION.RIGHT:
                            rootTilemap.SetTile(prevTileCoord, rightUpConnectorTile);
                            break;

                    }

                    timeUntilMovement = speed;
                    prevDirection = currentDirection;
                    break;

                case DIRECTION.DOWN:

                    currentTileCoord += new Vector3Int(0, -1, 0);

                    rootTilemap.SetTile(currentTileCoord, downTipTile);
                    
                    //change previous tile to correct connector piece
                    switch (prevDirection)
                    {
                        case DIRECTION.DOWN:
                            rootTilemap.SetTile(prevTileCoord, verticalConnectorTile);
                            break;
                        case DIRECTION.UP:
                            //this shouldn't happen. you cannot go this way.
                            break;
                        case DIRECTION.LEFT:
                            rootTilemap.SetTile(prevTileCoord, leftDownConnectorTile);
                            break;
                        case DIRECTION.RIGHT:
                            rootTilemap.SetTile(prevTileCoord, rightDownConnectorTile);
                            break;

                    }

                    timeUntilMovement = speed;
                    prevDirection = currentDirection;
                    break;
                case DIRECTION.LEFT:

                    currentTileCoord += new Vector3Int(-1, 0, 0);

                    rootTilemap.SetTile(currentTileCoord, leftTipTile);

                    //change previous tile to correct connector piece
                    switch (prevDirection)
                    {
                        case DIRECTION.DOWN:
                            rootTilemap.SetTile(prevTileCoord, rightUpConnectorTile);
                            break;
                        case DIRECTION.UP:
                            rootTilemap.SetTile(prevTileCoord, rightDownConnectorTile);
                            break;
                        case DIRECTION.LEFT:
                            rootTilemap.SetTile(prevTileCoord, horizonalConnectorTile);
                            break;
                        case DIRECTION.RIGHT:
                            //this shouldn't happen. you cannot go this way.
                            break;

                    }

                    timeUntilMovement = speed;
                    prevDirection = currentDirection;
                    break;
                case DIRECTION.RIGHT:

                    currentTileCoord += new Vector3Int(1, 0, 0);

                    rootTilemap.SetTile(currentTileCoord, rightTipTile);

                    //change previous tile to correct connector piece
                    switch (prevDirection)
                    {
                        case DIRECTION.DOWN:
                            rootTilemap.SetTile(prevTileCoord, leftUpConnectorTile);
                            break;
                        case DIRECTION.UP:
                            rootTilemap.SetTile(prevTileCoord, leftDownConnectorTile);
                            break;
                        case DIRECTION.LEFT:
                            //this shouldn't happen. you cannot go this way.
                            break;
                        case DIRECTION.RIGHT:
                            rootTilemap.SetTile(prevTileCoord, horizonalConnectorTile);
                            break;

                    }

                    timeUntilMovement = speed;
                    prevDirection = currentDirection;
                    break;
            }
        }
        else
        {
            timeUntilMovement -= Time.deltaTime;
        }

    }
}
