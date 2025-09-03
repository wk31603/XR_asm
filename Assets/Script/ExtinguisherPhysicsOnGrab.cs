using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class ExtinguisherPhysicsOnGrab : MonoBehaviour
{
    XRGrabInteractable grab;
    Rigidbody rb;
    bool prevKinematic;
    bool prevUseGravity;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectEntered.AddListener(_ =>
        {
            // Save old settings
            prevKinematic = rb.isKinematic;
            prevUseGravity = rb.useGravity;

            // Disable physics while held
            rb.isKinematic = true;
            rb.useGravity = false;

            // Ensure movement type is kinematic
            grab.movementType = XRBaseInteractable.MovementType.Kinematic;
        });

        grab.selectExited.AddListener(_ =>
        {
            // Restore physics when dropped
            rb.isKinematic = prevKinematic;
            rb.useGravity = prevUseGravity;
        });
    }
}
