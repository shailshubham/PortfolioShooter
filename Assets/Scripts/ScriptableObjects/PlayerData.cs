using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerData")]
public class PlayerData : ScriptableObject
{
    public Vector3 playerPosition;
    public bool isStealthy = true;
    public bool isUnderAttack = false;
    public bool isCutscenePlaying = false;
}
