using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInput: MonoBehaviour
{
    public GameObject room;

    public float CameraOffset = -15.0f;
    public float PlayerOffsetX = -0.0f;
    public float PlayerOffsetY = -0.4f;

    public bool up = true;

    SpriteRenderer sprite;
    GameObject player;

    void Start()
    {
        player = null;
        sprite = (SpriteRenderer) gameObject.transform.GetComponentInChildren(typeof(SpriteRenderer));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            sprite.enabled = true;
            player = collider.transform.parent.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            sprite.enabled = false;
            player = null;
        }
    }

    void Update()
    {
        if (player != null && ((up && Input.GetKey(KeyCode.W)) || (!up && Input.GetKey(KeyCode.S))))
        {
            Vector3 pos = room.transform.position;
            pos.z = 0.0f;

            player.transform.position = pos + new Vector3(PlayerOffsetX, PlayerOffsetY, 0.0f);
            Camera.main.transform.position = pos + new Vector3(0.0f, -0.0f, CameraOffset);
        }
    }
}
