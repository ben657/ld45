using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 0.0f;
    [SerializeField]
    Unit target;

    public void SetTarget(Unit unit)
    {
        target = unit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target) Destroy(gameObject);
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0.0f;
        direction.Normalize();

        transform.position += direction * speed * Time.fixedDeltaTime;
        transform.forward = direction;
    }
}
