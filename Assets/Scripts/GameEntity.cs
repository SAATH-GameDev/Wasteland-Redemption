using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    public Transform displayTransform;
    public GameObject destroyedVersion;

    [Space]
    public int health;

    public Action OnDamage;

    protected List<Renderer> renderers;
    protected Vector3 baseScale;

    virtual protected void Start()
    {
        renderers = displayTransform.GetComponentsInChildren<Renderer>().ToList();
        baseScale = displayTransform.transform.localScale;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnDamage?.Invoke();
        
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);

            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(EnablingDamageEffect());
        }
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
            var material = renderer.material;
            material.DisableKeyword("_EMISSION");
        }
    }
}