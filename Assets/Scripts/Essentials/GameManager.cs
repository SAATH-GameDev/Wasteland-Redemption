using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Comparison
{
    GREATER,
    GREATER_EQUAL,
    LESSER,
    LESSER_EQUAL,
    EQUAL,
    NOT_EQUAL
}

partial class GameManager : MonoBehaviour
{
    public GameplayProfile gameplay;
    [Space]
    public Transform pointer;
    public LayerMask pointerLayers;

    [Space]
    public Transform canvas;

    [Space]
    public EventTrigger currentTrigger = null;

    public static GameManager Instance;

    private List<Camera> playerCameras = new List<Camera>();
    private SpriteRenderer pointerArrow;

    public static bool Compare(float value1, Comparison compare, float value2)
    {
        switch(compare)
        {
            case Comparison.GREATER: return value1 > value2;
            case Comparison.GREATER_EQUAL: return value1 >= value2;
            case Comparison.LESSER: return value1 < value2;
            case Comparison.LESSER_EQUAL: return value1 <= value2;
            case Comparison.EQUAL: return value1 == value2;
            case Comparison.NOT_EQUAL: return value1 != value2;
            default: return false;
        }
    }

    public void SetAllCamsTarget(Transform target)
    {
        foreach(Camera cam in playerCameras)
            cam.GetComponentInParent<CameraController>().SetTarget(target);
    }

    public void ClearCamsTarget()
    {
        foreach(Camera cam in playerCameras)
            cam.GetComponentInParent<CameraController>().SetTarget(null);
    }

    public Ray GetMouseRay(int playerIndex = 0)
    {
        if(playerCameras.Count <= 0)
            return new Ray();

        return playerCameras[playerIndex].ScreenPointToRay(Mouse.current.position.ReadValue(), Camera.MonoOrStereoscopicEye.Mono);
    }

    public Vector3 WorldToScreenPosition(Vector3 worldPosition, int playerIndex = 0)
    {
        if(playerCameras.Count <= 0)
            return Vector3.zero;

        return playerCameras[playerIndex].WorldToScreenPoint(worldPosition);
    }

    public void StepEventTriggerSequence()
    {
        if(currentTrigger != null)
            currentTrigger.StepSequence();
    }

    public void PlayEventTriggerSequence(int index)
    {
        if(currentTrigger != null)
            currentTrigger.PlaySequence(index);
    }

    public void DisableRender(Transform group)
    {
        for(int i = 0; i < group.childCount; i++)
            group.GetChild(i).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    public void EnableRender(Transform group)
    {
        for(int i = 0; i < group.childCount; i++)
            group.GetChild(i).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    private void UpdatePointer()
    {
        RaycastHit hitInfo;
        Physics.Raycast(GetMouseRay(), out hitInfo, 10000.0f, pointerLayers, QueryTriggerInteraction.Collide);
        if(!hitInfo.collider)
            return;
        if(hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            pointer.position = hitInfo.collider.transform.position;
            pointerArrow.transform.localScale = Vector3.one * 0.4f;
            pointerArrow.color = Color.red;
        }
        else
        {
            pointer.position = hitInfo.point;

            if(pointerArrow.transform.localScale.x > 0.25f)
            {
                pointerArrow.transform.localScale = Vector3.one * 0.25f;
                pointerArrow.color = Color.cyan;
            }
        }
    }

    void Awake()
    {
        Instance = this;
        
        #if UNITY_EDITOR
        #else
        int screenCount = GetScreenCount();
        for(int i = 0; i < screenCount; i++)
            Display.displays[i].Activate();
        #endif

        pointerArrow = pointer.GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        if(!pointer)
            return;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        if(!ready)
        {
            Time.timeScale = 0.0f;
            return;
        }

        if(!pointer || playerCameras.Count <= 0)
            return;

        UpdatePointer();
    }
}
