using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private string cameraToLookTag;
    [SerializeField] private bool lookToCamera;
    [SerializeField] private Image fillBar; // for changing fill value later on

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
}
