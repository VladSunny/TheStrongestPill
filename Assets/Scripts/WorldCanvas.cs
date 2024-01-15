using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    public float displayDistance = 20f;
    public GameObject canvas;
    private Transform _cameraTransform;
    
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, _cameraTransform.position);
        canvas.SetActive(distanceToCamera <= displayDistance);
        
        if (canvas.activeSelf)
            transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
