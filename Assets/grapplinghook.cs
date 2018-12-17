using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplinghook : MonoBehaviour
{
    DistanceJoint2D joint;
    Vector3 targetPos;
    RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    public float step = 0.02f;
    public GameObject linePrefab;

    GameObject line;

    // Use this for initialization
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetMouseButtonDown(1))
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                joint.enabled = true;
                Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
                connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
                Debug.Log(connectPoint);
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


        if (Input.GetMouseButtonUp(1))
        {
            joint.enabled = false;
            Destroy(line);
            line = null;
        }

    }
}
