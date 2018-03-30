using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for any objects that can be interacted with via Raycast.
// Works with the "RaycastInteractionSystem" script.
// Written by Connor Smith and Anish Kannan at the VR Club at UCSD.

public class RaycastInteractableObject : MonoBehaviour {

    // Called when a raycast first hits the object (1 frame only).
    public virtual void OnRaycastEnter(RaycastHit hitInfo)
    {
        Debug.Log("Raycast entered on " + gameObject.name);
    }

    // Called when a raycast leaves this object (1 frame only).
    public virtual void OnRaycastExit()
    {
        Debug.Log("Raycast exited on " + gameObject.name);
    }

    // Called every frame that a raycast is hitting this object.
    public virtual void OnRaycast(RaycastHit hitInfo)
    {
        Debug.Log("Raycast hold on " + gameObject.name);
    }

    // Try enabling input in RaycastInteractionSystem!"
    // Dependent on the device you're using.
    /*
    public virtual void OnPress(RaycastHit hitInfo)
    {
        Debug.Log("Button pressed");
    }

    public virtual void OnRelease(RaycastHit hitInfo)
    {
        Debug.Log("Button released");
    }

    public virtual void OnHold(RaycastHit hitInfo)
    {
        Debug.Log("Button hold");
    }
    */
}
