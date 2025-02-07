using UnityEngine;

public class PickupItems : MonoBehaviour
{
    [SerializeField] KeyCode pickupKeyCode = KeyCode.E;

    [SerializeField] float itemPickupDistance;
    [SerializeField] float itemPickupRayRadius;

    IPickable lastWatchedItem;

    [SerializeField] WeaponHolder weaponHolder;

    private void Update()
    {
        /*
        Vector3 boxHalfExtents = new Vector3(itemPickupRadius, itemPickupRadius, itemPickupMaxDistance);
        Vector3 boxCenter = Camera.main.transform.position + Camera.main.transform.forward * (itemPickupMaxDistance);
        Quaternion boxOrientation = Quaternion.LookRotation(Camera.main.transform.forward);

        if (Physics.BoxCast(Camera.main.transform.position, boxHalfExtents, Camera.main.transform.forward, out RaycastHit hit, 
            boxOrientation, itemPickupMaxDistance))
        {
            if (hit.transform.TryGetComponent(out IPickable item) && Vector3.Distance(transform.root.position, hit.transform.position) <= itemPickupMaxDistance)
            {
                //Can pickup item
                lastWatchedItem = item;
                item.ShowInfo();

                if (Input.GetKeyDown(pickupKeyCode))
                {
                    //Picks up item
                    item.Pickup();
                }
            }
            else if (lastWatchedItem != null) {
                lastWatchedItem.HideInfo();
            }
        }

        DrawBoxCast(boxCenter, boxHalfExtents, boxOrientation);
        */    

        Vector3 origin = Camera.main.transform.position - Camera.main.transform.forward * (itemPickupRayRadius / 2);
        Vector3 direction = Camera.main.transform.forward;

        if (Physics.SphereCast(origin, itemPickupRayRadius, direction, out RaycastHit hit)) { 
            if (hit.transform.TryGetComponent(out IPickable item)) {
                //Player points to an item
                if (Vector3.Distance(transform.root.position, hit.transform.position) <= itemPickupDistance) {
                    //Player is in pickup range
                    item.ShowInfo();
                    lastWatchedItem = item;

                    if (Input.GetKeyDown(pickupKeyCode)) {
                        item.Pickup(weaponHolder);
                    }
                }
                else {
                    lastWatchedItem?.HideInfo();
                }
            }
            else {
                lastWatchedItem?.HideInfo();
            }
        }

        DrawSphereCast(origin, direction);
    }

    private void DrawSphereCast(Vector3 origin, Vector3 direction)
    {
        Vector3 endPosition = origin + direction * itemPickupDistance;

        // Draw the main cast line
        Debug.DrawRay(origin, direction * itemPickupDistance, Color.green);

        // Draw properly rotated circles at the start and end of the SphereCast
        DrawCircle(origin, itemPickupRayRadius, direction, Color.green);
        DrawCircle(endPosition, itemPickupRayRadius, direction, Color.green);
    }

    private void DrawCircle(Vector3 position, float radius, Vector3 forward, Color color)
    {
        int segments = 16;
        float angleStep = 360f / segments;

        // Find right and up vectors perpendicular to the forward direction
        Vector3 right = Vector3.Cross(forward, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Vector3 lastPoint = position + right * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            Vector3 nextPoint = position + (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * radius;

            Debug.DrawLine(lastPoint, nextPoint, color);
            lastPoint = nextPoint;
        }
    }

}
