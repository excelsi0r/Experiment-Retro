using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]


public class Input_Controller : MonoBehaviour
{
    //Checkpoint state
    public static int GAME_CHECKPOINT = 0;
    
    //Health const paramethers
    private const float MAX_HEALTH = 100f;
    private const float DEC_HEALTH = 5f;

    private float currHealth;
    public SimpleHealthBar health;
    public GameController gc;
    public GameManager gm;
    Rigidbody2D rb;
    SpriteRenderer Srend;
    Animator anim;
    BoxCollider2D headAndBodyCollider;

    bool isOnGround;

    public float jumpForce = 4f;
    public float maxVelocity = 4f;
    public float walkSpeed = 2f;
    public float bulletSpeed = 10f;
    public float BulletTresholdX = 0.23f;
    public float BulletTresholdY = 0.2f;
    public float BulletDestroyTime = 2.0f;
    public float BulletTresholdCrouchX = 0.5f;
    public float BulletTresholdCrouchY = -0.2f;

    public GameObject bulletPrefab;

    public GameObject FirstCheckpoint;
    public GameObject SecondCheckpoint;
    public GameObject ThirdCheckpoint;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        Srend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        headAndBodyCollider = GetComponentInChildren<BoxCollider2D>();
        currHealth = MAX_HEALTH;
        health.UpdateBar(currHealth, MAX_HEALTH);
        anim.SetTrigger("Revive");

        //Respawn with checkpoints
        if (GAME_CHECKPOINT == 0)
        {
            Vector3 pos = FirstCheckpoint.transform.position; pos.x += -0.56f; pos.y += -0.31f; pos.z = 0;
            gameObject.transform.position = pos;
            //Camera
            Vector3 camPos = FirstCheckpoint.transform.position;
            camPos.z = -15f;
            Camera.main.transform.position = camPos;
            //Controller
            gc.canSprint = false;
            gc.canShoot = false;
            gc.canGrapple = false;
        }
        else if(GAME_CHECKPOINT == 1)
        {
            Vector3 pos = SecondCheckpoint.transform.position; pos.x += -1.5f; pos.y += -0.4f; pos.z = 0;
            gameObject.transform.position = pos;
            //Camera
            Vector3 camPos = SecondCheckpoint.transform.position;
            camPos.z = -15f;
            Camera.main.transform.position = camPos;
            //Controller
            gc.canSprint = true;
            gc.canShoot = true;
            gc.canGrapple = false;
        }
        else if(GAME_CHECKPOINT == 2)
        {
            Vector3 pos = SecondCheckpoint.transform.position; pos.x += 7.0f; pos.y += -0.4f; pos.z = 0;
            gameObject.transform.position = pos;
            //Camera
            Vector3 camPos = SecondCheckpoint.transform.position;
            camPos.z = -15f;
            Camera.main.transform.position = camPos;
            //Controller
            gc.canSprint = true;
            gc.canShoot = true;
            gc.canGrapple = true;
            //delete boss
            //TODO DELETE BOSS
        }
    }

    void Update ()
    {
        if (gc.crouch && isOnGround)
        {
            anim.SetBool("IsDuck", true);
            headAndBodyCollider.enabled = false;

            if (gc.left)
                Srend.flipX = true;
            if (gc.right)
                Srend.flipX = false;
            if (gc.attack)
            {
                anim.SetBool("Attacking", true);
                Fire();
            }
            else
                anim.SetBool("Attacking", false);

            return;
        }
        else
        {
            anim.SetBool("IsDuck", false);
            headAndBodyCollider.enabled = true;
        }

        if (gc.attack)
        {
            anim.SetBool("Attacking", true);
            Fire();
        }
        else
        {
            anim.SetBool("Attacking", false);
        }

        if (gc.jump)
        {
            if (isOnGround)
            {
                rb.velocity = Vector2.zero;
                if (gc.left && gc.sprint)
                {
                    Vector2 force;
                    force.x = -1 * maxVelocity;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = true;
                }
                else if(gc.right && gc.sprint)
                {
                    Vector2 force;
                    force.x = 1 * maxVelocity;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = false;
                }
                else if(gc.left)
                {
                    Vector2 force;
                    force.x = -1 * walkSpeed;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = true;
                }
                else if(gc.right)
                {
                    Vector2 force;
                    force.x = 1 * walkSpeed;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = false;
                }
                else
                {
                    Vector2 force;
                    force.x = 0;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
            return;

        }

        if (gc.left && gc.sprint && isOnGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 force;
            force.x = -maxVelocity;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
            Srend.flipX = true;
            anim.SetBool("Running", true);
        }
        else if (gc.right && gc.sprint && isOnGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 force;
            force.x = maxVelocity;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
            Srend.flipX = false;
            anim.SetBool("Running", true);
        }
        else if (gc.left && isOnGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 force;
            force.x = -walkSpeed;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
            anim.SetBool("Walking", true);
            anim.SetBool("Running", false);
            Srend.flipX = true;
        }
        else if (gc.right && isOnGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 force;
            force.x = walkSpeed;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
            anim.SetBool("Walking", true);
            anim.SetBool("Running", false);
            Srend.flipX = false;
        }
        else if(gc.left && !isOnGround)
        {
            Srend.flipX = true;

            Vector2 force;
            force.x = -0.05f;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        else if (gc.right && !isOnGround)
        {
            Srend.flipX = false;

            Vector2 force;
            force.x = 0.05f;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            if (isOnGround)
            {
                Vector2 velociy = rb.velocity;
                velociy.x = 0;
                rb.velocity = velociy;
            }

            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }
    }


    //these functions detect if the object is on the ground or not
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isOnGround = true;
            anim.SetBool("IsOnGround", true);
        }

        if (collision.collider.tag.Equals("Bullet"))
        {
            currHealth -= DEC_HEALTH;
            health.UpdateBar(currHealth, MAX_HEALTH);
            anim.SetTrigger("Damaged");
            if (currHealth <= 0.0f)
                Death();
        }

        if (collision.collider.tag.Equals("GroundDeath"))
        {
            currHealth -= MAX_HEALTH;
            health.UpdateBar(currHealth, MAX_HEALTH);
            Death();
        }

    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isOnGround = true;
            anim.SetBool("IsOnGround", true);

        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isOnGround = false;
            anim.SetBool("IsOnGround", false);
            anim.SetBool("Idle", false);
            anim.SetTrigger("Jump");

        }
    }

    private void Fire()
    {
        if (Srend.flipX == false)
        {
            Vector3 pos = transform.position;
            Vector3 scale = transform.localScale;
            if (anim.GetBool("IsDuck"))
            {
                pos.y += BulletTresholdCrouchY * scale.y;
                pos.x += BulletTresholdCrouchX * scale.x;
            }
            else
            {
                pos.y += BulletTresholdY * scale.y;
                pos.x += BulletTresholdX * scale.x;
            }
            pos.z = 0.1f;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                pos,
                transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
            bullet.transform.localScale = Vector3.Scale(bullet.transform.localScale, scale);
            Destroy(bullet, BulletDestroyTime);
        }

        else if (Srend.flipX == true)
        {
            Vector3 pos = transform.position;
            Vector3 scale = transform.localScale;
            if (anim.GetBool("IsDuck"))
            {
                pos.y += BulletTresholdCrouchY * scale.y;
                pos.x += -BulletTresholdCrouchX * scale.x;
            }
            else
            {
                pos.y += BulletTresholdY * scale.y;
                pos.x += -BulletTresholdX * scale.x;
            }
            pos.z = 0.1f;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                pos,
                transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);
            bullet.transform.localScale = Vector3.Scale(bullet.transform.localScale, scale);
            Destroy(bullet, BulletDestroyTime);

            Vector3 bulletScale = bullet.transform.localScale;
            bulletScale.x *= -1;
            bullet.transform.localScale = bulletScale;
        }
    }

    private void Death()
    {
        anim.SetTrigger("Death");
        gc.state = GameController.GameState.Death;
        gm.FadeOut();
        Invoke("Respawn", 3.0f);
    }

    private void Respawn()
    {
        SceneManager.LoadScene("Chapter1");
    }
}
