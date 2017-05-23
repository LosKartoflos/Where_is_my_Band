using UnityEngine;

public class Camera : MonoBehaviour {

    //for the position of the player
    public GameObject player;

    // Set Camera on player
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 4.6f, 4); // maybe change if player pivot is on  the ground

    }
}
