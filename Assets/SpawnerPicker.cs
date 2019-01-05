using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPicker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject room;

    public float CameraOffset = -15.0f;
    public float PlayerOffsetX = -0.0f;
    public float PlayerOffsetY = -0.4f;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            Vector3 pos = room.transform.position;
            pos.z = 0.0f;

            collider.transform.parent.gameObject.transform.position = pos + new Vector3(PlayerOffsetX, PlayerOffsetY, 0.0f);
            Camera.main.transform.position = pos + new Vector3(0.0f, -0.0f, CameraOffset);
        }
    }
}
