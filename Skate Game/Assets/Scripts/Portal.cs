using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public Transform exitPoint;
    public float teleportDelay = 0.1f; // Prevents instant re-teleportation
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!linkedPortal || isTeleporting) return;

        // Check if the object is a player or physics object
        if (other.CompareTag("Player") || other.GetComponent<Rigidbody>())
        {
            StartCoroutine(TeleportObject(other));
        }
    }

    private IEnumerator TeleportObject(Collider obj)
    {
        isTeleporting = true;
        linkedPortal.isTeleporting = true;

        // Get the object's Rigidbody (for physics objects)
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        // Calculate new position and rotation
        Transform objTransform = obj.transform;
        Vector3 localOffset = transform.InverseTransformPoint(objTransform.position);
        Vector3 newWorldPosition = linkedPortal.transform.TransformPoint(localOffset);
        Quaternion rotationOffset = Quaternion.Inverse(transform.rotation) * objTransform.rotation;
        Quaternion newWorldRotation = linkedPortal.transform.rotation * rotationOffset;

        yield return new WaitForSeconds(teleportDelay);

        // Teleport object
        objTransform.position = newWorldPosition;
        objTransform.rotation = newWorldRotation;

        // Preserve velocity for physics objects
        if (rb)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            Vector3 newWorldVelocity = linkedPortal.transform.TransformDirection(localVelocity);
            rb.linearVelocity = newWorldVelocity;
        }

        yield return new WaitForSeconds(teleportDelay);
        isTeleporting = false;
        linkedPortal.isTeleporting = false;
    }
}