using System.Collections.Generic;
using UnityEngine;

public class PhysicsOnContact : MonoBehaviour
{
    public string targetTag = "Player";

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    void Start()
    {
        Rigidbody parentBody = GetComponent<Rigidbody>();
        if(parentBody != null)
            rigidbodies.Add(parentBody);
        rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());

        foreach(Rigidbody rb in rigidbodies)
            rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag(targetTag))
            foreach(Rigidbody rb in rigidbodies)
                rb.isKinematic = false;
    }
}
