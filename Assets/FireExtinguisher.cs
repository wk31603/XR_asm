using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisherVR : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem[] sprays;   // drag your FE Particle here (or multiple)
    public AudioSource sprayLoop;     // optional looping "pshhh" sound

    [Header("Tuning")]
    public float emissionWhenOn = 300f;   // density while pressed

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.activated.AddListener(OnActivated);         // trigger down
        grab.deactivated.AddListener(OnDeactivated);     // trigger up
        grab.selectExited.AddListener(OnSelectExited);   // dropped
    }

    void OnDestroy()
    {
        if (!grab) return;
        grab.activated.RemoveListener(OnActivated);
        grab.deactivated.RemoveListener(OnDeactivated);
        grab.selectExited.RemoveListener(OnSelectExited);
    }

    void OnActivated(ActivateEventArgs _)
    {
        foreach (var ps in sprays)
        {
            var e = ps.emission;
            e.rateOverTime = emissionWhenOn;
            if (!ps.isPlaying) ps.Play();
        }
        if (sprayLoop && !sprayLoop.isPlaying) sprayLoop.Play();
    }

    void OnDeactivated(DeactivateEventArgs _)
    {
        StopSpray();
    }

    void OnSelectExited(SelectExitEventArgs _)
    {
        StopSpray();
    }

    void StopSpray()
    {
        foreach (var ps in sprays)
        {
            var e = ps.emission;
            e.rateOverTime = 0f;      // lets existing particles die out naturally
            // ps.Stop();              // use this instead if you want instant cut
        }
        if (sprayLoop && sprayLoop.isPlaying) sprayLoop.Stop();
    }
}
