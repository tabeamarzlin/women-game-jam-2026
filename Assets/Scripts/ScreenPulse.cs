using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Required for URP Post-Processing

public class ScreenPulse : MonoBehaviour
{
    [Header("Settings")]
    public Volume globalVolume;
    public float pulseIntensity = -0.2f; // Negative bends outward, positive inward
    public float returnSpeed = 5f;       // How fast the pulse fades back to normal

    private Vignette vignette;

    void Start()
    {
        // Grab the Lens Distortion override from the volume profile
        if (globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }
        BeatManager.Instance.OnBeat += TriggerPulse;
    }

    void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= TriggerPulse;
    }

    void Update()
    {
        if (vignette == null) return;

        // Smoothly interpolate the intensity back to 0 over time
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, 0f, returnSpeed * Time.deltaTime);
    }

    // Call this method whenever a beat hits
    public void TriggerPulse()
    {
        if (vignette != null)
        {
            vignette.intensity.value = pulseIntensity;
        }
    }
}