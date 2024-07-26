using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueGroup group;
    
    [Space]
    public GameObject dialogueBox;
    public Vector3 dialogueBoxTargetOffset;

    [Space]
    public TextMeshProUGUI dialogueText;

    [HideInInspector] public List<string> currentDialogues = new List<string>();

    static public DialogueManager Instance;

    private Transform target;

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
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
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
