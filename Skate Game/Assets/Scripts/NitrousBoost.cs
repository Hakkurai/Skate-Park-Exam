using UnityEngine;
using TMPro; // For updating UI

public class NitrousBoost : MonoBehaviour
{
    public float maxNitrous = 100f; // Maximum Nitrous capacity
    private float currentNitrous; // Current Nitrous amount
    public float nitrousDrainRate = 20f; // How fast Nitrous depletes per second
    public float speedBoostMultiplier = 2f; // How much faster you go when boosting
    private float normalSpeed; // Store player's default speed
    private bool isBoosting = false;

    public TextMeshProUGUI nitrousText; // TMP UI Text to display Nitrous level
    private SkateboardController playerController; // Reference to movement script

    void Start()
    {
        currentNitrous = maxNitrous; // Start with full Nitrous
        playerController = GetComponent<SkateboardController>();

        if (playerController != null)
        {
            normalSpeed = playerController.moveSpeed; // Store default speed
        }
        else
        {
            Debug.LogError("SkateboardController script not found on player!");
        }

        // Ensure Rigidbody is present
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Rigidbody missing on Player! Add one for proper collision detection.");
        }

        UpdateNitrousUI();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentNitrous > 0)
        {
            ActivateBoost();
        }
        else
        {
            DeactivateBoost();
        }

        // Debug log with percentage meter
        Debug.Log("Nitrous Level: " + Mathf.Round((currentNitrous / maxNitrous) * 100) + "%");
    }

    void ActivateBoost()
    {
        if (!isBoosting && playerController != null && currentNitrous > 0)
        {
            playerController.moveSpeed = normalSpeed * speedBoostMultiplier;
            isBoosting = true;
        }

        currentNitrous -= nitrousDrainRate * Time.deltaTime;
        currentNitrous = Mathf.Clamp(currentNitrous, 0, maxNitrous);

        UpdateNitrousUI();
    }

    void DeactivateBoost()
    {
        if (isBoosting && playerController != null)
        {
            playerController.moveSpeed = normalSpeed;
            isBoosting = false;
        }
    }

    public void ReplenishNitrous(GameObject pickup)
    {
        currentNitrous = maxNitrous;
        Debug.Log("Nitrous Fully Replenished! 100%");

        // Ensure pickup is detached from player (if somehow attached)
        if (pickup.transform.parent != null)
        {
            pickup.transform.SetParent(null);
        }

        Destroy(pickup); // Destroy the pickup prefab
        UpdateNitrousUI();
    }

    void UpdateNitrousUI()
    {
        if (nitrousText != null)
        {
            nitrousText.text = "Nitrous: " + Mathf.Round(currentNitrous) + "%";
        }
    }
}
