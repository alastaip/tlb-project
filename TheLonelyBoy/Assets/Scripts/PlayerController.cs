using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed;

    public float jumpForce;

    //private Rigidbody myRigidbody;

    private CharacterController controller;
    private Vector3 moveDirection;
    public float gravityScale;

    private Animator anim;

    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;
   // public GameObject projectile;
    //public Transform shootPoint;

	// Use this for initialization
	void Start () {

        //myRigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        

	}
	
	// Update is called once per frame
	void Update () {
        CheckDeath();


        float yStore = moveDirection.y;

        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Fire1"))
            {
                moveDirection.y = jumpForce;
                
            }
        }


        //Apply gravity
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);


        //Apply vector
        controller.Move(moveDirection * Time.deltaTime);

        //Move the player in different directions based on camera look direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") !=0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }


   
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("moveSpeed", (Mathf.Abs(Input.GetAxis("Vertical")) + (Mathf.Abs(Input.GetAxis("Horizontal")))));

    }

 
    void CheckDeath()
    {
        if (controller.transform.position.y < -3)
        {
            controller.transform.position = new Vector3(7f, 1.1f, 6f);
            moveDirection = new Vector3(0, 0, 0);
            
        }
    }
}
