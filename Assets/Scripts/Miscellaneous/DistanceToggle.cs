using System.Collections.Generic;
using UnityEngine;

public class DistanceToggle : MonoBehaviour
{
    public float distanceFactor;
    public float delay = 0.1f;

    private float timer = 0.0f;

    private List<Transform> list = new List<Transform>();

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
            list.Add(transform.GetChild(i));
    }

    void Update()
    {
        if(PlayerController.count <= 0)
            return;
            
        if(timer <= 0.0f)
        {
            foreach(Transform obj in list)
            {
                obj.gameObject.SetActive(false);
                foreach(Transform player in PlayerController.activePlayers)
                {
                    if(Vector3.SqrMagnitude(player.position - obj.position) < distanceFactor)
                    {
                        obj.gameObject.SetActive(true);
                        break;
                    }
                }
            }
            timer = delay;
        }
        else
        {
            timer -= Time.unscaledDeltaTime;
        }
    }
}
