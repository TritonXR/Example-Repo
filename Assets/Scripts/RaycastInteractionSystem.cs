using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A system to handle raycast interactions with objects.
// For example, would be useful for a gaze interaction system on mobile VR devices.
// This should be placed on any object that EMTIS the raycast (the raycast origin).
// Specifically uses the "RaycastInteractableObject" script.
// Written by Connor Smith and Anish Kannan at the VR Club at UCSD.
public class RaycastInteractionSystem : MonoBehaviour
{
    
    // Keeps track of which object the raycast is currently hitting.
    private RaycastInteractableObject currentRaycastObject;

    // Used for input if implemented. Keeps track of current input object.
    private RaycastInteractableObject currentSelectedObject;

    // Line renderer to show the active raycast.
    [SerializeField] private LineRenderer raycastLine;

    // Colors for the raycast on hit or miss.
    [SerializeField] private Color raycastMissColor = Color.red;
    [SerializeField] private Color raycastHitColor = Color.green;

    public void Start()
    {

        // If there's no raycast line dragged in, create a default one.
        if (!raycastLine)
        {
            Debug.LogWarning("No LineRenderer dragged in for raycast line. Dragging in is recommended! Creating a default now.");

            // Create a new object, add a Line Renderer, and set some defaults.
            GameObject newLine = new GameObject("RaycastLine");
            newLine.transform.parent = this.transform;
            raycastLine = newLine.AddComponent<LineRenderer>();
            raycastLine.startWidth = raycastLine.endWidth = 0.1f;
            SetRaycastColor(raycastMissColor);

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Do the actual raycast.
        ProcessRaycast();
    }

    // Performs the raycast and affects any raycasted objects.
    public void ProcessRaycast()
    {

        // Set the origin of the raycast line renderer.
        raycastLine.SetPosition(0, transform.position);

        // Create a ray to raycast with.
        Ray raycastRay = new Ray(transform.position, transform.forward);

        // Variable to store the result of the raycast.
        RaycastHit hitInfo;

        // Show a debug line in the editor.
        Debug.DrawRay(raycastRay.origin, raycastRay.direction);

        // Do the raycast and store the result in hitInfo.
        if (Physics.Raycast(raycastRay, out hitInfo))
        {

            // If we hit something, update the raycast to end at that object.
            raycastLine.SetPosition(1, hitInfo.point);

            // Store the hit object.
            GameObject hitObj = hitInfo.collider.gameObject;

            // Get the raycast interactable script from the object (if it has one).
            RaycastInteractableObject gazeObj = hitObj.GetComponent<RaycastInteractableObject>();

            // If it does have the interactable script, interact with the object.
            if (gazeObj != null)
            {
                // Update the raycast hit color.
                SetRaycastColor(raycastHitColor);

                // If we aren't looking at the same object as last frame, update the past/present objects.
                if (gazeObj != currentRaycastObject)
                {

                    // Clear the previous object.
                    ClearCurrentObject();

                    // Set the new object.
                    currentRaycastObject = gazeObj;

                    // Call OnEnter.
                    currentRaycastObject.OnRaycastEnter(hitInfo);
                }

                // If we ARE looking at the same object, just call it's OnRaycast() function.
                else
                {
                    // Call the hold function.
                    currentRaycastObject.OnRaycast(hitInfo);
                }
            }
            
            // If we aren't looking at a valid interactable object, clear the previous object.
            else
            {
                // Clear previous object.
                ClearCurrentObject();
            }
        }
        
        // If we aren't looking at any physics object, clear previous object and update line renderer.
        else
        {
            // Make line renderer stretch out to infinity (kinda).
            raycastLine.SetPosition(1, transform.forward * 1000.0f);
            SetRaycastColor(raycastMissColor);

            // Clear any active object, if there is one.
            ClearCurrentObject();
        }

        // Check for user input.
        CheckForInput(hitInfo);

    }

    // Try enabling input for the device you're using!
    // Will need to replace the "Input.GetMouseButton" calls with your own device's controllers.
    public void CheckForInput(RaycastHit hitInfo)
    {
        /*
        if (Input.GetMouseButtonDown(0) && currentRaycastObject != null)
        {
            currentSelectedObject = currentRaycastObject;
            currentSelectedObject.OnPress(hitInfo);
        }
        else if (Input.GetMouseButton(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnHold(hitInfo);
        }
        else if (Input.GetMouseButtonUp(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnRelease(hitInfo);
            currentSelectedObject = null;
        }
        */
    }

    // Clears any currently selected object and calls on exit if there is one.
    private void ClearCurrentObject()
    {
        // Check if there's a current object.
        if (currentRaycastObject != null)
        {
            // If so, call OnExit on that object and clear the variable.
            currentRaycastObject.OnRaycastExit();
            currentRaycastObject = null;
        }
    }

    // Set the color of the raycast line renderer.
    private void SetRaycastColor(Color color)
    {

        // Store the line's material.
        Material raycastMat = raycastLine.material;

        // Set the line's color.
        raycastMat.SetColor("_Color", color);

        // Store back in the new material.
        raycastLine.material = raycastMat;

    }
}
