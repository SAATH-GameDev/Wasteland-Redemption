using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<Camera> playerCameras = new List<Camera>();

    public int GetScreenCount()
    {
        #if UNITY_EDITOR
        List<DisplayInfo> info = new List<DisplayInfo>();
        Screen.GetDisplayLayout(info);
        return info.Count;
        #else
        return Display.displays.Length;
        #endif
    }

    public void AddPlayerCamera(Camera camera)
    {
        if(GetScreenCount() >= PlayerController.count)
            camera.targetDisplay = PlayerController.count - 1;
        playerCameras.Add(camera);
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
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
