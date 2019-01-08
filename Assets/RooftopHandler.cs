using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RooftopHandler : MonoBehaviour
{

    GameObject player;

    public float cameraOffsetZ = -15.0f;
    public float playerDeathCam = 10f;

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

            float heigth;

            if (playerPos.y > gameObject.transform.position.y - playerDeathCam)
                heigth = playerPos.y;
            else
                heigth = gameObject.transform.position.y - playerDeathCam;

            Vector3 newPos = new Vector3(playerPos.x, heigth, cameraOffsetZ);
            Camera.main.transform.position = newPos;
        }
    }
}
