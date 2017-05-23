using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    public GameObject player;
    public Movement movementScript;

    private string lastTileName = "";
    private string lastWalkingDirection, currentWalkingDirection;

    // Update is called once per frame
    void Update()
    {
        movingDirection(player);
        movementScript.direction = currentWalkingDirection;
    }

    // Movement Ki

    //checks where npc and player is and tells the npc where the player relativeley is to him on the axis 
    private string[] checkForPlayerPosition(GameObject _player)
    {
        string x_axis = "", z_axis = "";

        //horizontal
        if (_player.transform.position.x == transform.position.x)
        {
            x_axis = "justified";
        }
        else if (_player.transform.position.x > transform.position.x)
        {
            x_axis = "rightside";
        }
        else if (_player.transform.position.x < transform.position.x)
        {
            x_axis = "lefttside";
        }

        //vertical
        if (_player.transform.position.x == transform.position.x)
        {
            z_axis = "justified";
        }
        else if (_player.transform.position.z > transform.position.z)
        {
            z_axis = "above";
        }
        else if (_player.transform.position.z < transform.position.z)
        {
            z_axis = "below";
        }


        return new string[2] { x_axis, z_axis };
    }

    // this function determines the walking direction depending on the players position and the current tile
    private void movingDirection(GameObject _player)
    {

        string x_axis, z_axis;
        string[] axisStoreValue = new string[2];
        Tile activeTile = movementScript.get_activeTile();

        axisStoreValue = checkForPlayerPosition(_player);

        x_axis = axisStoreValue[0];
        z_axis = axisStoreValue[1];

        if (lastTileName != activeTile.name)
            lastTileName = activeTile.name;

        //depending on the Tile the NPC is standing and the position of the Player a moving direction is given
        //to do: add recognizing radius and random walk before
        //to do: now the npc has to start on a vertical or horizontal tile (make it indipendend form the tile)


         //start
         if(currentWalkingDirection == null)
         {
            if (x_axis == "leftside" && activeTile.left)
            {
                currentWalkingDirection = "left";
            }
            else if (x_axis == "rightside" && activeTile.right)
            {
                currentWalkingDirection = "right";
            }
            else if (z_axis == "above" && activeTile.forward)
            {
                currentWalkingDirection = "forward";
            }
            else if (z_axis == "below" && activeTile.back)
            {
                currentWalkingDirection = "back";
            }
         }


        //checks for new directions if it's completly on active tile and it's not horizontal or corner or vertical


        //Corner
        //makes npc walk arround the corner (doesn't look for player)
        else if (activeTile.corner)
        {
            currentWalkingDirection = "";
            if(Mathf.Abs(activeTile.transform.position.x - transform.position.x) < 0.015f || Mathf.Abs(activeTile.transform.position.z - transform.position.z) < 0.015f)
            {
                if (lastWalkingDirection == "left" || lastWalkingDirection == "right")
                {
                    if (activeTile.forward)
                        currentWalkingDirection = "forward";
                    else if (activeTile.back)
                        currentWalkingDirection = "back";
                }
                else if (lastWalkingDirection == "forward" || lastWalkingDirection == "back")
                {
                    if (activeTile.left)
                        currentWalkingDirection = "left";
                    else if (activeTile.right)
                        currentWalkingDirection = "right";
                }
            }
            
        }
        //Crossing
        else if (activeTile.crossing)
        {


        }
        //T-Crossing
        else if (activeTile.t_crossing)
        {
             
             
        }
        //Dead End
        else if (!activeTile.horizontal && !activeTile.vertical)
        {

         }

        if (lastTileName != activeTile.name)
            lastWalkingDirection = currentWalkingDirection;


        //Vertical Tile (just for beginning)
        /*if (activeTile.vertical && currentWalkingDirection == null )
         {
             // same on 
             if (z_axis == "justified")
                 currentWalkingDirection = "forward";
             else if (z_axis == "below")
                 currentWalkingDirection = "back";
             else if (z_axis == "above")
                 currentWalkingDirection = "forward";


         }
         //Horizontal Tile (just for beginning)
         else if (activeTile.horizontal && currentWalkingDirection == null)
         {
             if (x_axis == "justified")
                 currentWalkingDirection = "left";
         }
         //Corner
         else if (activeTile.corner)
         {
             if(lastWalkingDirection == "left" || lastWalkingDirection == "right")
             {
                 if (activeTile.forward)
                     currentWalkingDirection = "forward";
                 else if (activeTile.back)
                     currentWalkingDirection = "back";
             }
             else if(lastWalkingDirection == "forward" || lastWalkingDirection == "back")
             {
                 if (activeTile.left)
                     currentWalkingDirection = "left";
                 else if (activeTile.right)
                     currentWalkingDirection = "right";
             }
         }
         //Crossing
         else if (activeTile.crossing)
         {


         }
         //T-Crossing
         else if (activeTile.t_crossing)
         {
             if (lastWalkingDirection == "left" || lastWalkingDirection == "right")
             {
                 if (z_axis == "justified")
                 {
                     if (x_axis == "leftside" && activeTile.left)
                         currentWalkingDirection = "left";
                     else if (x_axis == "rightside" && activeTile.right)
                         currentWalkingDirection = "right";
                     else if (activeTile.forward)
                         currentWalkingDirection = "forward";
                     else if (activeTile.back)
                         currentWalkingDirection = "back";

                 }
                 else if (z_axis == "below")
                 {
                     if (activeTile.back)
                         currentWalkingDirection = "back";
                     else if (lastWalkingDirection != "left") 
                         currentWalkingDirection = "left";
                     else
                         currentWalkingDirection = "right";
                 }
                 else if (z_axis == "above")
                 {
                     if (activeTile.forward)
                         currentWalkingDirection = "forward";
                     else if (lastWalkingDirection != "left")
                         currentWalkingDirection = "left";
                     else
                         currentWalkingDirection = "right";
                 }
             }
             else if (lastWalkingDirection == "forward" || lastWalkingDirection == "back")
             {
                 if (x_axis == "justified")
                 {
                     if (z_axis == "above" && activeTile.forward)
                         currentWalkingDirection = "forward";
                     else if (z_axis == "below" && activeTile.back)
                         currentWalkingDirection = "back";
                     else if (activeTile.left)
                         currentWalkingDirection = "left";
                     else if (activeTile.right)
                         currentWalkingDirection = "right";

                 }
             }
         }
         //Dead End
         else
         {

         }
         */
        /* if(x_axis == "justified")
         {
             if (z_axis == "above" && activeTile.forward)
                 currentWalkingDirection = "forward";
             else if (z_axis == "below" && activeTile.back)
                 currentWalkingDirection = "back";
         }
         else if (x_axis == "leftside")
         {
             if (activeTile.left)
                 currentWalkingDirection = "left";
             else if (z_axis == "above" && activeTile.forward)
                 currentWalkingDirection = "forward";
             else if (z_axis == "below" && activeTile.back)
                 currentWalkingDirection = "back";
         }
         else if (x_axis == "rightside")
         {
             if (activeTile.right)
                 currentWalkingDirection = "right";
             else if (z_axis == "above" && activeTile.forward)
                 currentWalkingDirection = "forward";
             else if (z_axis == "below" && activeTile.back)
                 currentWalkingDirection = "back";
         }

         lastWalkingDirection = currentWalkingDirection;
         */
    }
}
