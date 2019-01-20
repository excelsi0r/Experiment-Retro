using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private const float MAX_HEALTH = 100f;
    private const float DEC_HEALTH = 5f;

    private float currHealth;
    private Animator anim;
    public SimpleHealthBar health;
    public GameController gc;
    public GameManager gm;

    void Start()
    {
        currHealth = MAX_HEALTH;
        health.UpdateBar(currHealth, MAX_HEALTH);
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag.Equals("Bullet"))
        {
            Debug.Log("Hit by Bullet");
            currHealth -= DEC_HEALTH;
            health.UpdateBar(currHealth, MAX_HEALTH);
            anim.SetTrigger("Damaged");
            if (currHealth <= 0.0f)
                Death();
        }
        else if(collision.collider.tag.Equals("GroundDeath"))
        {
            Debug.Log("Death by falling bullet");
            currHealth -= MAX_HEALTH;
            health.UpdateBar(currHealth, MAX_HEALTH);
            Death();
        }
    }

    private void Death()
    {
        anim.SetTrigger("Death");
        gc.state = GameController.GameState.Death;
        gm.FadeIn();
        Invoke("Respawn", 5.0f);
    }

    private void Respawn()
    {
        SceneManager.LoadScene("Chapter1");
    }

}
