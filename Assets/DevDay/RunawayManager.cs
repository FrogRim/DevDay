using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayManager : MonoBehaviour
{
    public GameObject targetPrefab = null;
    Vector3 transformP = Vector3.zero;
    public Goalgoing Goalgoing;
    public ChaserManager chaserManager;
    Vector3 startPos;
    

    public void OnEpisodeBegin()
    {

        Goalgoing.GetComponent<Goalgoing>().nmAgent.enabled = false;
        
        chaserManager.EndEpisode(2);
        Debug.Log("arrive");
        this.transform.localPosition = startPos;
        Goalgoing.GetComponent<Goalgoing>().nmAgent.enabled = true;
        


    }
}
