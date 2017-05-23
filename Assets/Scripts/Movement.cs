using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //settings
    public float originalSpeed = 20f;
    public string direction; //to do: make this better(protected)
    private float speed;


    //save the current and the last tile standing on
    private Tile currentTile;   //newly collided
    private Tile oldTile;       //collided before
    private Tile activeTile;    //more standing on

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
        moveFigure(movementChecker());
        // for testing this
        //moveFigure(direction);
    }

    private void moveFigure(string _direction)
    {
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
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
      

            
    }

   /* public void OnTriggerEnter(Collider col)
    {
        Tile collidedTile = col.gameObject.GetComponent<Tile>();
        // the old and the current tile have always to be connected. if the Player moves in the oppisite direction you got to assure that the old tile stays the same.
        // the next current and old tile need to be in a line, this is checked by the leaste indented if clauses (or else it's a corner)
        // if the old and the next tile are 2 apart, then there is no direction change.


        //inital
        if (currentTile == null)
        {
            currentTile = collidedTile;
            oldTile = collidedTile;
        }

        // to do: make function to reduce redudancy
        //Check which is the currentTile (The tile player is standing on more) 
        //horizontal 
        if (collidedTile.transform.position.z == currentTile.transform.position.z && currentTile.transform.position.z == oldTile.transform.position.z)
        {
            if ((collidedTile.transform.position.x - oldTile.transform.position.x) == 2)//no change
            {
                oldTile = currentTile;
                currentTile = collidedTile;

            }
            else if ((collidedTile.transform.position.x - oldTile.transform.position.x) <= 1)//change
            {
                //oldTile stays the same
                currentTile = collidedTile;

            }
        }
        //vertical
        else if (collidedTile.transform.position.x == currentTile.transform.position.x && currentTile.transform.position.x == oldTile.transform.position.x)
        {
            if ((collidedTile.transform.position.z - oldTile.transform.position.z) == 2)//no change
            {
                oldTile = currentTile;
                currentTile = collidedTile;

            }
            else if ((collidedTile.transform.position.z - oldTile.transform.position.z) <= 1)//change
            {
                //oldTile stays the same
                currentTile = collidedTile;

            }
        }
        else //corner
        {
            oldTile = currentTile;
            currentTile = collidedTile;
        }

    }*/



    //Enter thw wished directions and the current and old tile, a bool value will be given if direction is allowed (at corners it will guide the figure to the right z or x position (the middle of the tile)
    private string movementChecker()
    { 
        float tolerance = 0.15f;

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


        //left
        //corner not in the middle (adjiust until it's centerd and can change direction)
        if (direction == "left")
        {
            //not perfectlty centerd vertical but left is true
            if (activeTile.left && activeTile.transform.position.z - transform.position.z != 0)
            {
                //left and 99% centered
                if (Math.Abs(activeTile.transform.position.z - transform.position.z) < tolerance)
                    transform.position = new Vector3(transform.position.x, transform.position.y, activeTile.transform.position.z);
                //left too high
                else if (activeTile.transform.position.z < transform.position.z && activeTile.transform.position.z != transform.position.z)
                    return "back";
                //left too low
                else if (activeTile.transform.position.z > transform.position.z)
                    return "forward";
            }

            //centered vertical and no Corner
            else if (activeTile.left)
                return direction;

            //centered vertical and a Corner (needs to be centered horizontal
            else if (!activeTile.left && activeTile.right && activeTile.transform.position.x < transform.position.x)
                return direction;
            else if (!activeTile.left && activeTile.right && activeTile.transform.position.x > transform.position.x)
                transform.position = new Vector3(activeTile.transform.position.x, transform.position.y, transform.position.z);
        }

        //right
        //corner not in the middle (adjiust until it's centerd and can change direction)
        else if (direction == "right")
        {
            //not perfectlty centerd vertical but right is true
            if (activeTile.right && activeTile.transform.position.z - transform.position.z != 0)
            {
                //right and 99% centered
                if (Math.Abs(activeTile.transform.position.z - transform.position.z) < tolerance)
                    transform.position = new Vector3(transform.position.x, transform.position.y, activeTile.transform.position.z);
                //right too high
                else if (activeTile.transform.position.z < transform.position.z && activeTile.transform.position.z != transform.position.z)
                    return "back";
                //right too low
                else if (activeTile.transform.position.z > transform.position.z)
                    return "forward";
            }

            //centered vertical and no Corner
            else if (activeTile.right)
                return direction;

            //centered vertical and a Corner (needs to be centered horizontald
            else if (direction == "right" && !activeTile.right && activeTile.left && activeTile.transform.position.x > transform.position.x)
                return direction;
            else if (direction == "right" && !activeTile.right && activeTile.left && activeTile.transform.position.x < transform.position.x)
                transform.position = new Vector3(activeTile.transform.position.x, transform.position.y, transform.position.z);
        }

        //forward
        //corner not in the middle (adjiust until it's centerd and can change direction)
        else if (direction == "forward")
        {
            //not perfectlty centerd horizontal but foward is true
            if (activeTile.forward && activeTile.transform.position.x - transform.position.x != 0)
            {
                //foward and 99% centered
                if (Math.Abs(activeTile.transform.position.x - transform.position.x) < tolerance)
                    transform.position = new Vector3(activeTile.transform.position.x, transform.position.y, transform.position.z);
                //foward too right
                else if (activeTile.transform.position.x < transform.position.x)
                    return "left";
                //foward too left
                else if (activeTile.transform.position.x > transform.position.x)
                    return "right";
            }

            //centered horizonatal and no Corner
            else if (activeTile.forward)
                return direction;

            //centered horizontal and a Corner (needs to be centered vertical)
            else if (direction == "forward" && !activeTile.forward && activeTile.back && activeTile.transform.position.z > transform.position.z)
                return direction;
            else if (direction == "forward" && !activeTile.forward && activeTile.back && activeTile.transform.position.z < transform.position.z)
                transform.position = new Vector3(transform.position.x, transform.position.y, activeTile.transform.position.z);
        }
        //back
        //corner not in the middle (adjiust until it's centerd and can change direction)
        else if (direction == "back")
        {
            //not perfectlty centerd horizontal but back is true
            if (activeTile.back && activeTile.transform.position.x - transform.position.x != 0)
            {
                //back and 99% centered
                if (Math.Abs(activeTile.transform.position.x - transform.position.x) < tolerance)
                    transform.position = new Vector3(activeTile.transform.position.x, transform.position.y, transform.position.z);
                //back too high
                else if (activeTile.transform.position.x < transform.position.x)
                    return "left";
                //back too low
                else if (activeTile.transform.position.x > transform.position.x)
                    return "right";
            }

            //centered horizonatal and no Corner
            else if (activeTile.back)
                return direction;

            //centered horizontal and a Corner (needs to be centered vertical)
            else if (!activeTile.back && activeTile.forward && activeTile.transform.position.z < transform.position.z)
                return direction;
            else if (!activeTile.back && activeTile.forward && activeTile.transform.position.z > transform.position.z)
                transform.position = new Vector3(transform.position.x, transform.position.y, activeTile.transform.position.z);
        }

        return null;
    }
}