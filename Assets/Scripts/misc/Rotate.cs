using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10f;
    [System.Serializable]
    enum RotationAxis { X, Y, Z }
    [SerializeField]
    RotationAxis rotationAxis;
    Vector3 totalRotation;
    // Start is called b
    // efore the first frame update
    void Start()
    {
        totalRotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = new Vector3();
        if (rotationAxis == RotationAxis.X)
            rotation = new Vector3(rotationSpeed * Time.deltaTime, 0, 0);
        if (rotationAxis == RotationAxis.Y)
            rotation = new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        if (rotationAxis == RotationAxis.Z)
            rotation = new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        totalRotation += rotation;
        transform.localRotation = Quaternion.Euler(totalRotation);
    }
}
