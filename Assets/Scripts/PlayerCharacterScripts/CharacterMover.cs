using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHight = 2f;
    public bool isGrounded = false;
    public float GroundDistance;

    CharacterController controller;
    [SerializeField]InputData inputData;

    Vector3 velocity = Vector3.zero;
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
        isGrounded = GroundCheck(out GroundDistance);
        TickGravity();
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

    public bool GroundCheck(out float groundDistance)
    {
        float minimalDist = .1f;
        if (Physics.Raycast(transform.position - transform.up * minimalDist * .5f, -transform.up, out RaycastHit hitInfo))
        {
            float distance = hitInfo.distance;
            if (distance < minimalDist)
            {
                groundDistance = distance;
                return true;
            }
            else
            {
                groundDistance = distance;
                return false;
            }
        }
        groundDistance = 1000f;
        return false;
    }
}
