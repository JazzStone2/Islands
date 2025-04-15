using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;
using TMPro; // For TextMeshPro

public class DayAndNight : MonoBehaviour
{
    public float dayLength = 1440f; // Total real-world seconds for a full in-game day (24 minutes)
    public Tilemap[] tilemaps; // Array of Tilemaps to adjust during the day-night cycle
    public Color dayColor = Color.white; // Tilemap color during the day
    public Color nightColor = Color.gray; // Tilemap color during the night
    public Gradient lightGradient; // Gradient controlling light color over time

    [Range(0f, 1f)]
    public float timeOfDay = 0f; // Time of day normalized (0 = start of day, 1 = end of day)
    private Light2D worldLight; // Reference to the global light source
    public TextMeshProUGUI timeDisplay; // Reference to UI Text for displaying time

    private const int MinutesInDay = 1440; // Total in-game minutes in a day (24 hours)
    public bool isDaytime; // Tracks if it's currently day (6 AM to 6 PM)

    void Start()
    {
        // Find the global light if not assigned
        worldLight = FindAnyObjectByType<Light2D>();
        if (worldLight == null)
        {
            Debug.LogError("No Light2D component found! Please add a global 2D light source to the scene.");
        }
    }

    void Update()
    {
        // Update the time of day based on elapsed real-world time
        timeOfDay += Time.deltaTime / dayLength;
        if (timeOfDay > 1f) timeOfDay = 0f; // Wrap around after a full day

        // Update whether it's daytime (6 AM to 6 PM)
        isDaytime = timeOfDay >= (6f / 24f) && timeOfDay < (18f / 24f);

        // Update lighting and tilemap colors
        UpdateGlobalLight();
        UpdateTilemapColors();

        // Update the UI time display
        UpdateTimeDisplay();
    }

    public float GetNightFactor()
    {
        // Gradually calculate night factor for blending colors and lights
        return Mathf.Clamp01((timeOfDay >= (18f / 24f) || timeOfDay < (6f / 24f)) 
            ? Mathf.Abs(Mathf.Sin((timeOfDay - 0.75f) * Mathf.PI * 2f)) 
            : 0f);
    }

    void UpdateGlobalLight()
    {
        if (worldLight != null && lightGradient != null)
        {
            // Use the gradient to update the global light's color based on time of day
            worldLight.color = lightGradient.Evaluate(timeOfDay);
        }
    }

    void UpdateTilemapColors()
    {
        float nightFactor = GetNightFactor();

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap != null)
            {
                // Gradually blend between day and night colors
                tilemap.color = Color.Lerp(dayColor, nightColor, nightFactor);
            }
        }
    }

    void UpdateTimeDisplay()
    {
        if (timeDisplay != null)
        {
            // Convert normalized timeOfDay (0 to 1) into real-world hours and minutes
            float totalMinutes = timeOfDay * MinutesInDay; // Total minutes elapsed in the in-game day
            int hour = Mathf.FloorToInt(totalMinutes / 60f) % 24; // Convert to 24-hour format
            int minute = Mathf.FloorToInt(totalMinutes % 60f);

            // Convert to 12-hour format and determine AM/PM
            string period = hour >= 12 ? "PM" : "AM";
            if (hour == 0) hour = 12; // Midnight case (0 => 12 AM)
            else if (hour > 12) hour -= 12; // Convert to 12-hour clock for PM times

            // Display time as "HH:MM AM/PM"
            timeDisplay.text = string.Format("{0:00}:{1:00} {2}", hour, minute, period);
        }
        else
        {
            Debug.LogWarning("Time Display UI Text is not assigned in the Inspector!");
        }
    }
}
