using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserManager : MonoBehaviour
{
    public List<ChaserAgent> agentList = new List<ChaserAgent>();

    public void EndEpisode(int num)
    {
        foreach (var agent in agentList)
        {
            if (num == 0)
            {
                agent.SetReward(-0.06f);
            }
            else if (num == 1)
            {
                agent.SetReward(15f);
            }
            else
            {
                continue;
            }
            
            agent.EndEpisode();
        }
    }
}
