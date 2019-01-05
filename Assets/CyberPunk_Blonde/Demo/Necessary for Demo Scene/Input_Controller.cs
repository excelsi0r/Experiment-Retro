using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]


public class Input_Controller : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer Srend;
    Animator anim;

    bool isOnGround;

    public float jumpForce = 3f;
    public float maxVelocity = 3f;
    public float bulletSpeed = 10f;
    public float tresholdX = 0.1f;
    public float tresholdY = 0.14f;

    public GameObject bulletPrefab;


    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        Srend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

    }

    void Update ()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            float y = rb.velocity.y;
            float x = 0;
            rb.velocity = new Vector2(x, y);
            anim.SetBool("Running", false);
            
        }
        else if(Input.GetKey(KeyCode.A))
        {
            float y = rb.velocity.y;
            float x = -1 * maxVelocity;
            rb.velocity = new Vector2(x, y);
            anim.SetBool("Running", true);
            anim.SetTrigger("Run");
            Srend.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float y = rb.velocity.y;
            float x = 1 * maxVelocity;
            rb.velocity = new Vector2(x, y);
            anim.SetBool("Running", true);
            anim.SetTrigger("Run");
            Srend.flipX = false;
        }
        else if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            float y = rb.velocity.y;
            float x = 0;
            rb.velocity = new Vector2(x, y);
            anim.SetBool("Running", false);
            anim.SetTrigger("Iddle");
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * 1 * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
            anim.SetBool("Idle", false);
        }

        if (isOnGround)
        {
            anim.SetBool("Idle", true);
        }
        else
        {
            anim.SetBool("Idle", false);
        }

        if (isOnGround && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (anim.GetBool("Idle"))
            {
                anim.SetBool("IsDuck", true);
            }
            else
            {
                return;
            }
        }
        if (isOnGround && Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("IsDuck", false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Attacking", true);
            
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
           anim.SetBool("Attacking", false);
        }
        if(Input.GetKey(KeyCode.F))
        {
            if(anim.GetBool("Attacking"))
                Fire();
        }
    }


    //these functions detect if the object is on the ground or not
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isOnGround = true;
            anim.SetBool("IsOnGround", true);
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


        }
    }

    private void Fire()
    {

        if (Srend.flipX == true)
        {

            Vector3 pos = transform.position;
            Vector3 scale = transform.localScale;
            pos.y += tresholdY * scale.y;
            pos.x += -tresholdX * scale.x;
            pos.z = 0.1f;
            

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                pos,
                transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);
            bullet.GetComponent<SpriteRenderer>().flipX = true;
            bullet.transform.localScale = Vector3.Scale(bullet.transform.localScale, scale);
            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

        }
        else
        {
            Vector3 pos = transform.position;
            Vector3 scale = transform.localScale;
            pos.y += tresholdY * scale.y;
            pos.x += tresholdX * scale.x;
            pos.z = 0.1f;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                pos,
                transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
            bullet.transform.localScale = Vector3.Scale(bullet.transform.localScale, scale);

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

        }


    }
}
