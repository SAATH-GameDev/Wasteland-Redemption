using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class GameEntity : MonoBehaviour, IDamageable
{
    public Transform displayTransform;

    [Space]
    public int health;
    public UnityEvent OnDamage;
    public UnityEvent OnDeath;

    [Space]
    public GameObject healthBarPrefab;
    public Vector3 healthBarOffset;
    [Space]

    protected List<Renderer> renderers;
    protected Vector3 baseScale;
    
    protected int maxHealth;
    protected Transform healthBar;

    virtual protected void Start()
    {
        renderers = displayTransform.GetComponentsInChildren<Renderer>().ToList();
        baseScale = displayTransform.transform.localScale;

        if(healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity).transform;
            healthBar.transform.parent = GameManager.Instance.canvas.GetChild(0);
        }
    }

    virtual protected void Update()
    {
        if(healthBar)
            healthBar.position = GameManager.Instance.WorldToScreenPosition(transform.position) + healthBarOffset;
    }

    public void UpdateHealthBar()
    {
        if(healthBar)
        {
            if(healthBar.childCount > 0)
                healthBar.GetChild(0).GetComponent<Image>().fillAmount = (float)health / maxHealth;
            else
                healthBar.GetComponent<Image>().fillAmount = (float)health / maxHealth;
        }
    }

    public void OnHealthDecrement(bool effect = true)
    {
        OnDamage?.Invoke();
        if (health <= 0)
        {
            health = 0;
            if(healthBar)
                Destroy(healthBar.gameObject);
            Destroy(gameObject);
            OnDeath?.Invoke();
        }
        else if(effect)
        {
            StartCoroutine(EnablingDamageEffect());
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        OnHealthDecrement();
    }

    private IEnumerator EnablingDamageEffect()
    {
        if(renderers == null)
            Start();

        foreach(Renderer renderer in renderers)
        {
            if(!renderer || !renderer.material) continue;

            Material material = renderer.material;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.white);
        }

        // Scale down
        float duration = 0.05f;
        Vector3 targetScale = baseScale * 1.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            displayTransform.transform.localScale = Vector3.Lerp(baseScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayTransform.transform.localScale = targetScale;

        // Scale back up
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            displayTransform.transform.localScale = Vector3.Lerp(targetScale, baseScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayTransform.transform.localScale = baseScale;

        yield return new WaitForSeconds(0.05f);

        foreach(var renderer in renderers)
        {
            if(!renderer)
                continue;
            var material = renderer.material;
            material.DisableKeyword("_EMISSION");
        }
    }
}