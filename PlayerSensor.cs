using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provide sensory based on raycast
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerSensor : MonoBehaviour
{
    const float skinWidth = 0.015f; //our skin width

    [SerializeField]
    BoxCollider2D boxCollider2D;

    RaycastOrigins raycastOrigin;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    private float horizontalRaySpacing;
    [SerializeField]
    private float verticalRaySpacing;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>(); 
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
        DrawRay();
    }

    public void DrawRay()
    {
        // we want to loop on vertical ray count
        for (int i = 0; i < verticalRayCount; i++)
        {
            /*
             * We start casting ray from origin left, where bottom left based BoxCollider width shape
             * We want to re-position the casted ray WHERE :
             * 1 mean we want to cast ray along X axis of the BoxCollider width shape
             * Multiply by vertical ray spacing
             * i mean how many vertical ray spacing we want to multiply with
             * i will determine how far ray casted from origin bottom left
             */
            Vector2 start = raycastOrigin.bottomLeft + new Vector2(1 * verticalRaySpacing * i, 0);
            Vector2 destination = Vector2.down * 2;
            Debug.DrawRay(start, destination, Color.green);
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            /*
             * Same like vertical, but this time we do both left and right
             * We also do subtraction because we want to re-position downwards
             */
            Vector2 startRight = raycastOrigin.topRight - (new Vector2(0, 1 * horizontalRaySpacing * i));
            Vector2 destinationRight = Vector2.right * 2;
            Debug.DrawRay(startRight, destinationRight, Color.green);

            Vector2 startLeft = raycastOrigin.topLeft - (new Vector2(0, 1 * horizontalRaySpacing * i));
            Vector2 destinationLeft = Vector2.right * -2;
            Debug.DrawRay(startLeft, destinationLeft, Color.green);
        }
    }

    private void UpdateRaycastOrigins()
    {
        //Set bounds value same with our box collider shapes area
        Bounds bounds = boxCollider2D.bounds;
        //We want to shrink a bit our bounds compared to original box collider shape
        bounds.Expand(skinWidth * -2);

        //Below we just set raycast origin based on their types
        raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    }

    private void CalculateRaySpacing()
    {
        //Set bounds value same with our box collider shapes area
        Bounds bounds = boxCollider2D.bounds;
        //We want to shrink a bit our bounds compared to original box collider shape
        bounds.Expand(skinWidth * -2);

        //We want to make sure minimum horizontal ray equal to 2, can't be below, but can go higher
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        //Calculate spacing between each casted ray. More casted rays, smaller spacing size
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        //Calculate spacing between each casted ray. More casted rays, smaller spacing size
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    
    struct RaycastOrigins
    {
        public Vector2 topRight, topLeft;
        public Vector2 bottomRight, bottomLeft;
    }

}
