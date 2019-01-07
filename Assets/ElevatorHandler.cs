using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorHandler : MonoBehaviour
{
    bool running = false;

    public float velocity;
    public GameObject room;


    private void Update()
    { 
        if (running)
        {
            Vector3 mypos = gameObject.transform.position;
            mypos.z = -15f;

            Camera.main.transform.position = mypos;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Stop"))
        {
            gameObject.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(0, 0);
            running = false;
        }
    }
    


    public void ElevatorUp()
    {
        gameObject.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(0, velocity);
        running = true;
    }
}
