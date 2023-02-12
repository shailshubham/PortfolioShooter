using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    public static GameInputSystem instance;
    [SerializeField] InputData inputData;
    private void Awake()
    {
        if (instance == null) { instance= this; }
        else if(instance !=this) { Destroy(gameObject); }
    }
    // Start is called before the first frame update
    void Start()
    {
        inputData.dpadInput = Vector2.zero;
        inputData.pointerInput= Vector2.zero;
        inputData.jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        inputData.dpadInput.x = Input.GetAxis("Horizontal");
        inputData.dpadInput.y = Input.GetAxis("Vertical");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inputData.jump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            inputData.jump = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            inputData.run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            inputData.run = false;
        }
    }
}
