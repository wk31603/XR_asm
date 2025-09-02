using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ExtinguisherSpray : MonoBehaviour
{
    public ParticleSystem spray;
    XRGrabInteractable grab;
    bool isHeld = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        // Track grab state
        grab.selectEntered.AddListener(_ => isHeld = true);
        grab.selectExited.AddListener(_ => { isHeld = false; StopSpray(); });

        // Activated = spray on
        grab.activated.AddListener(_ => { if (spray && !spray.isPlaying) spray.Play(true); });
        grab.deactivated.AddListener(_ => StopSpray());
    }

    void StopSpray()
    {
        if (spray && spray.isPlaying)
            spray.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
