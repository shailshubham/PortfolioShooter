using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Cinematics : MonoBehaviour
{
    PlayableDirector director;
    [SerializeField] List<Placable> placables;
    public UnityEvent onCinematicsEnd;
    [SerializeField] PlayerData playerData;
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
        Invoke("CenematicEnd", (float)director.duration);
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

    void CenematicEnd()
    {
        foreach(Placable placable in placables)
        {
            if(placable.placeOnCinematicEnd)
            {
                placable.Object.transform.position = placable.transformToPlace.position;
                placable.Object.transform.rotation = placable.transformToPlace.rotation;
                placable.Object.SetActive(true);
            }
        }
        playerData.isCutscenePlaying = false;
        if(onCinematicsEnd !=null)
        {
            onCinematicsEnd.Invoke();
        }
        Destroy(gameObject);
    }

    [System.Serializable]
    public class Placable
    {
        public GameObject Object;
        public Transform transformToPlace;
        public bool placeOnCinematicEnd;
    }

}
