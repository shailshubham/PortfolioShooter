using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationSystem : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SetupAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupAnimator()
    {
        Animator childAnim = transform.GetChild(0).GetComponent <Animator>();
        Avatar childAvtar = childAnim.avatar;
        anim.avatar = childAvtar;
        Destroy(childAnim);
    }
}
