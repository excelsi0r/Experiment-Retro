using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunsetHandler : MonoBehaviour
{
    public GameController gc;
    public GameManager gm;
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            Invoke("StopGame", 1f);
            Invoke("Logo", 2f);
            Invoke("TotalFade", 10f);
        }

    }

    void StopGame()
    {
        gc.state = GameController.GameState.Transition;
    }

    void Logo()
    {
        anim.SetTrigger("logo");
    }

    void TotalFade()
    {
        gm.FadeOut();
    }
}
