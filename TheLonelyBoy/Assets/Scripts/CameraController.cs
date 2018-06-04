using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;

    public Vector3 offset;

    public bool useOffsetValue;

    public float rotateSpeed;

    public Transform pivot;

    public float maxViewAngle;
    public float minViewAngle;

    public bool invertY;


	// Use this for initialization
	void Start () {
        if (!useOffsetValue)
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        // pivot.transform.parent = target.transform;
        pivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        pivot.transform.position = target.transform.position;

        /* //get x position of the mouse and rotate the target
         float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
         pivot.Rotate(0, horizontal, 0);

         //get Y position of the mouse and rotate the pivot
         float vertical = Input.GetAxis("Mouse Y") * rotateSpeed; */

        //get x position of right stick and rotate the target
        float horizontal = Input.GetAxis("RightStickX") * rotateSpeed;
        pivot.Rotate(0, horizontal, 0);

        //get Y position of the rightstick and rotate the pivot
        float vertical = Input.GetAxis("RightStickY") * rotateSpeed; 

        if (invertY)
        {
            pivot.Rotate(vertical, 0, 0);
        }
        else
        {
            pivot.Rotate(-vertical, 0, 0);
        }

        //limit up/down camera movement
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0f, 0f);
        }

        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
        {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0f, 0f);
        }

        //move the camera based on the current rotation of target and the original offset
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        //transform.position = target.position - offset;

        if(transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }
        transform.LookAt(target);	
	}
}
