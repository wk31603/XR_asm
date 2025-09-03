using UnityEngine;

// Attach to your white CO₂ ParticleSystem (FE Particle)
[RequireComponent(typeof(ParticleSystem))]
public class FEParticleExtinguisher : MonoBehaviour
{
    [Tooltip("How fast the spray removes heat when it collides with fire.")]
    public float extinguishPerHit = 25f;

    // Called when a particle collides with a collider (needs setup below)
    void OnParticleCollision(GameObject other)
    {
        // Look for FireController on the hit object or its parents
        var fire = other.GetComponentInParent<FireController>();
        if (fire != null)
        {
            fire.ApplyExtinguish(extinguishPerHit);
        }
    }
}
