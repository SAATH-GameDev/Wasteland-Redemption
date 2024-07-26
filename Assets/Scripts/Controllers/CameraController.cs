using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float lerpFactor;

    private Transform focus;
    private Transform defaultTarget;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetTarget()
    {
        target = defaultTarget;
    }

    void Start()
    {
        focus = transform.parent;
        GameManager.Instance.AddPlayerCamera(GetComponentInChildren<Camera>());
        defaultTarget = target;
    }

    void LateUpdate()
    {
        if(!target)
            return;

        focus.transform.position = Vector3.Lerp(focus.transform.position, target.transform.position, Time.deltaTime * lerpFactor);
    }
}
