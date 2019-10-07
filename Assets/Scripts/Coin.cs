using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float life = 1.0f;
    public float force = 0.0f;

    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        Vector3 direction = Vector3.forward;
        direction = Quaternion.AngleAxis(Random.Range(0, 360.0f), Vector3.up) * direction;
        direction = Quaternion.AngleAxis(Random.Range(0, 90.0f), -Vector3.Cross(Vector3.up, direction)) * direction;
        Debug.Log(direction);
        body.AddForce(direction * force);
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }
}
