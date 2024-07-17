using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float lerpFactor;

    private Transform focus;

    void Start()
    {
        focus = transform.parent;
        GameManager.Instance.AddPlayerCamera(GetComponentInChildren<Camera>());
    }

    void LateUpdate()
    {
        if(!target) return;

        focus.transform.position = Vector3.Lerp(focus.transform.position, target.transform.position, Time.deltaTime * lerpFactor);
    }
}
