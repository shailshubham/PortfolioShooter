using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] InputData inputData;
    [SerializeField] Transform aimTarget;
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float min, max;
    [SerializeField] GameObject IdleVertualCam;
    [SerializeField] GameObject RunVerturalCam;
    [SerializeField] GameObject WeaponAimVertualCam;
    Vector3 xRot = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        IdleAim();
    }

    // Update is called once per frame
    void Update()
    {
        xRot.x += -inputData.pointerInput.y*sensitivity;
        xRot.x = Mathf.Clamp(xRot.x, min, max);
        aimTarget.localRotation = Quaternion.Euler(xRot);

        float yRot = inputData.pointerInput.x*sensitivity;
        transform.rotation *= Quaternion.Euler(new Vector3(0f,yRot,0f));
    }

    public void IdleAim()
    {
        IdleVertualCam.SetActive(true);
        RunVerturalCam.SetActive(false);
        WeaponAimVertualCam.SetActive(false);
    }
    public void RunAim()
    {
        IdleVertualCam.SetActive(false);
        RunVerturalCam.SetActive(true);
        WeaponAimVertualCam.SetActive(false);
    }
    public void WeaponAim()
    {
        IdleVertualCam.SetActive(false);
        RunVerturalCam.SetActive(false);
        WeaponAimVertualCam.SetActive(true);
    }
}
