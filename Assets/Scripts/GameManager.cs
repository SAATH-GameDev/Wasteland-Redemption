using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<Camera> playerCameras = new List<Camera>();

    public int GetScreenCount()
    {
        return Display.displays.Length;
        
        //List<DisplayInfo> info = new List<DisplayInfo>();
        //Screen.GetDisplayLayout(info);
        //return info.Count;
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
        int screenCount = GetScreenCount();
        for(int i = 0; i < screenCount; i++)
            Display.displays[i].Activate();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
