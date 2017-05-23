using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathEmptySettings : MonoBehaviour {

    //direction allowed
    [Header("Direction allowed")]
    public bool left = false;
    public bool right = false;
    public bool forward = false;
    public bool back = false;
    //does have to switch lane
    [Header("Switching Directions")]
    public bool leftSwitch = false;
    public bool rightSwitch = false;
    public bool forwardSwitch = false;
    public bool backSwitch = false;
}
