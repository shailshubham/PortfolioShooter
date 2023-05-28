using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    public static GameInputSystem instance;
    [SerializeField] InputData inputData;
    [SerializeField] PlayerData playerData;
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
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        inputData.dpadInput.x = x;
        inputData.dpadInput.y = y;

        float  pointerX = Input.GetAxis("Mouse X");
        float pointerY = Input.GetAxis("Mouse Y");

        inputData.pointerInputPosition = Input.mousePosition;

        inputData.pointerInput.x = pointerX;
        inputData.pointerInput.y = pointerY;


        if (Input.GetKeyDown(KeyCode.Space))
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

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            inputData.Aim = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            inputData.Aim = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            inputData.shoot = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            inputData.shoot = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            inputData.reload = true;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            inputData.reload = false;
        }

        if((Input.GetKeyDown(KeyCode.Tab)||Input.GetKeyDown(KeyCode.I))&&!playerData.isCutscenePlaying)
        {
            Inventory.instance.gameObject.SetActive(!Inventory.instance.gameObject.activeSelf);
        }
    }
}
