using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAdavanced : MonoBehaviour
{


    //settings
    public float originalSpeed = 20.0f;
    public string direction; //to do: make this better(protected)
    public LayerMask pathEmptys;
    private float speed;
    private float offset = 0.35f;
    private string directionState = "";

    //save the current and the last tile standing on
    private Tile currentTile;   //newly collided
    private Tile oldTile;       //collided before
    private Tile activeTile;    //more standing on
    private Vector3 emptyPositionNorth;
    private Vector3 emptyPositionSouth;
    private Vector3 emptyPositionWest;
    private Vector3 emptyPositionEast;
    private Vector3 emptyPositionCorner = Vector3.zero;
    private GameObject CornerPathEmpty;

    //figure positioning state
    private bool isForward = false;
    private bool isBackward = false;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isAtCorner = false;



    public Tile get_activeTile()
    {
        return activeTile;
    }

    private void Start()
    {
        speed = originalSpeed;
    }


    void Update()
    {

        //movementChecker returns if its possible the wished move direction. Makes little Corrections at corners
        if(direction != "")
            moveFigure(movementChecker());
        // for testing this
       //moveFigure(direction);
    }

    private Vector3 directionToVector(string _direction)
    {
        if (direction == "left")
        {
            return (Vector3.left);
        }
        else if (direction == "right")
        {
            return (Vector3.right);
        }
        else if (direction == "forward")
        {
            return (Vector3.forward);
        }
        else if (direction == "back")
        {
            return (Vector3.back);
        }

        return (new Vector3(0,0,0));
    }

    
    //If directionState is Empty a raycast looks if there is an active PathEmpty,
    //if yes: a directionState is assigend, depending on the Userinput and the empty it's vertical or horizontal.
    //The directionStat determines the Walking and the switch directon.
    //if an Empty is reached a new directon State should be assigend
    //(if player reaches turnoff, new direction, if player is on straight road, nothing is to be done except when switch lanes) .
    private string movementChecker()
    {
        RaycastHit hitN = new RaycastHit();

        //Debug.Log(isAtCorner);
        //checkForCorner();

        //Stop at corner and center player
        if ((direction == "left" || direction == "right") && Physics.Raycast(transform.position + new Vector3(0, 0, 0.8f), Vector3.back, out hitN, 1, pathEmptys) && directionState != "")
        {
            Debug.Log("at corner");
            directionState = "";
            emptyPositionCorner = hitN.collider.transform.position;

            if (direction == "left" && transform.position.x < emptyPositionCorner.x)
            {
                transform.position = emptyPositionCorner + new Vector3(0, 0.1f, 0);
            }
            else if (direction == "right" && transform.position.x > emptyPositionCorner.x)
            {
                transform.position = emptyPositionCorner + new Vector3(0, 0.1f, 0);
            }
           
        }
        else if ((direction == "forward" || direction == "backward") && Physics.Raycast(transform.position + new Vector3(0.8f, 0, 0), Vector3.left, out hitN, 1, pathEmptys) && directionState != "")
        {
            Debug.Log("at corner");
            directionState = "";
            emptyPositionCorner = hitN.collider.transform.position;

            if (direction == "forward" && transform.position.z > emptyPositionCorner.z)
            {
                transform.position = emptyPositionCorner + new Vector3(0, 0.1f, 0);
            }
            else if (direction == "back" && transform.position.z < emptyPositionCorner.z)
            {
                transform.position = emptyPositionCorner + new Vector3(0, 0.1f, 0);
            }
        }
        //after Corner Stop center figure and get new direction state
        if (Physics.Raycast(transform.position + new Vector3(0,0,0.8f), Vector3.back, out hitN, 1, pathEmptys)  && directionState == "")
        {
            Debug.Log("Walk away from corner");
            emptyPositionCorner = hitN.collider.transform.position;
            transform.position = emptyPositionCorner + new Vector3(0, 0.1f, 0);

            if (direction == "left")
            {
                if (CornerPathEmpty.GetComponent<pathEmptySettings>().left)
                {

                    if (!CornerPathEmpty.GetComponent<pathEmptySettings>().leftSwitch)
                    {                        
                        return direction;
                    }
                    else if (CornerPathEmpty.GetComponent<pathEmptySettings>().leftSwitch)
                    {
                        return "switch";
                    }
                }
            }
            else if (direction == "right")
            {
                if (CornerPathEmpty.GetComponent<pathEmptySettings>().right)
                {
                    if (!CornerPathEmpty.GetComponent<pathEmptySettings>().rightSwitch)
                    {
                                           
                        return direction;
                    }
                    else if (CornerPathEmpty.GetComponent<pathEmptySettings>().rightSwitch)
                    {
                        return "switch";
                    }
                }
            }
            else if (direction == "forward")
            {
                if (CornerPathEmpty.GetComponent<pathEmptySettings>().forward)
                {
                    if (!CornerPathEmpty.GetComponent<pathEmptySettings>().forwardSwitch)
                    {
                        Debug.Log(direction);
                        return direction;
                    }
                    else if (CornerPathEmpty.GetComponent<pathEmptySettings>().forwardSwitch)
                    {
                        return "switch";
                    }
                }
            }
            else if (direction == "back")
            {
                if (CornerPathEmpty.GetComponent<pathEmptySettings>().back)
                {
                    if (!CornerPathEmpty.GetComponent<pathEmptySettings>().backSwitch)
                    {
                        return direction;
                    }
                    else if (CornerPathEmpty.GetComponent<pathEmptySettings>().backSwitch)
                    {
                        return "switch";
                    }

                }
            }
            Debug.Log("last return");
            return "";

        }


        if (directionState == "" && !Physics.Raycast(transform.position + new Vector3(0, 0, 0.8f), Vector3.back, out hitN, 1, pathEmptys) && (direction == "left" || direction == "right"))
        {
            sendRays();

            //Problem: Es wird kein neure direction state zugewiesen bei ecken 

            //Debug.Log(hit1.collider.tag);
            //Debug.Log(directionState);
            //Debug.Log(hit1.point);
            //Debug.Log("N: " + emptyPositionNorth + "|| S: " + emptyPositionSouth + "|| W: "+ emptyPositionWest + "|| E: "+ emptyPositionEast +"||"+ directionState);


        }
        else if
        (directionState == "" && !Physics.Raycast(transform.position + new Vector3(0.8f, 0, 0), Vector3.left, out hitN, 1, pathEmptys) && (direction == "foward" || direction == "back"))
        {
            sendRays();
        }
        /* else if (isAtCorner == true )
         {

             //to do instad of every direction us something like ..GetComponent<pathEmptySettings>().direction (GetType...)
             if(direction == "left")
             {
                 if (CornerPathEmpty.GetComponent<pathEmptySettings>().left)
                 {
                     isAtCorner = false;
                     directionState = "horizontal";
                     Debug.Log(directionState);

                     if (!CornerPathEmpty.GetComponent<pathEmptySettings>().leftSwitch)
                     {
                         //Debug.Log("Return Direction");
                         return direction;                 
                     }
                     else
                     {
                         Debug.Log("Switch");
                         return "switch";
                     }

                 }
             }
             else if(direction == "right")
             {
                 if (CornerPathEmpty.GetComponent<pathEmptySettings>().right)
                 {
                     isAtCorner = false;
                     directionState = "horizontal";
                     Debug.Log(directionState);

                     if (!CornerPathEmpty.GetComponent<pathEmptySettings>().rightSwitch)
                     {
                         //Debug.Log("Return Direction");
                         return direction;
                     }
                     else
                     {
                         Debug.Log("Switch");
                         return "switch";
                     }
                 }
             }
             else if (direction == "foward")
             {
                 if (CornerPathEmpty.GetComponent<pathEmptySettings>().forward)
                 {

                     isAtCorner = false;
                     directionState = "vertical";
                     Debug.Log(directionState);

                     if (!CornerPathEmpty.GetComponent<pathEmptySettings>().forwardSwitch)
                     {
                         //Debug.Log("Return Direction");
                         return direction;
                     }
                     else
                     {
                         Debug.Log("Switch");
                         return "switch";
                     }
                 }
             }
             else if (direction == "back")
             {
                 if (CornerPathEmpty.GetComponent<pathEmptySettings>().back)
                 {
                     isAtCorner = false;
                     directionState = "vertical";


                     if (!CornerPathEmpty.GetComponent<pathEmptySettings>().backSwitch)
                     {
                         //Debug.Log("Return Direction");
                         return direction;
                     }
                     else
                     {
                         Debug.Log("Switch");
                         return "switch";
                     }


                 }
             }


             //isAtCorner = false;
         }*/
        else if (directionState == "horizontal")
        {
            if (direction == "left" || direction == "right")
                return direction;
            else if (direction == "forward" || direction == "back")
                return "switch";
        }
        else if (directionState == "vertical")
        {
            if (direction == "forward" || direction == "back")
            {
                Debug.Log(direction);
                return direction;
            }
            else if (direction == "left" || direction == "right")
                return "switch";
        }

        

        return "";
    }

    private void sendRays()
    {

        //to do: maybe reduce to one
        RaycastHit hitN = new RaycastHit();
        RaycastHit hitS = new RaycastHit();
        RaycastHit hitW = new RaycastHit();
        RaycastHit hitE = new RaycastHit();

        emptyPositionEast = Vector3.zero;
        emptyPositionNorth = Vector3.zero;
        emptyPositionWest = Vector3.zero;
        emptyPositionSouth = Vector3.zero;

        //searches the next empties
        //North
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0.8f), Vector3.forward, out hitN, 100, pathEmptys))
        {
            emptyPositionNorth = hitN.collider.transform.position;
        }
        else
        {
            if (emptyPositionSouth != Vector3.zero)
                emptyPositionNorth = emptyPositionCorner;
            else
                emptyPositionNorth = Vector3.zero;
        }
        //South
        if (Physics.Raycast(transform.position + new Vector3(0, 0, -0.8f), Vector3.back, out hitS, 100, pathEmptys))
        {
            emptyPositionSouth = hitS.collider.transform.position;
        }
        else
        {
            if (emptyPositionNorth != Vector3.zero)
                emptyPositionSouth = emptyPositionCorner;
            else
                emptyPositionSouth = Vector3.zero;
        }
        //West
        if (Physics.Raycast(transform.position + new Vector3(-0.8f, 0, 0), Vector3.left, out hitW, 100, pathEmptys))
        {
            emptyPositionWest = hitW.collider.transform.position;
        }
        else
        {
            if (emptyPositionEast != Vector3.zero)
                emptyPositionWest = emptyPositionCorner;
            else
                emptyPositionWest = Vector3.zero;
        }
        //East
        if (Physics.Raycast(transform.position + new Vector3(0.8f, 0, 0), Vector3.right, out hitE, 100, pathEmptys))
        {
            emptyPositionEast = hitE.collider.transform.position;
        }
        else
        {
            if (emptyPositionWest != Vector3.zero)
                emptyPositionEast = emptyPositionCorner;
            else
                emptyPositionEast = Vector3.zero;
        }

      

        if (emptyPositionEast != Vector3.zero && emptyPositionWest != Vector3.zero /*&& (currentTile.left || currentTile.right)*/)
        {
            directionState = "horizontal";
        }
        else if (emptyPositionSouth != Vector3.zero && emptyPositionNorth != Vector3.zero /*&& (currentTile.forward || currentTile.back)*/)
        {
            directionState = "vertical";
        }
        Debug.Log(directionState);
        Debug.Log("Empty South:" + emptyPositionSouth + "| Empty North: " + emptyPositionNorth + "| Empty West: " + emptyPositionWest + "| Empty East: " + emptyPositionEast);
    }

   /* private string detectCorner()
    {
        //North
        if (emptyPositionCorner == emptyPositionNorth )
        {

        }
        //South
        else if (emptyPositionCorner == emptyPositionSouth)
        {

        }
        //West
        else if (emptyPositionCorner == emptyPositionWest)
        {

        }
        //East
        else if (emptyPositionCorner == emptyPositionEast)
        {

        }

        return "";
    }*/

    //checks if a corner is reached: sets the directions state to null then.
   /* private void checkForCorner()
    {
  
        if (directionState == "horizontal")
        {
            if (emptyPositionEast.x < transform.position.x)
            {
                isAtCorner = true;
                transform.position = emptyPositionEast + new Vector3(0,0.1f,0);
                directionState = "";
                //emptyPositionCorner = emptyPositionEast;
                //Debug.Log("CFC" + emptyPositionNorth + "||" + emptyPositionSouth + "||" + directionState);
            }
            else if (emptyPositionWest.x > transform.position.x)
            {
                isAtCorner = true;
                transform.position = emptyPositionWest + new Vector3(0, 0.1f, 0);
                directionState = "";
                //emptyPositionCorner = emptyPositionWest;
                //Debug.Log("CFC" + emptyPositionNorth + "||" + emptyPositionSouth + "||" + directionState);
            }

        }
        else if (directionState == "vertical")
        {
            
            if (emptyPositionNorth.z > transform.position.z)
            {
                isAtCorner = true;
                transform.position = emptyPositionNorth + new Vector3(0, 0.1f, 0);
                directionState = "";
                //emptyPositionCorner = emptyPositionNorth;
                //Debug.Log("CFC" + emptyPositionNorth + "||" + emptyPositionSouth + "||" + directionState);
            }
            else if (emptyPositionSouth.z < transform.position.z)
            {
                isAtCorner = true;
                transform.position = emptyPositionSouth + new Vector3(0, 0.1f, 0);
                directionState = "";
                //emptyPositionCorner = emptyPositionSouth;
                //Debug.Log("CFC" + emptyPositionNorth + "||" + emptyPositionSouth + "||" + directionState);
            }
        }
    }
    */
    private void moveFigure(string _direction)
    {


        Vector3 designatedPosition;
        switch (_direction)
        {
            case "forward":
                transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime, Space.World);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "back":
                transform.Translate(Vector3.back * speed * Time.smoothDeltaTime, Space.World);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "left":
                transform.Translate(Vector3.left * speed * Time.smoothDeltaTime, Space.World);
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case "right":
                transform.Translate(Vector3.right * speed * Time.smoothDeltaTime, Space.World);
                //transform.position = Vector3.MoveTowards(transform.position, designatedPosition, 7);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
           /* case "switch":
                switch (direction)
                    { 
                    case "forward":
                        if (isBackward)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }            
                        break;
                    case "back":
                        if (isForward)
                        {
                            //designatedPosition = transform.position + (Vector3.back * 7);
                            //transform.position = Vector3.MoveTowards(transform.position, designatedPosition,7);
                            //transform.Translate(Vector3.back * speed * Time.smoothDeltaTime, Space.World);
                            transform.Translate(Vector3.back * 1, Space.World);
                            //playerMovement = StartCoroutine(Move(Vector3.back*1));
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }    
                        break;
                    case "left":
                        if (isRight)
                        {
                            transform.rotation = Quaternion.Euler(0, -90, 0);
                        }
                        break;
                    case "right":
                        if (isLeft)
                        {
                            transform.rotation = Quaternion.Euler(0, 90, 0);
                        }
                        break;
                    }
            break;*/
        }
    }
    //to do: make this work
    public const float stepDuration = 0.1f;
    private Coroutine playerMovement;

    private IEnumerator Move(Vector3 directionVector)
    {
        Vector3 startPosition = transform.position;
        Vector3 destinationPosition = transform.position + directionVector;
        float t = 0.0f;

        while (t < 2.0f)
        {
            transform.position = Vector3.Lerp(startPosition, destinationPosition,1);
            t += Time.deltaTime / stepDuration;
            yield return new WaitForEndOfFrame();
        }

        transform.position = destinationPosition;

        playerMovement = null;
    }



    /*private string movementChecker()
    {
        positionChecker(currentTile);

        if(currentTile.horizontal)
        {
            if (direction == "left" || direction == "right")
                return direction;
            else if (direction == "forward" || direction == "back")
                return "switch";
        }
        else if (currentTile.vertical)
        {
            if (direction == "forward" || direction == "back")
                return direction;
            else if (direction == "left" || direction == "right")
                return "switch";
        }
        else if (currentTile.corner)
        {
            if (currentTile.left)//west
            {
                if (currentTile.forward)//north
                {
                    if(isBackward && (direction == "right" || direction == "back"))
                    {

                    }

                }
                if (currentTile.back)//south
                {

                }
            }
            else if (currentTile.right)//east
            {
                if (currentTile.forward)//north
                {

                }
                if (currentTile.back)//south
                {

                }
            }
        }


        return "";
    }*/

    //checks where, depending from the Tile, the player stands.
   /* private void positionChecker(Tile _currentTile)
    {
        isBackward = false;
        isForward = false;
        isLeft = false;
        isRight = false;

        if (_currentTile.transform.position.x < transform.position.x)
            isRight = true;
        else
            isRight = true;

        if (_currentTile.transform.position.z < transform.position.z)
            isForward = true;
        else
            isBackward = true;
        if (_currentTile.horizontal)
        {
           
        }
        else if (_currentTile.vertical)
        {
            
        }
        else if (_currentTile.corner)
        else if (_currentTile.t_crossing)
        else if (_currentTile.crossing)
        else
            Debug.Log("Unkown Tile Type");

    }*/


    /* public string movementChecker()
     {

         float  tolerance = 0.15f;


         if (currentTile.transform.position.z == oldTile.transform.position.z)
         {
             if (Math.Abs(currentTile.transform.position.x - transform.position.x) <= 0.5f)
                 activeTile = currentTile;
             else
                 activeTile = oldTile;
         }
         else if (currentTile.transform.position.x == oldTile.transform.position.x)
         {
             if (Math.Abs(currentTile.transform.position.z - transform.position.z) <= 0.5f)
                 activeTile = currentTile;
             else
                 activeTile = oldTile;
         }
         else
         {
             activeTile = currentTile;
         }

         if (direction == "left")
         {
             if (currentTile.vertical == true)
             {
                 return "left";
             }
         }
         else if (direction == "right")
         {
             if (currentTile.vertical == true)
             {
                 return "right";
             }
         }


         return "";

     }
     */
    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Tile")
        {
            Tile collidedTile = col.gameObject.GetComponent<Tile>();
            // the old and the current tile have always to be connected. if the Player moves in the oppisite direction you got to assure that the old tile stays the same.
            // the next current and old tile need to be in a line, this is checked by the leaste indented if clauses (or else it's a corner)
            // if the old and the next tile are 2 apart, then there is no direction change.


            //assgin tiles
            if (currentTile != null)
            {
                oldTile = currentTile;
                currentTile = collidedTile;

            }
            //inital
            else
            {
                currentTile = collidedTile;
                oldTile = collidedTile;
            }
        }
        
        if(col.tag == "PathEmpty")
        {
            CornerPathEmpty = col.gameObject;
        }
    }

}
