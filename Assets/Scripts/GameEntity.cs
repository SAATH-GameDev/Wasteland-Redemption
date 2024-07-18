using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable
{
    public Transform displayTransform;

    [Space]
    public int health;

    protected List<Renderer> renderers;

    virtual protected void Start()
    {
        renderers = displayTransform.GetComponentsInChildren<Renderer>().ToList();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            health = 0;
            //print($"{gameObject.name} has died");
            Destroy(gameObject);
        }
        else
        {
            //print($"{gameObject.name} took {damage} damage, current health: {health}");
            StartCoroutine(EnablingDamageEffect());
        }
    }

    private IEnumerator EnablingDamageEffect()
    {
        foreach(var renderer in renderers)
        {
            var material = renderer.material;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.white);
        }

        // Scale down
        float duration = 0.075f;
        Vector3 originalScale = displayTransform.transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            displayTransform.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayTransform.transform.localScale = targetScale;

        // Scale back up
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            displayTransform.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        displayTransform.transform.localScale = originalScale;

        yield return new WaitForSeconds(0.075f);

        foreach(var renderer in renderers)
        {
            var material = renderer.material;
            material.DisableKeyword("_EMISSION");
        }
    }
}