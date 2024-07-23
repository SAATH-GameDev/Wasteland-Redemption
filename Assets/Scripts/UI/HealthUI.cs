using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private string cameraToLookTag;
    [SerializeField] private bool lookToCamera;
    [SerializeField] private Image fillBar; // for changing fill value later on
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth = 10000f;

    public int damageTaken = 0;
    public bool DT = false;


    private void Start()
    {
        if (!lookToCamera) return;

        GameObject targetObject = GameObject.FindGameObjectWithTag(cameraToLookTag);
        if (targetObject)
        {
            SetupLookAtConstraint(targetObject);
        }
        else
        {
            StartCoroutine(WaitForPlayer());
        }

        _currentHealth = _maxHealth;
    }

    private IEnumerator WaitForPlayer()
    {
        while (true)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag(cameraToLookTag);
            if (targetObject)
            {
                SetupLookAtConstraint(targetObject);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SetupLookAtConstraint(GameObject targetObject)
    {
        LookAtConstraint lookAtConstraint = gameObject.AddComponent<LookAtConstraint>();
        lookAtConstraint.AddSource(new ConstraintSource { sourceTransform = targetObject.transform, weight = 1f });
        lookAtConstraint.constraintActive = true;
    }

    void Update()
    {
        if (DT == true)
        {
            _currentHealth -= damageTaken;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            DecreaseHealth();
            DT = false;
        }
    }
    void DecreaseHealth()
    {
        float _healthPercentage = _currentHealth / _maxHealth;
        fillBar.rectTransform.localScale = new Vector3(_healthPercentage, 1, 1);
    }
}
