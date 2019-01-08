using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerMechanic : MonoBehaviour
{

    public GameController gc;
    public Pickup pickup;

    public enum Pickup
    {
        Default,
        Sprint,
        Pistol,
        Hook
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals("Player"))
        {
            if(pickup == Pickup.Sprint)
            {
                gc.canSprint = true;
            }
            else if (pickup == Pickup.Pistol)
            {
                gc.canShoot = true;
            }
            else if (pickup == Pickup.Hook)
            {
                gc.canGrapple = true;
            }
            Destroy(gameObject);
        }
    }
}
