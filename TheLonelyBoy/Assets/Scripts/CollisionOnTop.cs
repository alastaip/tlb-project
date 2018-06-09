using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOnTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            ObjectInteraction.bisOnTop = true;
            CollisionDetection.active = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            ObjectInteraction.bisOnTop = false;
            CollisionDetection.active = true;

        }
    }

}
