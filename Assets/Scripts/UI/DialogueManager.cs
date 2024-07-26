using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public DialogueGroup group;
    
    [Space]
    public GameObject dialogueBox;
    public Vector3 dialogueBoxTargetOffset;

    [Space]
    public TextMeshProUGUI dialogueText;

    [Serializable]
    public class CommandEventPair
    {
        public string name;
        public UnityEvent eventsToInvoke;
    }

    [Space]
    public List<CommandEventPair> commands = new List<CommandEventPair>();

    [HideInInspector] public List<string> currentDialogues = new List<string>();

    static public DialogueManager Instance;

    private Transform target;

    public void Play(int index)
    {
        currentDialogues.AddRange(group.Get(index));
        Proceed();
    }

    public void Play(string tag)
    {
        currentDialogues.AddRange(group.Get(tag));
        Proceed();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
    }

    void Awake()
    {
        Instance = this;
        Play(0);
    }

    void Update()
    {
        if(PlayerController.count <= 0)
            return;

        if(target == null)
        {
            dialogueBox.transform.position = GameManager.Instance.WorldToScreenPosition(PlayerController.activePlayers[0].transform.position) + dialogueBoxTargetOffset;
        }
        else
        {
            dialogueBox.transform.position = GameManager.Instance.WorldToScreenPosition(target.position) + dialogueBoxTargetOffset;
        }
        
        if(currentDialogues.Count > 0)
        {
            dialogueBox.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    
    public bool IsActive()
    {
        return dialogueBox.activeSelf;
    }

    private string ProcessCommands(string text)
    {
        int limit = 100;
        while(limit > 0)
        {
            int commandStart = text.IndexOf("[");

            if(commandStart <= -1)
                return text;

            int commandEnd = text.IndexOf("]");

            if(commandEnd <= -1)
                return text;

            string commandString = text.Substring(commandStart, commandEnd - commandStart + 1);
            foreach(var command in commands)
                if(("[" + command.name + "]").Equals(commandString))
                    command.eventsToInvoke.Invoke();

            text = text.Remove(commandStart, commandEnd - commandStart + 1);

            limit--;
        }
        return text;
    }

    public bool Proceed()
    {
        if(currentDialogues.Count > 0)
        {
            dialogueText.text = ProcessCommands(currentDialogues[0]);
            currentDialogues.RemoveAt(0);
            return true;
        }

        dialogueBox.SetActive(false);
        Time.timeScale = 1.0f;
        return false;
    }
}
