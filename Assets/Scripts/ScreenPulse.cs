using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Required for URP Post-Processing

public class ScreenPulse : MonoBehaviour
{
    public Volume globalVolume;
    public float pulseIntensity = -0.2f; 
    public float returnSpeed = 0.5f;       

    private Vignette vignette;

    void Start()
    {
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

        // smoothly change intensity
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, 0f, returnSpeed * Time.deltaTime);
    }

    public void TriggerPulse()
    {
        if (vignette != null)
        {
            vignette.intensity.value = pulseIntensity;
        }
    }
}