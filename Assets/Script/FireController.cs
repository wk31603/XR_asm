using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("State")]
    public float startHeat = 100f;    // bigger = takes longer to extinguish
    public float heat;                // runtime

    [Header("Visuals (optional but nice)")]
    public ParticleSystem fireParticles; // assign your orange flame particle
    public Light glow;                   // optional point light
    public AudioSource crackle;          // optional sound

    void Awake()
    {
        heat = startHeat;
        if (!fireParticles) fireParticles = GetComponentInChildren<ParticleSystem>();
        if (!glow) glow = GetComponentInChildren<Light>();
        if (!crackle) crackle = GetComponentInChildren<AudioSource>();
    }

    public void ApplyExtinguish(float amount)
    {
        if (heat <= 0f) return;

        heat -= amount * Time.deltaTime;
        float t = Mathf.Clamp01(heat / startHeat);  // 1 = full fire, 0 = out

        // Fade visuals as it cools
        if (fireParticles)
        {
            var em = fireParticles.emission;
            em.rateOverTime = Mathf.Lerp(0f, 50f, t); // adjust numbers for your system
        }
        if (glow) glow.intensity = Mathf.Lerp(0f, 3f, t);
        if (crackle) crackle.volume = Mathf.Lerp(0f, 1f, t);

        if (heat <= 0f)
        {
            Extinguish();
        }
    }

    void Extinguish()
    {
        heat = 0f;
        if (fireParticles) fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (glow) glow.enabled = false;
        if (crackle) crackle.Stop();
        // Optional: disable collider so it no longer “burns”
        var col = GetComponent<Collider>();
        if (col) col.enabled = false;
    }
}
