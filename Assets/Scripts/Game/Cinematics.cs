using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cinematics : MonoBehaviour
{
    PlayableDirector director;
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform truck, truckPos;
    [SerializeField] PlayerData playerData;
    public bool cinematicEndPlacement = true;
    public bool playOnStart = true;
    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        if(playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        director.Play();
        Invoke("OnCenematicEnd", (float)director.duration);
        playerData.isCutscenePlaying = true;
        Inventory.instance.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    void OnCenematicEnd()
    {
        if(cinematicEndPlacement)
        {
            truck.position = truckPos.position;
            GameManager.instance.player.transform.position = playerPosition.position;
            GameManager.instance.player.SetActive(true);
        }

        Destroy(gameObject);
        playerData.isCutscenePlaying = false;
    }
}
