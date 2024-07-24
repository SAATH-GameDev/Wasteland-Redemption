using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : GameEntity
{

    private float maxHunger;
    public float currentHunger;
    private float delayInHungerVal = 0.01f;
    public float hungerPerc => currentHunger / maxHunger;
    public Image hungerBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHunger = base.hungerVal;
        currentHunger = maxHunger;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.isDialogEnd == true)
        {
            currentHunger -= delayInHungerVal * Time.deltaTime;
            hungerBar.fillAmount = currentHunger;
        }
      
    }
}
