using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : GameEntity
{
    [Header("Player")]
    public PlayerProfile profile;
    public PlayerCharacterProfile characterProfile;

    [Header("Attachments")]
    public Animator animator;
    public WeaponController weapon;
    public GameObject capsuleMesh;
    public Transform headRotatorTarget;

    [Header("UI")]
    public GameObject statsPrefab;
    public GameObject inventoryPrefab;
    public GameObject statusEffectPrefab;

    [Space]
    public Transform movementTransform;
    public float smoothTime = 0.05f;

    private int index = -1;
    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    private float currentHunger = 0.0f;
    private Image hungerBarImage;
    private TextMeshProUGUI equipNameText;
    private TextMeshProUGUI equipCountText;
    private PlayerInventory inventory;
    private StatusEffectController statusEffectController = new StatusEffectController();
    private Transform statusEffectsHolder;

    private int walkX = Animator.StringToHash("walkX");
    private int walkY = Animator.StringToHash("walkY");
    private float _walkX = 0.0f;
    private float _walkY = 0.0f;
    
    static public int count = 0;
    static public List<Transform> activePlayers = new List<Transform>();

    void OnEnable()
    {
        index = count;
        count++;
        activePlayers.Add(transform);
    }

    void OnDisable()
    {
        count--;
        activePlayers.Remove(transform);
    }

    override protected void Start()
    {
        base.Start();

        if (movementTransform == null)
            movementTransform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
        
        maxHealth = health = (int)(profile.health * characterProfile.health);

        currentHunger = profile.hunger * characterProfile.hunger;

        if(statsPrefab)
        {
            GameObject statsUI = Instantiate(statsPrefab, GameManager.Instance.canvas.transform);
            equipNameText = statsUI.transform.Find("EquipName").GetComponent<TextMeshProUGUI>();
            equipCountText = statsUI.transform.Find("EquipCount").GetComponent<TextMeshProUGUI>();
            healthBar = statsUI.transform.Find("HealthFill");
            hungerBarImage = statsUI.transform.Find("HungerFill").GetComponent<Image>();
            statusEffectsHolder = statsUI.transform.Find("StatusEffectsHolder");
        }

        if(inventoryPrefab)
        {
            inventory.UI = Instantiate(inventoryPrefab, GameManager.Instance.canvas.transform);
            inventory.SetupSlots();
            inventory.UI.SetActive(false);
        }

        statusEffectController.Setup();
    }

    public float GetHungerRatio()
    {
        return currentHunger / (profile.hunger * characterProfile.hunger);
    }

    public void IncrementHunger(float value)
    {
        currentHunger = Mathf.Min(currentHunger + value, profile.hunger * characterProfile.hunger);
        hungerBarImage.fillAmount = GetHungerRatio();
    }

    private void ProcessHunger()
    {
        currentHunger -= GameManager.Instance.gameplay.hungerDepletionRate * Time.deltaTime;
        hungerBarImage.fillAmount = GetHungerRatio();

        if(currentHunger <= 0.0f)
        {
            health -= GameManager.Instance.gameplay.hungerHealthDepletionRate * Time.deltaTime;
            OnHealthDecrement();
        }
        else if(hungerBarImage.fillAmount >= GameManager.Instance.gameplay.hungerHealthReplenishMinRatio
        && GetHealthRatio() < GameManager.Instance.gameplay.hungerHealthReplenishLimitRatio)
        {
            health += GameManager.Instance.gameplay.hungerHealthReplenishRate * Time.deltaTime;
        }
    }

    private void ProcessStatusEffects()
    {
        //>>> Values to change
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.HEALTH] = health;
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.HUNGER] = currentHunger;

        //>>> Values to mod
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.ATTACK] = profile.attack;
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.STAMINA] = profile.stamina;
        statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.SPEED] = profile.speed;

        statusEffectController.Update();

        //>>> Update the changed values
        //HEALTH
        float prevHealth = health;
        health = statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.HEALTH];
        UpdateHealthBar();
        if(prevHealth > health)
            OnHealthDecrement();
        //HUNGER
        currentHunger = statusEffectController.attributeValues[(int)StatusEffectProfile.Attribute.HUNGER];
        //Note: Hunger is processed after this function so, no need to update anything
    }

    override protected void Update()
    {
        //base.Update();

        if(Time.timeScale <= 0.0f) return;

        Quaternion prevYRot = displayTransform.rotation;
        displayTransform.LookAt(GameManager.Instance.pointer);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);
        displayTransform.rotation = Quaternion.Slerp(prevYRot, displayTransform.rotation, 7.0f * Time.deltaTime);

        headRotatorTarget.position = GameManager.Instance.pointer.position;

        if(inventory.UI.activeSelf)
            inventory.UI.transform.position = GameManager.Instance.WorldToScreenPosition(transform.position, index) + new Vector3(inventory.offset.x * Screen.width, inventory.offset.y * Screen.height, 0.0f);
        
        _walkX = Mathf.Lerp(_walkX, Vector3.Dot(displayTransform.right, _movement), 16.0f * Time.deltaTime);
        animator.SetFloat(walkX, _walkX);

        _walkY = Mathf.Lerp(_walkY, Vector3.Dot(displayTransform.forward, _movement), 16.0f * Time.deltaTime);
        animator.SetFloat(walkY, _walkY);

        ProcessStatusEffects();
        ProcessHunger();
    }

    void FixedUpdate()
    {
        if(Time.timeScale <= 0.0f) return;
        
        Vector3 targetVelocity = _movement.normalized * ((profile.speed + statusEffectController.modValues[(int)StatusEffectProfile.Attribute.SPEED]) * characterProfile.speed);
        _rigidbody.linearVelocity = Vector3.SmoothDamp(_rigidbody.linearVelocity, targetVelocity, ref _currentVelocity, smoothTime);
    }

    public void UpdateEquipment(Profile profile, int count)
    {
        if(!profile)
        {
            equipNameText.text = equipCountText.text = "";
            return;
        }

        equipNameText.text = profile.name;

        if(profile is WeaponProfile)
            equipCountText.text = count <= 0 ? "<color=red>Reloading</color>" : count.ToString() + "/" + ((WeaponProfile)profile).magazine.ToString();
        else
            equipCountText.text = count.ToString();
    }

    public void AddStatusEffect(StatusEffectProfile effect)
    {
        statusEffectController.Add(effect, statusEffectPrefab, statusEffectsHolder);
    }
}
