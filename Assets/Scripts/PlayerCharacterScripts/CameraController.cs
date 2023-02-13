using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] InputData inputData;
    [SerializeField] Transform aimTarget;
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float min, max;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xRot = inputData.pointerInput.y*sensitivity;
        xRot = Mathf.Clamp(xRot, min, max);
        float yRot = inputData.pointerInput.x*sensitivity;
        aimTarget.rotation *= Quaternion.Euler(new Vector3(-xRot,0f,0f));
        transform.rotation *= Quaternion.Euler(new Vector3(0f,yRot,0f));
    }
}
