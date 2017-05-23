using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Tile : MonoBehaviour
{

    //this class sets up the tiles in the begining:
    //the allowed walking directions and the texture,
    //depending on walking directions

    //allowed walking directions
    [Header("Walking Directions")]
    public bool left = false;
    public bool right = false;
    public bool forward = false;
    public bool back = false;

    //tile type
    [Header("Tile Type")]
    // if nothing is true, its a dead end
    public bool crossing = false;
    public bool t_crossing = false;
    public bool vertical = false;
    public bool horizontal = false;
    public bool corner = false;
    public bool deadend = true;

    //empties
    //to do: do this over get component: e.g.  transform.FindChild("NW").gameObject.GetComponent<pathEmptySettings>().back = true;
    private pathEmptySettings pathEmptySettingsNW;
    private pathEmptySettings pathEmptySettingsNE;
    private pathEmptySettings pathEmptySettingsSW;
    private pathEmptySettings pathEmptySettingsSE;


    //to Do: Rotatate Material for less Materials and make it load the materials itself (less public variables)
    //Materials for different Road Layout 
    //the letters at the ending stand for the direction the street goes: n=north, s=south, w=west, e=east.
    [Header("Materials")]
    public Material material_straight_hor;
    public Material material_straight_ver;
    public Material material_corner_nw;
    public Material material_corner_ne;
    public Material material_corner_sw;
    public Material material_corner_se;
    public Material material_crossing;
    public Material material_t_crossing_n;
    public Material material_t_crossing_e;
    public Material material_t_crossing_s;
    public Material material_t_crossing_w;
    public Material material_deadend_n;
    public Material material_deadend_e;
    public Material material_deadend_s;
    public Material material_deadend_w;
   
    

    //Renderer renderer = gameObject.GetComponent<Renderer>();


    void Start()
    {
        pathEmptySettingsNW = transform.Find("NW").gameObject.GetComponent<pathEmptySettings>();
        pathEmptySettingsNE = transform.Find("NE").gameObject.GetComponent<pathEmptySettings>();
        pathEmptySettingsSW = transform.Find("SW").gameObject.GetComponent<pathEmptySettings>();
        pathEmptySettingsSE = transform.Find("SE").gameObject.GetComponent<pathEmptySettings>();



    left = false;
        right = false;
        forward = false;
        back = false;
        //checks for colliders of other sidwalks to set the allowed walking directions
        //(**to do: check if there is a baking option to save the level an do it before)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 12.1f);
        int i = 0;
       
        while (i < hitColliders.Length)
        {
            //sort the sidewalks out by tag and check if it is the same
            if (hitColliders[i].tag == "Tile" && hitColliders[i].transform.position != transform.position)
            {
                SetAllowedDirections(hitColliders[i].gameObject);
            }

            i += 2;
            // because it detects the mesh and the box collider of the same 
            //(**to do: if there are  detections problems change it to i++)
        }
        setType();

    }

    private void SetAllowedDirections(GameObject collided)
    {
        //sets the allowed walking directions depending
        //on the position of the detected neighbours

        if (collided.transform.position - Vector3.left*10 == transform.position)
            left = true;
        if (collided.transform.position - Vector3.right*10 == transform.position)
            right = true;
        if (collided.transform.position - Vector3.forward*10 == transform.position)
            forward = true;
        if (collided.transform.position - Vector3.back*10 == transform.position)
            back = true;

    }

    //sets the type of tile depending on it's directions
    //although sets the texture and the acitvates the PathEmpty Collider
    private void setType()
    {
        //horizontal straight
        if (left && right && !forward && !back)
        {
            horizontal = true;
            gameObject.GetComponent<Renderer>().material = material_straight_hor;
            setPathwayEmptyCollidersActive(new string[] { });
        }
        //verical straight    
        else if (!left && !right && forward && back)
        {
            vertical = true;
            gameObject.GetComponent<Renderer>().material = material_straight_ver;
            setPathwayEmptyCollidersActive(new string[] {  });
        }
        //corners
        else if (numberOfDirections() == 2)
        {
            corner = true;

            t_crossing = true;

            //North West
            if (left && forward)
            {
                //North West Empty
                pathEmptySettingsNW.forward = true;
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;
                pathEmptySettingsNW.left = true;

                pathEmptySettingsNW.backSwitch = true;
                pathEmptySettingsNW.rightSwitch = true;

                //South East Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.left = true;               

                gameObject.GetComponent<Renderer>().material = material_corner_nw;
                setPathwayEmptyCollidersActive(new string[] { "NW", "SE" });
                transform.Find("SE").gameObject.GetComponent<pathEmptySettings>().right = true;
            }
            //South West   
            else if (left && back)
            {
                //North East Empty
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.left = true;

                //South West Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.back = true;
                pathEmptySettingsSW.right = true;
                pathEmptySettingsSW.left = true;

                pathEmptySettingsSW.forwardSwitch = true;
                pathEmptySettingsSW.rightSwitch = true;

                gameObject.GetComponent<Renderer>().material = material_corner_sw;
                setPathwayEmptyCollidersActive(new string[] { "NE", "SW" });
            }        
            //North East      
            else if (right && forward)
            {
                //North East Empty
                pathEmptySettingsNE.forward = true;
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.right = true;
                pathEmptySettingsNE.left = true;

                pathEmptySettingsNE.backSwitch = true;
                pathEmptySettingsNE.leftSwitch = true;

                //South West Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.right = true;

                gameObject.GetComponent<Renderer>().material = material_corner_ne;
                setPathwayEmptyCollidersActive(new string[] { "NE","SW"});
            }     
            //South East
            else if (right && back)
            {
                //North West Empty
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;

                //South East Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.back = true;
                pathEmptySettingsSE.left = true;
                pathEmptySettingsSE.right = true;

                pathEmptySettingsSE.leftSwitch = true;
                pathEmptySettingsSE.forwardSwitch = true;

                gameObject.GetComponent<Renderer>().material = material_corner_se;
                setPathwayEmptyCollidersActive(new string[] { "NW", "SE" });
            }
                

        }
        //t crossing
        else if (numberOfDirections() == 3)
        {
            //East
            t_crossing = true;

            if (!left)
            {
                //North East Empty
                pathEmptySettingsNE.forward = true;
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.right = true;
                pathEmptySettingsNE.left = true;

                pathEmptySettingsNE.backSwitch = true;
                pathEmptySettingsNE.leftSwitch = true;

                //North West Empty
                pathEmptySettingsNW.forward = true;
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;
                pathEmptySettingsNW.left = true;

                pathEmptySettingsNW.rightSwitch = true;

                //South East Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.back = true;
                pathEmptySettingsSW.right = true;
                pathEmptySettingsSW.left = true;

                pathEmptySettingsSW.forwardSwitch = true;
                pathEmptySettingsSW.leftSwitch = true;

                //South West Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.back = true;
                pathEmptySettingsSE.right = true;
                pathEmptySettingsSE.left = true;

                pathEmptySettingsSE.rightSwitch = true;

                gameObject.GetComponent<Renderer>().material = material_t_crossing_e;
                setPathwayEmptyCollidersActive(new string[] { "NE", "NW", "SW", "SE" });
            }
            //West
            else if (!right)
            {
                //North East Empty
                pathEmptySettingsNE.forward = true;
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.left = true;

                pathEmptySettingsNE.leftSwitch = true;

                //North West Empty
                pathEmptySettingsNW.forward = true;
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;
                pathEmptySettingsNW.left = true;

                pathEmptySettingsNW.backSwitch = true;
                pathEmptySettingsNW.rightSwitch = true;

                //South East Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.back = true;
                pathEmptySettingsSW.left = true;

                pathEmptySettingsSW.leftSwitch = true;

                //South West Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.back = true;
                pathEmptySettingsSE.right = true;
                pathEmptySettingsSE.left = true;

                pathEmptySettingsSE.forwardSwitch = true;
                pathEmptySettingsSE.rightSwitch = true;

                gameObject.GetComponent<Renderer>().material = material_t_crossing_w;
                setPathwayEmptyCollidersActive(new string[] { "NE", "NW", "SW", "SE" });
            }
            //South
            else if (!forward)
            {
                //North East Empty
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.right = true;
                pathEmptySettingsNE.left = true;

                pathEmptySettingsNE.backSwitch = true;

                //North West Empty
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;
                pathEmptySettingsNW.left = true;

                pathEmptySettingsNW.backSwitch = true;

                //South East Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.back = true;
                pathEmptySettingsSW.right = true;
                pathEmptySettingsSW.left = true;

                pathEmptySettingsSW.forwardSwitch = true;
                pathEmptySettingsSW.leftSwitch = true;

                //South West Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.back = true;
                pathEmptySettingsSE.right = true;
                pathEmptySettingsSE.left = true;

                pathEmptySettingsSE.forwardSwitch = true;
                pathEmptySettingsSE.rightSwitch = true;

                gameObject.GetComponent<Renderer>().material = material_t_crossing_s;
                setPathwayEmptyCollidersActive(new string[] { "NE", "NW", "SW", "SE" });
            }
            //North                
            else if (!back)
            {
                //North East Empty
                pathEmptySettingsNE.forward = true;
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.right = true;
                pathEmptySettingsNE.left = true;

                pathEmptySettingsNE.backSwitch = true;
                pathEmptySettingsNE.leftSwitch = true;

                //North West Empty
                pathEmptySettingsNW.forward = true;
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;
                pathEmptySettingsNW.left = true;

                pathEmptySettingsNW.backSwitch = true;
                pathEmptySettingsNW.rightSwitch = true;

                //South East Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.right = true;
                pathEmptySettingsSW.left = true;

                pathEmptySettingsSW.forwardSwitch = true;

                //South West Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.right = true;
                pathEmptySettingsSE.left = true;

                pathEmptySettingsSE.forwardSwitch = true;

                gameObject.GetComponent<Renderer>().material = material_t_crossing_n;
                setPathwayEmptyCollidersActive(new string[] { "NE", "NW", "SW", "SE" });
            }
        }
        //crossing
        else if (left && right && forward && back)
        {
            crossing = true;
            gameObject.GetComponent<Renderer>().material = material_crossing;
            setPathwayEmptyCollidersActive(new string[] {"NE","NW", "SW", "SE" });


            //North East Empty
            pathEmptySettingsNE.forward = true;
            pathEmptySettingsNE.back = true;
            pathEmptySettingsNE.right = true;
            pathEmptySettingsNE.left = true;

            pathEmptySettingsNE.backSwitch = true;          
            pathEmptySettingsNE.leftSwitch = true;

            //North West Empty
            pathEmptySettingsNW.forward = true;
            pathEmptySettingsNW.back = true;
            pathEmptySettingsNW.right = true;
            pathEmptySettingsNW.left = true;

            pathEmptySettingsNW.backSwitch = true;
            pathEmptySettingsNW.rightSwitch = true;

            //South East Empty
            pathEmptySettingsSW.forward = true;
            pathEmptySettingsSW.back = true;
            pathEmptySettingsSW.right = true;
            pathEmptySettingsSW.left = true;

            pathEmptySettingsSW.forwardSwitch = true;
            pathEmptySettingsSW.leftSwitch = true;

            //South West Empty
            pathEmptySettingsSE.forward = true;
            pathEmptySettingsSE.back = true;
            pathEmptySettingsSE.right = true;
            pathEmptySettingsSE.left = true;

            pathEmptySettingsSE.forwardSwitch = true;
            pathEmptySettingsSE.rightSwitch = true;

        }
        //deadend
        else if (numberOfDirections() == 1)
        {
            deadend = true;
            //East
            if (left)
            {             
                //North West Empty
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;

                //South West Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.right = true;

                gameObject.GetComponent<Renderer>().material = material_deadend_w;
                setPathwayEmptyCollidersActive(new string[] { "NE", "SE" });
            }
            //West
            else if (right)
            {
                //North East Empty
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.left = true;

                //South East Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.left = true;

                gameObject.GetComponent<Renderer>().material = material_deadend_e;
                setPathwayEmptyCollidersActive(new string[] { "NW", "SW"});
            }
            //North
            else if (forward)
            {
                //South East Empty
                pathEmptySettingsSW.forward = true;
                pathEmptySettingsSW.left = true;

                //South West Empty
                pathEmptySettingsSE.forward = true;
                pathEmptySettingsSE.right = true;

                gameObject.GetComponent<Renderer>().material = material_deadend_n;
                setPathwayEmptyCollidersActive(new string[] { "SW", "SE" });
            }
            //South
            else if (back)
            {
                //North East Empty
                pathEmptySettingsNE.back = true;
                pathEmptySettingsNE.left = true;

                //North West Empty
                pathEmptySettingsNW.back = true;
                pathEmptySettingsNW.right = true;


                gameObject.GetComponent<Renderer>().material = material_deadend_s;
                setPathwayEmptyCollidersActive(new string[] { "NE", "NW", });
            }

        }
            
    }
    //to do: dont hardcode the assgining of the empties directions

    // counts trues of directions (if it's 3 it's a t-crossing and if it's 2 it could be a corner)
    private int numberOfDirections()
    { 
        int counter = 0;

        if (left)
            counter++;
        if (right)
            counter++;
        if (forward)
            counter++;
        if (back)
            counter++;

        return counter;

    }


    //activates the choosen Collider, non choosen get disabeld
    private void setPathwayEmptyCollidersActive(string[] emptyName)
    {
        if (emptyName.Length != 4)
        {
            transform.Find("NE").gameObject.GetComponent<Collider>().enabled = false;
            transform.Find("NW").gameObject.GetComponent<Collider>().enabled = false;
            transform.Find("SE").gameObject.GetComponent<Collider>().enabled = false;
            transform.Find("SW").gameObject.GetComponent<Collider>().enabled = false;
        }      

        for (int i = 0; i < emptyName.Length; i++)
        {
            transform.Find(emptyName[i]).gameObject.GetComponent<Collider>().enabled = true;
        }

      
    }
}
