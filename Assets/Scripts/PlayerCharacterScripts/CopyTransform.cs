using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] Transform copyObject;
    public enum CopyType { CopyFrom,CopyTo };
    public CopyType copyType = CopyType.CopyFrom;
    [SerializeField] bool position = true;
    [SerializeField] bool rotation = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(copyType == CopyType.CopyTo)
        {
            if(position)
                copyObject.transform.position = transform.position;
            if(rotation)
                copyObject.transform.rotation = transform.rotation;
        }
        if (copyType == CopyType.CopyFrom)
        {
            if (position)
                transform.position = copyObject.transform.position;

            if(rotation)
                transform.rotation = copyObject.transform.rotation;
        }
    }
}
