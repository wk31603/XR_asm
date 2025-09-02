using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class SpawnAtPoint : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform spawnPoint;
    public KeyCode recenterKey = KeyCode.R;

    IEnumerator Start()
    {
        yield return null; // wait one frame for XR to initialize
        Recenter();
    }

    void Update()
    {
        if (Input.GetKeyDown(recenterKey)) Recenter();
    }

    public void Recenter()
    {
        if (!xrOrigin || !spawnPoint) return;

        // Place the CAMERA at the spawn position
        xrOrigin.MoveCameraToWorldLocation(spawnPoint.position);

        // (Optional) Also match yaw to the spawn's forward
        var fwd = spawnPoint.forward;
        fwd.y = 0f;
        if (fwd.sqrMagnitude > 0.0001f)
            xrOrigin.MatchOriginUpCameraForward(Vector3.up, fwd.normalized);
    }
}