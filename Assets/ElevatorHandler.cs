using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorHandler : MonoBehaviour
{
    bool running = false;

    public float velocity;
    public GameObject room;
    public GameManager gm;

    public float CameraOffset = -15.0f;
    public float PlayerOffsetX = -0.0f;
    public float PlayerOffsetY = -0.4f;

    GameObject player;

    private void Update()
    { 
        if (running)
        {
            Vector3 mypos = gameObject.transform.position;
            mypos.z = -15f;

            Camera.main.transform.position = mypos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player") && player == null)
        {
            player = collision.collider.transform.parent.gameObject;
            Invoke("ElevatorUp",2.0f);
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Stop"))
        {
            gameObject.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(0, 0);
            running = false;
            Invoke("Fade", 0.5f);
            Invoke("Spawn",1.1f);
        }
    }
    
    public void ElevatorUp()
    {
        gameObject.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(0, velocity);
        running = true;
    }

    void Fade()
    {
        gm.FadeInOut();
    }

    void Spawn()
    {
        Vector3 pos = room.transform.position;
        pos.z = 0.0f;

        player.transform.position = pos + new Vector3(PlayerOffsetX, PlayerOffsetY, 0.0f);
        Camera.main.transform.position = pos + new Vector3(0.0f, -0.0f, CameraOffset);
    }
}
