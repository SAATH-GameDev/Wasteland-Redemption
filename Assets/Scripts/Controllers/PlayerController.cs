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

    [Header("UI")]
    public GameObject statsUIPrefab;

    [Space]
    public Transform movementTransform;
    public float smoothTime = 0.05f;

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    private float currentHunger = 0.0f;
    private Image hungerBarImage;
    private TextMeshProUGUI equipNameText;
    private TextMeshProUGUI equipCountText;

    private int isWalkingAnimParam = Animator.StringToHash("isWalking");
    
    static public int count = 0;
    static public List<Transform> activePlayers = new List<Transform>();

    void OnEnable()
    {
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
        
        maxHealth = health = profile.health;

        currentHunger = profile.hunger * characterProfile.hunger;

        if(statsUIPrefab)
        {
            GameObject statsUI = Instantiate(statsUIPrefab, GameManager.Instance.canvas.transform);
            equipNameText = statsUI.transform.Find("EquipName").GetComponent<TextMeshProUGUI>();
            equipCountText = statsUI.transform.Find("EquipCount").GetComponent<TextMeshProUGUI>();
            healthBar = statsUI.transform.Find("HealthFill");
            hungerBarImage = statsUI.transform.Find("HungerFill").GetComponent<Image>();
        }
    }

    private void ProcessHunger()
    {
        currentHunger -= GameManager.Instance.gameplay.hungerDepletionRate * Time.deltaTime;
        if(hungerBarImage)
            hungerBarImage.fillAmount = currentHunger / (profile.hunger * characterProfile.hunger);
    }

    override protected void Update()
    {
        //base.Update();

        if(Time.timeScale <= 0.0f) return;

        displayTransform.LookAt(transform.position + _look);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);

        animator.SetBool(isWalkingAnimParam, _movement.sqrMagnitude > 0.25f);

        ProcessHunger();
    }

    void FixedUpdate()
    {
        if(Time.timeScale <= 0.0f) return;
        
        Vector3 targetVelocity = _movement.normalized * (profile.speed * characterProfile.speed);
        _rigidbody.linearVelocity = Vector3.SmoothDamp(_rigidbody.linearVelocity, targetVelocity, ref _currentVelocity, smoothTime);
    }

    public void UpdateEquipment(Profile profile, int count)
    {
        equipNameText.text = profile.name;

        if(profile is WeaponProfile)
            equipCountText.text = count <= 0 ? "<color=red>Reloading</color>" : count.ToString() + "/" + ((WeaponProfile)profile).magazine.ToString();
        else
            equipCountText.text = count.ToString();
    }
}
