using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RooftopHandler : MonoBehaviour
{

    GameObject player;

    public float cameraOffsetZ = -15.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player") && player == null)
        {
            player = collision.collider.transform.parent.gameObject;
        }
    }

    void Update()
    {
        if(player != null)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 cameraPos = Camera.main.transform.position;

            Vector3 newPos = new Vector3(playerPos.x, cameraPos.y, cameraOffsetZ);
            Camera.main.transform.position = newPos;
        }
    }
}
