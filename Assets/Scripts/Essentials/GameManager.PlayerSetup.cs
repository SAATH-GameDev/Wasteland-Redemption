using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

partial class GameManager : MonoBehaviour
{
    [Space]
    public int forceScreens = 0;
    public Transform playerSpawns;
    public Color[] playerColors;
    public GameObject playerSetupUIPrefab;

    [Space]
    public bool ready = false;

    public class PlayerSetup
    {
        public Transform ui;
        public PlayerController controller;

        public int screen;
        public string grid;
        public bool ready = false;
    }
    public List<PlayerSetup> setups = new List<PlayerSetup>();

    public int GetScreenCount()
    {
        if(forceScreens > 0)
            return forceScreens;

        return Display.displays.Length;
    }

    public void AddCamera(Camera camera)
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
                playerCameras[0].rect = new Rect(0, 0, 0.5f, 1.0f);

                playerCameras[1].targetDisplay = 0;
                playerCameras[1].rect = new Rect(0.5f, 0, 0.5f, 1.0f);
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
                playerCameras[0].rect = new Rect(0, 0, 0.5f, 1.0f);

                playerCameras[1].targetDisplay = 0;
                playerCameras[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);

                playerCameras[2].targetDisplay = 0;
                playerCameras[2].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
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
                playerCameras[2].rect = new Rect(0, 0, 1, 0.5f);

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

    private void SetRect(RectTransform r, float w, float h, float x, float y)
    {
        r.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        r.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        r.localPosition = new Vector3(x, y, 0.0f);
    }

    public void UpdateSetup(PlayerSetup setup)
    {
        Vector2 size = canvas.GetComponent<RectTransform>().sizeDelta;
        RectTransform r = setup.ui.GetComponent<RectTransform>();
        Camera cam = playerCameras[setup.controller.GetIndex()];
        //cam.targetDisplay = setup.screen; //TEMP
        switch(setup.grid)
        {
            case "####":
                SetRect(r, size.x, size.y, 0.0f, 0.0f);
                cam.rect = new Rect(0, 0, 1, 1);
                break;
            case "#0#0":
                SetRect(r, size.x / 2.0f, size.y, -size.x / 4.0f, 0.0f);
                cam.rect = new Rect(0, 0, 0.5f, 1);
                break;
            case "0#0#":
                SetRect(r, size.x / 2.0f, size.y, size.x / 4.0f, 0.0f);
                cam.rect = new Rect(0.5f, 0, 0.5f, 1);
                break;
            case "##00":
                SetRect(r, size.x, size.y / 2.0f, 0.0f, -size.y / 4.0f);
                cam.rect = new Rect(0, 0, 1, 0.5f);
                break;
            case "00##":
                SetRect(r, size.x, size.y / 2.0f, 0.0f, size.y / 4.0f);
                cam.rect = new Rect(0, 0.5f, 1, 0.5f);
                break;
            case "#000":
                SetRect(r, size.x / 2.0f, size.y / 2.0f, -size.x / 4.0f, -size.y / 4.0f);
                cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                break;
            case "0#00":
                SetRect(r, size.x / 2.0f, size.y / 2.0f, size.x / 4.0f, -size.y / 4.0f);
                cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
            case "00#0":
                SetRect(r, size.x / 2.0f, size.y / 2.0f, -size.x / 4.0f, size.y / 4.0f);
                cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                break;
            case "000#":
                SetRect(r, size.x / 2.0f, size.y / 2.0f, size.x / 4.0f, size.y / 4.0f);
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                break;
        }
    }

    public PlayerSetup GetSetupOfIndex(int index)
    {
        foreach(PlayerSetup setup in setups)
            if(setup.controller.GetIndex() == index)
                return setup;
        return null;
    }

    public Transform GetPlayerUI(int index)
    {
        foreach(PlayerSetup setup in setups)
            if(setup.controller.GetIndex() == index)
                return setup.ui;
        return null;
    }

    public PlayerSetup GetSetupOfGrid(string grid, int screen = 0)
    {
        foreach(PlayerSetup setup in setups)
            if(setup.grid.Equals(grid))// && setup.screen == screen) //TEMP: IGNORING SCREENS
                return setup;
        return null;
    }

    public void MoveSetupInX(PlayerSetup setup, bool isNegative)
    {
        string prevGrid = setup.grid;
        int prevScreen = setup.screen;
        PlayerSetup swapSetup = null;
        switch(setup.grid)
        {
            case "####":
                setup.screen += isNegative ? -1 : 1;
                swapSetup = GetSetupOfGrid("####", setup.screen);
                setup.grid = "####";
                break;
            case "#0#0":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid("0#0#", setup.screen);
                setup.grid = "0#0#";
                break;
            case "0#0#":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid("#0#0", setup.screen);
                setup.grid = "#0#0";
                break;
            case "##00":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid(isNegative ? "00##" : "00##", setup.screen);
                setup.grid = isNegative ? "00##" : "00##";
                break;
            case "00##":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid(isNegative ? "##00" : "##00", setup.screen);
                setup.grid = isNegative ? "##00" : "##00";
                break;
            case "#000":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid(isNegative ? "0#00" : "0#00", setup.screen);
                setup.grid = isNegative ? "0#00" : "0#00";
                break;
            case "0#00":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid(isNegative ? "#000" : "#000", setup.screen);
                setup.grid = isNegative ? "#000" : "#000";
                break;
            case "00#0":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid(isNegative ? "000#" : "000#", setup.screen);
                setup.grid = isNegative ? "000#" : "000#";
                break;
            case "000#":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid(isNegative ? "00#0" : "00#0", setup.screen);
                setup.grid = isNegative ? "00#0" : "00#0";
                break;
        }
        if(swapSetup != null)
        {
            swapSetup.screen = prevScreen;
            swapSetup.grid = prevGrid;
        }

        foreach(PlayerSetup s in setups)
            UpdateSetup(s);
    }

    public void MoveSetupInY(PlayerSetup setup, bool isNegative)
    {
        string prevGrid = setup.grid;
        int prevScreen = setup.screen;
        PlayerSetup swapSetup = null;
        switch(setup.grid)
        {
            case "####":
                setup.screen += isNegative ? -1 : 1;
                swapSetup = GetSetupOfGrid("####", setup.screen);
                setup.grid = "####";
                break;
            case "#0#0":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid("0#0#", setup.screen);
                setup.grid = "0#0#";
                break;
            case "0#0#":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid("#0#0", setup.screen);
                setup.grid = "#0#0";
                break;
            case "##00":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid(isNegative ? "00##" : "00##", setup.screen);
                setup.grid = isNegative ? "00##" : "00##";
                break;
            case "00##":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid(isNegative ? "##00" : "##00", setup.screen);
                setup.grid = isNegative ? "##00" : "##00";
                break;
            case "#000":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid(isNegative ? "00#0" : "00#0", setup.screen);
                setup.grid = isNegative ? "00#0" : "00#0";
                break;
            case "0#00":
                setup.screen += isNegative ? -1 : 0;
                swapSetup = GetSetupOfGrid(isNegative ? "000#" : "000#", setup.screen);
                setup.grid = isNegative ? "000#" : "000#";
                break;
            case "00#0":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid(isNegative ? "#000" : "0#00", setup.screen);
                setup.grid = isNegative ? "#000" : "0#00";
                break;
            case "000#":
                setup.screen += isNegative ? 0 : 1;
                swapSetup = GetSetupOfGrid(isNegative ? "0#00" : "0#00", setup.screen);
                setup.grid = isNegative ? "0#00" : "0#00";
                break;
        }
        if(swapSetup != null)
        {
            swapSetup.screen = prevScreen;
            swapSetup.grid = prevGrid;
        }

        foreach(PlayerSetup s in setups)
            UpdateSetup(s);
    }

    public void AddSetupUI(PlayerController controller)
    {
        PlayerSetup setup = new PlayerSetup
        {
            controller = controller,
            ui = Instantiate(playerSetupUIPrefab, canvas).transform,

            //TEMP
            screen = 0
        };
        setups.Add(setup);

        controller.transform.parent.position = playerSpawns.GetChild(controller.GetIndex()).position;

        switch(setups.Count)
        {
            case 1:
                setups[0].grid = "####";
                break;

            case 2:
                setups[0].grid = "#0#0";
                setups[1].grid = "0#0#";
                break;

            case 3:
                setups[0].grid = "#0#0";
                setups[1].grid = "0#00";
                setups[2].grid = "000#";
                break;

            case 4:
                setups[0].grid = "#000";
                setups[1].grid = "0#00";
                setups[2].grid = "00#0";
                setups[3].grid = "000#";
                break;
        }
        foreach(PlayerSetup s in setups)
            UpdateSetup(s);

        Transform setupUI = setup.ui.GetChild(0);
        TextMeshProUGUI[] texts = setupUI.GetComponentsInChildren<TextMeshProUGUI>();
        setupUI.GetComponent<Image>().color = texts[0].color = texts[1].color = texts[2].color = playerColors[controller.GetIndex()];
        texts[0].text = "Player " + (controller.GetIndex() + 1).ToString();
        texts[1].text = setup.controller.GetComponent<PlayerInput>().currentControlScheme;
        texts[2].text = texts[1].text == "Gamepad" ? "Press Left Shoulder to get ready!" : "Press TAB to get ready!";
    }

    public Transform GetSetupUI(PlayerController controller)
    {
        foreach(PlayerSetup setup in setups)
            if(setup.controller == controller)
                return setup.ui;
        return null;
    }

    public void ReadySetup(PlayerSetup setup)
    {
        if(setup == null)
            return;

        setup.ready = true;
        setup.ui.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Ready! Wait for others...";

        ready = AreSetupsReady();

        if(ready)
        {
            foreach(PlayerSetup s in setups)
                s.ui.GetChild(0).gameObject.SetActive(false);
            Destroy(GetComponent<PlayerInputManager>());
            Time.timeScale = 1.0f;
        }
    }

    public bool AreSetupsReady()
    {
        foreach(PlayerSetup setup in setups)
            if(!setup.ready)
                return false;
        return true;
    }
}