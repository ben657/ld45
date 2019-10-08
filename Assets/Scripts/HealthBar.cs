using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
    public Transform frontBar;
    
    public void SetPercentage(float percentage)
    {
        Vector3 scale = frontBar.localScale;
        scale.x = percentage;
        frontBar.localScale = scale;
    }

    private void Update()
    {
        transform.forward = -Camera.main.transform.forward;
    }
}
