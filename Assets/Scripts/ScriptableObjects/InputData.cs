using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyAsset/InputData")]
public class InputData : ScriptableObject
{
    public Vector2 dpadInput = Vector2.zero;
    public Vector2 pointerInput= Vector2.zero;
    public Vector2 pointerInputPosition = Vector2.zero;
    public bool jump = false;
    public bool run = false;
    public bool Aim = false;
    public bool shoot = false;
    public bool reload = false;
}
