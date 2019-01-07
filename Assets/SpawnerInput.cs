using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInput: MonoBehaviour
{
    public GameController gc;
    public GameManager gm;

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
        if (player != null && ((gc.up && up) || (gc.down && !up)))
        {
            gm.FadeInOut();
            Invoke("Spawn", 0.7f);
        }
    }


    /// <summary>
    /// Spawn predefined
    /// </summary>
    void Spawn()
    {
        Vector3 pos = room.transform.position;
        pos.z = 0.0f;

        player.transform.position = pos + new Vector3(PlayerOffsetX, PlayerOffsetY, 0.0f);
        Camera.main.transform.position = pos + new Vector3(0.0f, -0.0f, CameraOffset);
    }
}
