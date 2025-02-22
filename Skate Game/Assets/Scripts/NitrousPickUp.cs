using UnityEngine;

public class NitrousPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Checks if the player collides with the pickup
        {
            NitrousBoost nitrous = other.GetComponent<NitrousBoost>();

            if (nitrous != null)
            {
                Debug.Log("Collected Nitrous! Boost Replenished to 100%");
                nitrous.ReplenishNitrous(gameObject); // Calls the method and destroys this pickup
            }
            else
            {
                Debug.LogError("NitrousBoost script not found on Player!");
            }
        }
    }
}
