using UnityEngine;
using UnityEngine.UI;

partial class PlayerController : GameEntity
{
    [Header("Player")]
    public PlayerProfile profile;
    public PlayerCharacterProfile characterProfile;

    [Header("Attachments")]
    public WeaponController weapon;
    public GameObject capsuleMesh;

    [Header("UI")]
    public GameObject hungerBarPrefab;

    [Space]
    public Transform movementTransform;
    public float smoothTime = 0.05f;

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    private float currentHunger = 0.0f;
    private Image hungerBarImage;
    
    static public int count = 0;

    void OnEnable()
    {
        count++;
    }

    void OnDisable()
    {
        count--;
    }

    override protected void Start()
    {
        base.Start();

        if (movementTransform == null)
            movementTransform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        
        maxHealth = health = profile.health;

        currentHunger = profile.hunger * characterProfile.hunger;
        if(hungerBarPrefab)
            hungerBarImage = Instantiate(hungerBarPrefab, GameManager.Instance.canvas.transform).GetComponent<Image>();
    }

    private void ProcessHunger()
    {
        currentHunger -= GameManager.Instance.gameplay.hungerDepletionRate * Time.deltaTime;
        if(hungerBarImage)
            hungerBarImage.fillAmount = currentHunger / (profile.hunger * characterProfile.hunger);
    }

    override protected void Update()
    {
        base.Update();

        if(Time.timeScale <= 0.0f) return;

        displayTransform.LookAt(transform.position + _look);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);

        ProcessHunger();
    }

    void FixedUpdate()
    {
        if(Time.timeScale <= 0.0f) return;
        
        Vector3 targetVelocity = _movement.normalized * (profile.speed * characterProfile.speed);
        _rigidbody.linearVelocity = Vector3.SmoothDamp(_rigidbody.linearVelocity, targetVelocity, ref _currentVelocity, smoothTime);
    }
}
