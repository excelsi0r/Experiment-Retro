using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]




public class Input_Controller : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer Srend;
    Animator anim;

    bool isOnGround;

    //change these variables if you wish to test different speeds and jump heights
    [SerializeField]
    float moveForce = .1f;
    [SerializeField]
    float jumpForce = 5f;
    [SerializeField]
    float maxVelocity = 10f;

    public GameObject bulletPrefab;

    //this variable is used for the screen wrapping
    float screenHalfWidthInWorldUnits;


    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        Srend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        //these lines are used to calculate screen wrapping
        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
    }

    void Update ()
    {

        //controller and sprite manipulation
        #region
        //controller and sprite manipulation
        if (Input.GetKey(KeyCode.A))
        {
            if (rb.velocity.x > 3)
            {
                anim.SetBool("IsSkid", true);
            }else
            {
                anim.SetBool("IsSkid", false);
            }
        
      
            if (Mathf.Abs(rb.velocity.x) < maxVelocity)
            {
                rb.AddForce(Vector2.right * -1 * moveForce, ForceMode2D.Impulse);//moves the object
                anim.SetFloat("MoveX", Mathf.Abs(rb.velocity.x));
           
            }
            if (rb.velocity.x < 0)
            {
                Srend.flipX = true;//flips the sprite
            }
            anim.SetBool("Idle", false);
            //call animation
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (rb.velocity.x < -3)
            {
                anim.SetBool("IsSkid", true);
            }
            else
            {
                anim.SetBool("IsSkid", false);
            }

            if (Mathf.Abs(rb.velocity.x) < maxVelocity)
            {
                rb.AddForce(Vector2.right * 1 * moveForce, ForceMode2D.Impulse);//moves the object
                anim.SetFloat("MoveX", Mathf.Abs(rb.velocity.x));

            }
            //call animation
            if (rb.velocity.x > 0 )
            {
                Srend.flipX = false;//flips the sprite
            }
            anim.SetBool("Idle", false);

        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
          

            rb.AddForce(Vector2.up * 1 * jumpForce, ForceMode2D.Impulse);//moves the sprite
            anim.SetTrigger("Jump");//call animation
            anim.SetBool("Idle", false);

        }

        anim.SetFloat("MoveX", Mathf.Abs(rb.velocity.x));
        if (isOnGround)
        {
            anim.SetBool("Idle", true);
        }else
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
        if (isOnGround && Input.GetKeyUp(KeyCode.DownArrow))
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Damaged");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetBool("Die", true);
            anim.SetTrigger("Death");
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            anim.SetBool("Die", false);
        }
        #endregion // //controls and sprite manipulation

        //camera wrap
        #region
        //controls the camera wrap
        if (transform.position.x < -screenHalfWidthInWorldUnits)
        {
            transform.position = new Vector2(screenHalfWidthInWorldUnits, transform.position.y);
        }

        if (transform.position.x > screenHalfWidthInWorldUnits)
        {
            transform.position = new Vector2(-screenHalfWidthInWorldUnits, transform.position.y);
        }
        #endregion//camera wrap 
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

            Vector2 pos = transform.position;
            pos.y += 0.7f;
            pos.x += -1.5f;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                pos,
                transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-20.0f, 0);
            bullet.GetComponent<SpriteRenderer>().flipX = true;
            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

        }
        else
        {
            Vector2 pos = transform.position;
            pos.y += 0.7f;
            pos.x += 1.5f;

            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                pos,
                transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(20.0f, 0);
            
            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

        }


    }
}
