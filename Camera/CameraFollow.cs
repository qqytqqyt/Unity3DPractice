using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Target;

    public float Smoothing = 5f;

    private Vector3 _offset;

	// Use this for initialization
    private void Start ()
    {
        _offset = transform.position - Target.position;
    }
	
	// Update is called once per frame
    private void Update ()
    {
        if (Target == null)
            return;

        var targetCamPos = Target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, Smoothing * Time.deltaTime);
    }
}
