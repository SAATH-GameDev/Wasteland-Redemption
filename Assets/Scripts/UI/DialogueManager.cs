using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueGroup group;

    public List<string> currentDialogues = new List<string>();

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    static public DialogueManager Instance;

    public void Play(int index)
    {
        currentDialogues.AddRange(group.Get(index));

        dialogueText.text = currentDialogues[0];
        currentDialogues.RemoveAt(0);
    }

    public void Play(string tag)
    {
        currentDialogues.AddRange(group.Get(tag));

        dialogueText.text = currentDialogues[0];
        currentDialogues.RemoveAt(0);
    }

    void Awake()
    {
        Instance = this;

        //TEMPORARY; for testing
        Play(0);
    }

    void Update()
    {
        if(PlayerController.count <= 0) return;
        
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

    public bool Proceed()
    {
        if(currentDialogues.Count > 0)
        {
            dialogueText.text = currentDialogues[0];
            currentDialogues.RemoveAt(0);
            return true;
        }

        dialogueBox.SetActive(false);
        Time.timeScale = 1.0f;
        return false;
    }
}
