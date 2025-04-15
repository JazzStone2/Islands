using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D

public class Torch : MonoBehaviour
{
    public Light2D torchLight; // Reference to the Light2D component on the torch
    public float minIntensity = 0.5f; // Minimum light intensity (during the day)
    public float maxIntensity = 2.0f; // Maximum light intensity (at night)
    public ParticleSystem torchParticles; // Reference to the ParticleSystem on the torch

    private DayAndNight dayAndNightManager; // Reference to the DayAndNight script

    private void Start()
    {
        // Ensure torchLight is assigned
        if (torchLight == null)
        {
            torchLight = GetComponent<Light2D>();
            if (torchLight == null)
            {
                Debug.LogError("No Light2D component found! Please assign the torchLight in the inspector.");
                return;
            }
        }

        // Ensure torchParticles is assigned
        if (torchParticles == null)
        {
            torchParticles = GetComponentInChildren<ParticleSystem>();
            if (torchParticles == null)
            {
            }
        }

        // Find the DayAndNight manager in the scene
        dayAndNightManager = FindAnyObjectByType<DayAndNight>();
        if (dayAndNightManager == null)
        {
            Debug.LogError("No DayAndNight script found! Please ensure there is a DayAndNight script in the scene.");
        }
    }

    private void Update()
    {
        // Continuously update the torch's intensity based on the day-night cycle
        UpdateTorchLight();

        // Ensure the particle system is playing
        if (torchParticles != null && !torchParticles.isPlaying)
        {
            torchParticles.Play();
        }
    }

    private void UpdateTorchLight()
    {
        if (dayAndNightManager != null)
        {
            // Get the normalized timeOfDay value from the DayAndNight script
            float nightFactor = dayAndNightManager.GetNightFactor();

            // Gradually blend the light intensity based on the night factor
            torchLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, nightFactor);
        }
        else
        {
            Debug.LogWarning("DayAndNight manager reference is missing.");
        }
    }
}
