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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        inputData.dpadInput = new Vector2(x, y);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inputData.jump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            inputData.jump = false;
        }
    }
}
