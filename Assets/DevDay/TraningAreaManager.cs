using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraningAreaManager : MonoBehaviour
{
    public RunawayManager runawayManager = null;
    public ChaserManager chaserManager = null;
    // Start is called before the first frame update
    void Start()
    {
        EpisodeStart();
    }

    public void EpisodeStart()
    {
        runawayManager.OnEpisodeBegin();
    }

    public void needEpisodeEnd()
    {
        chaserManager.EndEpisode(2);
        EpisodeStart(); //종료이후 에피소드 재시작
    }
}
