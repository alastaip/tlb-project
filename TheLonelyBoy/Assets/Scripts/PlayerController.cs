using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region GameObjects
    private PlayerController player;
    public GameObject playerModel;
    private CharacterController controller;
    private Vector3 moveDirection;
    private Animator anim;
    public GameObject interactingObject;
    public Vector3 interactingObjectCenter;
    private bool canInteract;
    #endregion

    #region MovementVars
    public float fMoveSpeed;    //4
    public float fJumpForce;    //20
    public float fGravity;      //0.15
    private float fTempYVelocity;
    public Transform pivot;
    public float fRotateSpeed;  //25
    #endregion

    void Start()
    {
        player = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        canInteract = false;
    }

    void Update()
    {
        CheckDeath();

        ResetState();
        CheckState();

        GetDirectionalInput();
        CalculateVelocity();
        GetActionInput();

        ApplyEnvironment();

        Move();

        SetAnim();
    }

    void CheckDeath()
    {
        if (controller.transform.position.y < -3)
        {
            controller.transform.position = new Vector3(7f, 1.1f, 6f);
            moveDirection = new Vector3(0, 0, 0);
        }
    }

    void ResetState() { }

    void CheckState() { }

    void GetDirectionalInput()
    {
        fTempYVelocity = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
    }

    void CalculateVelocity()
    {
        moveDirection = moveDirection.normalized * fMoveSpeed;
        moveDirection.y = fTempYVelocity;   //?why
    }

    void GetActionInput()
    {
        #region Jump
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                moveDirection.y = fJumpForce;
            }
        }
        #endregion

        if (Input.GetButtonDown("Fire2") && canInteract)
        {
            Interact();
            interactingObject = null;
            interactingObjectCenter = Vector3.zero;
            canInteract = false;
        }
    }

    void ApplyEnvironment()
    {
        //Apply gravity
        moveDirection.y = moveDirection.y + (Physics.gravity.y * fGravity);
    }

    void Move()
    {
        //Apply vector
        controller.Move(moveDirection * Time.deltaTime);

        //Move the player in different directions based on camera look direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, fRotateSpeed * Time.deltaTime);
        }
    }

    void SetAnim()
    {
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("moveSpeed", (Mathf.Abs(Input.GetAxis("Vertical")) + (Mathf.Abs(Input.GetAxis("Horizontal")))));
    }

    void Interact()
    {

        Debug.Log("I'm interacting!");
        player.transform.position = interactingObjectCenter;


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InteractZone")
        {
            canInteract = true;
            Debug.Log(other.transform.name);
            interactingObject = other.gameObject;
            interactingObjectCenter = other.gameObject.transform.TransformPoint(other.GetComponent<CapsuleCollider>().center);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
    }
}
