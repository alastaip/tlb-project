using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocuarCamera : MonoBehaviour {

    public GameObject binocularCamera;

	// Use this for initialization
	void Start () {
        binocularCamera.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetButton("Fire3"))
        {
            binocularCamera.SetActive(true);

        }

        else { binocularCamera.SetActive(false); }
	}
}
