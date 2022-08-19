using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{

    public float movement;
    public bool moveWithPlayer;

    // Update is called once per frame
    void Update()
    {
        float Horiz = 1;
        if (moveWithPlayer) {
            Horiz = Input.GetAxisRaw("Horizontal");
        }
        
        transform.position = new Vector3( transform.position.x + Horiz*movement, transform.position.y, transform.position.z);
    }
}
