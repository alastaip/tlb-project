using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {
    public static bool active;
    public bool isActive;

	// Use this for initialization
	void Start () {
        active = true;
	}
	
	// Update is called once per frame
	void Update () {
        isActive = active;
	}

    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.name == "Player")
            {
                ObjectInteraction.bisNear = true;
                Debug.Log("The player is near me!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        { ObjectInteraction.bisNear = false; }
    }
}
