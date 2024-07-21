using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float chaseSpeed;
    public float stopDistance = 0.1f; // Distance at which the enemy will stop chasing

    private float distance;

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Check if the enemy is further than the stop distance
        if (distance > stopDistance)
        {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}

