using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
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

    //references to the rock and water tilemaps for collision checks
    [SerializeField] private Tilemap rockTileMap;
    [SerializeField] private Tilemap waterTileMap;

    //animated root tip
    [SerializeField] private GameObject rootTip;

    
    /*****  MOVEMENT VARIABLES  ******/
    
    //reference to the tile the player is now moving to. This should always be a tip piece
    public Vector3Int currentTileCoord = new Vector3Int(0, 0, 0);

    //reference to the tile the player last resided in. This one needs to be changed from tip to connector
    private Vector3Int prevTileCoord;

    //how many seconds until the player moves a tile
    public float speed = 1f;
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

    /***** WATER GUAGE *****/
    [SerializeField] int waterRemaining = 100;
    [SerializeField] int waterCostPerTile = 1;
    [SerializeField] int waterGainedFromDeposit = 25;

    /**** EVENTS SYSTEM ******/
    [Header("Events")]
    public GameEvent onRootDepthChanged;
    public GameEvent onWaterRemainingChanged;
    public GameEvent onPlayerDied;
    public GameEvent onPlayerWon;
    public GameEvent onDrankWaterDeposit;
    public GameEvent onHitRock;

    void Start()
    {
        rootTilemap.SetTile(currentTileCoord, downTipTile);
        timeUntilMovement = speed;
        rootTip.transform.position = rootTilemap.GetCellCenterWorld(currentTileCoord);
    }

    void Update()
    {
        checkForUserInput();
        moveTip();
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
                    
                    handleCollisions(currentTileCoord);

                    //set newest tile
                    rootTilemap.SetTile(currentTileCoord, upTipTile);

                    onRootDepthChanged.Raise(-currentTileCoord.y);

                    //change previous tile to correct connector piece
                    switch (prevDirection)
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

                    handleCollisions(currentTileCoord);

                    rootTilemap.SetTile(currentTileCoord, downTipTile);

                    onRootDepthChanged.Raise(-currentTileCoord.y);

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

                    handleCollisions(currentTileCoord);

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

                    handleCollisions(currentTileCoord);

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
            waterRemaining -= waterCostPerTile;
            onWaterRemainingChanged.Raise(waterRemaining);
            checkWaterGuage();
        }
        else
        {
            timeUntilMovement -= Time.deltaTime;
        }

    }

    /****** COLLISION DETECTION METHODS *******/

    private bool rootAlreadyExists(Vector3Int tileToCheck)
    {
        return rootTilemap.GetTile(tileToCheck) != null;
    }
    private bool ranIntoRock(Vector3Int tileToCheck)
    {
        return rockTileMap.GetTile(tileToCheck) != null;
    }
    private bool foundWater(Vector3Int tileToCheck)
    {
        return waterTileMap.GetTile(tileToCheck) != null;
    }
    private void handleCollisions(Vector3Int tileToCheck)
    {
        if(tileToCheck.x < -10 || tileToCheck.x > 9 || tileToCheck.y > 0)
        {
            UnityEngine.Debug.Log("Out of bounds. Believe it or not, straight to jail!");
            onPlayerDied.Raise(-currentTileCoord.y);
        }
        else if (rootAlreadyExists(tileToCheck))
        {
            UnityEngine.Debug.Log("Womp Womp. You Lose. Ran into yoself");
            onPlayerDied.Raise(-currentTileCoord.y);
        }
        else if(ranIntoRock(tileToCheck)) 
        {
            UnityEngine.Debug.Log("OUCH... Hit a rock. You lose.");
            onHitRock.Raise(default);
            onPlayerDied.Raise(-currentTileCoord.y);
        }
        else if(foundWater(tileToCheck)) 
        {
            waterTileMap.SetTile(tileToCheck, null);
            waterRemaining += waterGainedFromDeposit;
            onWaterRemainingChanged.Raise(waterRemaining);
            onDrankWaterDeposit.Raise(default);
        }
    }

    private void checkWaterGuage()
    {
        if(waterRemaining <= 0)
        {
            UnityEngine.Debug.Log("sooo.. Thirsty... Can't go on... You Lose.");
            onPlayerDied.Raise(-currentTileCoord.y);
        }
    }

    /****** ROOT TIP ANIMATION  *******/

    private void moveTip()
    {
        Vector3 targetCoord;

        switch (currentDirection)
        {
            case DIRECTION.UP:
                targetCoord = rootTilemap.GetCellCenterWorld(currentTileCoord + new Vector3Int(0, 1, 0));
                rootTip.transform.position = Vector3.Lerp(rootTip.transform.position, targetCoord, speed);
                break;
            case DIRECTION.DOWN:
                targetCoord = rootTilemap.GetCellCenterWorld(currentTileCoord + new Vector3Int(0, -1, 0));
                rootTip.transform.position = Vector3.Lerp(rootTip.transform.position, targetCoord, speed);
                break;
            case DIRECTION.LEFT:
                targetCoord = rootTilemap.GetCellCenterWorld(currentTileCoord + new Vector3Int(-1, 0, 0));
                rootTip.transform.position = Vector3.Lerp(rootTip.transform.position, targetCoord, speed);
                break;
            case DIRECTION.RIGHT:
                targetCoord = rootTilemap.GetCellCenterWorld(currentTileCoord + new Vector3Int(1, 0, 0));
                rootTip.transform.position = Vector3.Lerp(rootTip.transform.position, targetCoord, speed);
                break;
        }
    }

    /***** METHOD FOR MOVEMENT INPUT *******/

    private void checkForUserInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (prevDirection != DIRECTION.DOWN)
            {
                currentDirection = DIRECTION.UP;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (prevDirection != DIRECTION.UP)
            {
                currentDirection = DIRECTION.DOWN;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (prevDirection != DIRECTION.RIGHT)
            {
                currentDirection = DIRECTION.LEFT;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (prevDirection != DIRECTION.LEFT)
            {
                currentDirection = DIRECTION.RIGHT;
            }

        }
    }
}
