using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class AnimationRiggingController : MonoBehaviour
{
    [SerializeField] Rig leftHandRig;
    [SerializeField] Rig rightHandRig;
    [SerializeField] float transitionSmoothness = .1f;

    [HideInInspector]public float leftHandWeight = 0f;
    [HideInInspector]public float rightHandWeight = 1f;

    RigBuilder rigBuilder;
    // Start is called before the first frame update
    void Start()
    {
        rigBuilder = GetComponent<RigBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        if(leftHandRig.weight!=leftHandWeight)
        {
            leftHandRig.weight = Mathf.Lerp(leftHandRig.weight,leftHandWeight, transitionSmoothness);
        }
        if (rightHandRig.weight != rightHandWeight)
        {
            rightHandRig.weight = Mathf.Lerp(rightHandRig.weight, rightHandWeight, transitionSmoothness);
        }
    }
}
