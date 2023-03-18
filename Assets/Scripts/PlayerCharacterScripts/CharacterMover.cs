using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHight = 2f;



    [HideInInspector]public bool isGrounded = false;
    public float GroundDistance;
    [SerializeField] GameObject groundCheckTransform;
    [SerializeField]LayerMask layer;

    CharacterController controller;
    [SerializeField]InputData inputData;

    public Vector3 velocity = Vector3.zero;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TickGravity();
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheckNew(out GroundDistance);
        Debug.Log(isGrounded);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHight * -gravity);
    }

    public void MoveCharacter(float speedMultiplier)
    {
        Vector3 dir = controller.transform.forward * inputData.dpadInput.y + controller.transform.right * inputData.dpadInput.x;
        controller.Move(dir * speed * Time.deltaTime *speedMultiplier);
    }

    void TickGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public bool GroundCheckNew(out float hitDistance)
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, layer);
        hitDistance = hitInfo.distance;
        return Physics.CheckSphere(transform.position, .25f, layer);
    }
}
