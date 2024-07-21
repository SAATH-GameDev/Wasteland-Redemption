using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //commit 
    public int forceScreens = 0;

    public static GameManager Instance;

    private List<Camera> playerCameras = new List<Camera>();

    public int GetScreenCount()
    {
        if(forceScreens > 0)
            return forceScreens;

        return Display.displays.Length;
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
