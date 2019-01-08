using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplinghook : MonoBehaviour
{
    DistanceJoint2D joint;
    Vector3 targetPos;
    RaycastHit2D hit;
    Rigidbody2D rb;
    LayerMask testMask;
    public float distance = 4f;
    public LayerMask mask;
    public float step = 0.02f;
    public float releaseDistance = 0.05f;
    public float releaseForce = 10f;
    public float impulseForce = 250f;
    public GameObject linePrefab;
    public GameController gc;
    
    

    GameObject line;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        testMask = LayerMask.NameToLayer("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        

        if (gc.canGrapple && Input.GetMouseButtonDown(1))
        {
            targetPos = GetWorldPositionOnPlane(Input.mousePosition, 1f);
            targetPos.z = -0f;

            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

            

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null && hit.transform.gameObject.layer == testMask)
            {
                joint.enabled = true;
                Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
                connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
                joint.connectedAnchor = connectPoint;

                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                joint.distance = Vector2.Distance(transform.position, hit.point);


                line = (GameObject)Instantiate(linePrefab);
                line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                line.GetComponent<LineRenderer>().SetPosition(1, hit.point);

            }
        }

        if(Input.GetMouseButton(1) && line != null)
        {
            line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        }


        if (Input.GetMouseButtonUp(1) && line != null)
        {
            joint.enabled = false;
            Destroy(line);
            line = null;
        }

        if(gc.up && line != null)
        {
            joint.distance -= step;
            if(joint.distance <= releaseDistance)
            {
                joint.enabled = false;
                Destroy(line);
                line = null;
                rb.AddForce(new Vector2(0f, impulseForce));
            }
        }

        if(gc.jump && line != null)
        {
            joint.enabled = false;
            Destroy(line);
            line = null;
            rb.AddForce(new Vector2(0f, releaseForce));
        }

        if (gc.down && line != null && joint.distance < distance)
        {
            joint.distance += step;
        }

    }

    Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
