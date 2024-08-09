using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameplayProfile gameplay;
    [Space]
    public Transform pointer;
    public LayerMask pointerLayers;

    [Space]
    public Transform canvas;
    public int forceScreens = 0;

    [Space]
    public EventTrigger currentTrigger = null;

    public static GameManager Instance;

    private List<Camera> playerCameras = new List<Camera>();
    private SpriteRenderer pointerArrow;

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

    public int GetScreenCount()
    {
        if(forceScreens > 0)
            return forceScreens;

        return Display.displays.Length;
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

    public void AddPlayerCamera(Camera camera)
    {
        playerCameras.Add(camera);

        int screenCount = GetScreenCount();
        switch(playerCameras.Count)
        {
            case 1:
            playerCameras[0].targetDisplay = 0;
            playerCameras[0].rect = new Rect(0, 0, 1, 1);
            break;

            case 2:
            if(screenCount == 1)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0.5f, 1, 0.5f);

                playerCameras[1].targetDisplay = 0;
                playerCameras[1].rect = new Rect(0, 0, 1, 0.5f);
            }
            else if(screenCount == 2)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0, 1, 1);

                playerCameras[1].targetDisplay = 1;
                playerCameras[1].rect = new Rect(0, 0, 1, 1);
            }
            break;

            case 3:
            if(screenCount == 1)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);

                playerCameras[1].targetDisplay = 0;
                playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                playerCameras[2].targetDisplay = 0;
                playerCameras[2].rect = new Rect(0, 0, 1, 0.5f);
            }
            else if(screenCount == 2)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0, 1, 0.5f);

                playerCameras[1].targetDisplay = 1;
                playerCameras[1].rect = new Rect(0, 0.5f, 1, 0.5f);

                playerCameras[2].targetDisplay = 1;
                playerCameras[2].rect = new Rect(0, 0, 1, 1);
            }
            else if(screenCount == 3)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0, 1, 1);

                playerCameras[1].targetDisplay = 1;
                playerCameras[1].rect = new Rect(0, 0, 1, 1);

                playerCameras[2].targetDisplay = 2;
                playerCameras[2].rect = new Rect(0, 0, 1, 1);
            }
            break;

            case 4:
            if(screenCount == 1)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);

                playerCameras[1].targetDisplay = 0;
                playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                playerCameras[2].targetDisplay = 0;
                playerCameras[2].rect = new Rect(0.5f, 0, 0.5f, 0.5f);

                playerCameras[3].targetDisplay = 0;
                playerCameras[3].rect = new Rect(0, 0, 0.5f, 0.5f);
            }
            else if(screenCount == 2)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0, 1, 0.5f);

                playerCameras[1].targetDisplay = 0;
                playerCameras[1].rect = new Rect(0, 0.5f, 1, 0.5f);

                playerCameras[2].targetDisplay = 1;
                playerCameras[1].rect = new Rect(0, 0, 1, 0.5f);

                playerCameras[3].targetDisplay = 1;
                playerCameras[3].rect = new Rect(0, 0.5f, 1, 0.5f);
            }
            else if(screenCount == 3)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0, 1, 1);

                playerCameras[1].targetDisplay = 1;
                playerCameras[1].rect = new Rect(0, 0, 1, 1);

                playerCameras[2].targetDisplay = 2;
                playerCameras[2].rect = new Rect(0, 0.5f, 1, 0.5f);

                playerCameras[3].targetDisplay = 2;
                playerCameras[3].rect = new Rect(0, 0, 1, 1);
            }
            else if(screenCount == 4)
            {
                playerCameras[0].targetDisplay = 0;
                playerCameras[0].rect = new Rect(0, 0, 1, 1);

                playerCameras[1].targetDisplay = 1;
                playerCameras[1].rect = new Rect(0, 0, 1, 1);

                playerCameras[2].targetDisplay = 2;
                playerCameras[2].rect = new Rect(0, 0, 1, 1);

                playerCameras[3].targetDisplay = 3;
                playerCameras[3].rect = new Rect(0, 0, 1, 1);
            }
            break;
        }
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
        if(!pointer || playerCameras.Count <= 0)
            return;

        UpdatePointer();
    }
}
