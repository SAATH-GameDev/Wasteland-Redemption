using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public int index = -1;
    public int targetIndex = -1;

    public List<string> dialogues = new List<string>();

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    static public DialogueManager Instance;

    public void PlaySingleDialogue(int fromIndex = -1)
    {
        if(fromIndex > -1) index = fromIndex;
        targetIndex = fromIndex + 1;
    }

    public void PlayDialogues(int fromIndex = -1, int toIndex = -1)
    {
        if(fromIndex > -1) index = fromIndex;
        if(toIndex > -1) targetIndex = toIndex;
    }

    public void PlayDialoguesForLength(int fromIndex = -1, int length = 0)
    {
        if(fromIndex > -1) index = fromIndex;
        if(length > 0) targetIndex = index + length;
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(PlayerController.count <= 0) return;
        
        if(index > -1 && index < dialogues.Count - 1 && index < targetIndex)
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
        if(index < dialogues.Count - 1 && index < targetIndex)
        {
            index++;
            dialogueText.text = dialogues[index];
            return true;
        }

        dialogueBox.SetActive(false);
        Time.timeScale = 1.0f;
        return false;
    }
}
