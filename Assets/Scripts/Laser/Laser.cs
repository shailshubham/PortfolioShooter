using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Laser : MonoBehaviour
{
    LineRenderer line;
    [SerializeField]DecalProjector redDot;
    [SerializeField] float redDotSizeMin =.05f, redDotSizeMax =.5f, minDistanceForMaxSizeRedDot = 50f;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out var hit, 500f))
        {
            line.SetPosition(1,transform.InverseTransformPoint(hit.point));
            float dotDistance = Vector3.Distance(transform.position, hit.point);
            float dotScaleMultiplier = dotDistance / 50f;
            dotScaleMultiplier = Mathf.Clamp(dotScaleMultiplier, redDotSizeMin, redDotSizeMax);
            redDot.size = new Vector3(dotScaleMultiplier,dotScaleMultiplier, redDot.size.z);
            line.endWidth = redDotSizeMax*.1f;
        }
        else
        {
            line.SetPosition(1,transform.InverseTransformPoint( transform.position + transform.forward * 500f));
            redDot.size = new Vector3(redDotSizeMax, redDotSizeMax, redDot.size.z);

        }
        
    }
}
