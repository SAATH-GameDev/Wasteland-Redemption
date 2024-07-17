using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

partial class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Player")]
    public PlayerProfile profile;
    public PlayerCharacterProfile characterProfile;
    public GameObject capsuleMesh;

    [Space]
    [Header("Health")]
    public int maxHealth = 100;
    int currentHealth;
    public int damage = 10; //DEBUGGING FOR NOW...

    [Space]
    [Header("References")]
    public Transform displayTransform;
    public Transform movementTransform;
    public float smoothTime = 0.05f;

    private Rigidbody _rigidbody;
    private Vector3 _currentVelocity;

    static public int count = 0;

    void OnEnable()
    {
        count++;
    }

    void OnDisable()
    {
        count--;
    }

    void Start()
    {
        if (movementTransform == null)
            movementTransform = transform;

        _rigidbody = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        displayTransform.LookAt(transform.position + _look);
        displayTransform.rotation = Quaternion.Euler(0.0f, displayTransform.rotation.eulerAngles.y, 0.0f);

        //DEBUGGING
        if (Keyboard.current.qKey.wasPressedThisFrame)
            TakeDamage(damage);
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = _movement.normalized * (profile.speed * characterProfile.speed);
        _rigidbody.linearVelocity = Vector3.SmoothDamp(_rigidbody.linearVelocity, targetVelocity, ref _currentVelocity, smoothTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            print("Player has died");
        }
        else
        {
            print($"Player took {damage} damage, current health: {currentHealth}");
            StartCoroutine(EnablingDamageEffect());
        }
    }

    private IEnumerator EnablingDamageEffect()
    {
        var renderer = capsuleMesh.GetComponentInChildren<Renderer>();
        var material = renderer.material;

        // Enable emission
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", Color.white);

        
        // Scale down
        float duration = 0.075f;
        Vector3 originalScale = capsuleMesh.transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            capsuleMesh.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        capsuleMesh.transform.localScale = targetScale;

        // Scale back up
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            capsuleMesh.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        capsuleMesh.transform.localScale = originalScale;

        yield return new WaitForSeconds(0.075f);
        material.DisableKeyword("_EMISSION");
    }
}
