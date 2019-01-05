using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletCollision : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        Destroy(gameObject);
    }
}
