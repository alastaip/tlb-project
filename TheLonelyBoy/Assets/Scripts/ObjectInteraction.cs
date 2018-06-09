using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public static bool bisNear;
    public static bool bisOnTop;
    public Collider boxBounds;
    public CapsuleCollider tz_prefab;

    public BoxFaces boxfaces;
    public ColliderCenters colliderCenters;

    public Vector3 boxDimension;
    public Vector3 sphereDimension;

    private float interactionRadius = 0.8f;

    // Use this for initialization
    void Start()
    {
        bisNear = false;
        bisOnTop = false;

        boxBounds = GetComponent<Collider>();

        SetTriggerZone();
    }

    // Update is called once per frame
    void Update()
    {
        if (bisOnTop)
        {
            Debug.Log("holy shit he's on me");
        }
    }

    void SetTriggerZone()
    {
        Bounds bounds = boxBounds.bounds;
        boxDimension = new Vector3(boxBounds.bounds.size.x, boxBounds.bounds.size.y, boxBounds.bounds.size.z);

        #region FindBoxBounds (points on face)
        boxfaces.face_north = new Vector3(boxBounds.bounds.center.x, boxBounds.bounds.center.y, boxBounds.bounds.max.z);
        boxfaces.face_east = new Vector3(boxBounds.bounds.max.x, boxBounds.bounds.center.y, boxBounds.bounds.center.z);
        boxfaces.face_south = new Vector3(boxBounds.bounds.center.x, boxBounds.bounds.center.y, boxBounds.bounds.min.z);
        boxfaces.face_west = new Vector3(boxBounds.bounds.min.x, boxBounds.bounds.center.y, boxBounds.bounds.center.z);
        boxfaces.face_top = new Vector3(boxBounds.bounds.center.x, boxBounds.bounds.max.y, boxBounds.bounds.center.z);
        boxfaces.face_bottom = new Vector3(boxBounds.bounds.center.x, boxBounds.bounds.min.y, boxBounds.bounds.center.z);
        #endregion

        #region FindCentreLocn for colliders
        colliderCenters.center_north = boxfaces.face_north;
        colliderCenters.center_east = boxfaces.face_east;
        colliderCenters.center_south = boxfaces.face_south;
        colliderCenters.center_west = boxfaces.face_west;
        // colliderCenters.center_top = boxfaces.face_top;
        // colliderCenters.center_bottom = boxfaces.face_bottom;
        #endregion

        #region InstantiateColliders
        CapsuleCollider northColl = Instantiate(tz_prefab, colliderCenters.center_north, Quaternion.identity);
        northColl.direction = 0;    //height works on the x-axis
        northColl.radius = interactionRadius;
        northColl.height = boxDimension.x;
        northColl.transform.parent = this.transform;
        northColl.name = "tz_north";
        northColl.tag = "InteractZone";

        CapsuleCollider eastColl = Instantiate(tz_prefab, colliderCenters.center_east, Quaternion.identity);
        eastColl.direction = 2; //height works on the z-axis
        eastColl.radius = interactionRadius;
        eastColl.height = boxDimension.z;
        eastColl.transform.parent = this.transform;
        eastColl.name = "tz_east";
        eastColl.tag = "InteractZone";

        CapsuleCollider southColl = Instantiate(tz_prefab, colliderCenters.center_south, Quaternion.identity);
        southColl.direction = 0;
        southColl.radius = interactionRadius;
        southColl.height = boxDimension.x;
        southColl.transform.parent = this.transform;
        southColl.name = "tz_south";
        southColl.tag = "InteractZone";

        CapsuleCollider westColl = Instantiate(tz_prefab, colliderCenters.center_west, Quaternion.identity);
        westColl.direction = 2; //height works on the z-axis
        westColl.radius = interactionRadius;
        westColl.height = boxDimension.z;
        westColl.transform.parent = this.transform;
        westColl.name = "tz_west";
        westColl.tag = "InteractZone";
        #endregion
    }



    public struct BoxFaces
    {
        public Vector3 face_north, face_east, face_south, face_west, face_top, face_bottom;
    }

    public struct ColliderCenters
    {
        public Vector3 center_north, center_east, center_south, center_west, center_top, center_bottom;
    }
}
