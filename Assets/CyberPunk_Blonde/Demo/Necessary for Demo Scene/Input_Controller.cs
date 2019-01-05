﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]


public class Input_Controller : MonoBehaviour {

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


    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        Srend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        headAndBodyCollider = GetComponentInChildren<BoxCollider2D>();
    }

    void Update ()
    {
        if (Input.GetKey(KeyCode.LeftControl) && isOnGround)
        {
            anim.SetBool("IsDuck", true);

            if (Input.GetKey(KeyCode.A))
                Srend.flipX = true;
            if (Input.GetKey(KeyCode.D))
                Srend.flipX = false;
            if (Input.GetKey(KeyCode.F))
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
        }

        if(Input.GetKey(KeyCode.Space))
        {
            if (isOnGround)
            {
                rb.velocity = Vector2.zero;
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
                {
                    Vector2 force;
                    force.x = -1 * maxVelocity;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = true;
                }
                else if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
                {
                    Vector2 force;
                    force.x = 1 * maxVelocity;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = false;
                }
                else if(Input.GetKey(KeyCode.A))
                {
                    Vector2 force;
                    force.x = -1 * walkSpeed;
                    force.y = 1 * jumpForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    Srend.flipX = true;
                }
                else if(Input.GetKey(KeyCode.D))
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
                anim.SetTrigger("Jump");
            }
            return;

        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift) && isOnGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 force;
            force.x = -maxVelocity;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
            Srend.flipX = true;
            anim.SetBool("Running", true);
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && isOnGround)
        {
            rb.velocity = Vector2.zero;
            Vector2 force;
            force.x = maxVelocity;
            force.y = 0f;
            rb.AddForce(force, ForceMode2D.Impulse);
            Srend.flipX = false;
            anim.SetBool("Running", true);
        }
        else if (Input.GetKey(KeyCode.A) && isOnGround)
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
        else if (Input.GetKey(KeyCode.D) && isOnGround)
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
        else if(Input.GetKey(KeyCode.A) && !isOnGround)
        {
            Srend.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D) && !isOnGround)
        {
            Srend.flipX = false;
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

        if (Input.GetKey(KeyCode.F))
        {
            anim.SetBool("Attacking", true);
            Fire();
        }
        else
        {
            anim.SetBool("Attacking", false);
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
}