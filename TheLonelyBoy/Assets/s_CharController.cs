using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_CharController : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 4f;
    Vector3 forward, right;

    // Use this for initialization
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey||Input.GetAxisRaw("HorizIso")>0||Input.GetAxisRaw("VertIso")>0)
        {
            Move();
        }


    }
    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("HorizIso"), 0, Input.GetAxis("VertIso"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizIso");
        Vector3 forwardMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VertIso");

        Vector3 heading = Vector3.Normalize(rightMovement + forwardMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += forwardMovement;
    }
}