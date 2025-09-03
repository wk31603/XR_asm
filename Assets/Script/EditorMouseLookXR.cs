using UnityEngine;
using UnityEngine.InputSystem;              // new Input System
using Unity.XR.CoreUtils;                   // for XROrigin

// Simple mouse-look for XR Device Simulator / editor testing.
// Rotates XR Origin for yaw and the Camera for pitch.
public class EditorMouseLookXR : MonoBehaviour
{
    public XROrigin xrOrigin;               // drag your XR Rig here
    public Camera xrCamera;                 // drag Main Camera here (child of XR Rig)
    public float sensitivity = 0.12f;       // mouse sensitivity
    public float pitchMin = -80f, pitchMax = 80f;
    public bool lockCursorOnPlay = true;    // lock cursor to Game view
    public bool alwaysOn = true;            // true = no RMB required
    public bool holdRightMouse = false;     // if alwaysOn=false, set this true to require RMB

    float pitch;                             // camera pitch accumulator

    void Reset()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        xrCamera = Camera.main;
    }

    void OnEnable()
    {
        if (lockCursorOnPlay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (xrCamera != null)
            pitch = xrCamera.transform.localEulerAngles.x;
    }

    void OnDisable()
    {
        if (lockCursorOnPlay)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // Only in editor / when a mouse exists
        if (Mouse.current == null || xrOrigin == null || xrCamera == null) return;

        bool allowLook = alwaysOn || (holdRightMouse && Mouse.current.rightButton.isPressed);
        if (!allowLook) return;

        Vector2 d = Mouse.current.delta.ReadValue();   // pixels since last frame
        float yawDelta = d.x * sensitivity;
        float pitchDelta = -d.y * sensitivity;

        // YAW: rotate XR Origin around world up
        xrOrigin.transform.Rotate(0f, yawDelta, 0f, Space.World);

        // PITCH: tilt camera locally, clamped
        pitch = Mathf.Clamp(NormalizeAngle(pitch + pitchDelta), pitchMin, pitchMax);
        Vector3 e = xrCamera.transform.localEulerAngles;
        e.x = pitch;
        e.z = 0f; // keep roll zero
        xrCamera.transform.localEulerAngles = e;
    }

    static float NormalizeAngle(float a)
    {
        while (a > 180f) a -= 360f;
        while (a < -180f) a += 360f;
        return a;
    }
}
