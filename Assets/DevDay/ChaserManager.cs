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
            // 목표물이 도망에 성공했을 때
            if (num == 0)
            {
                agent.AddReward(0.1f);
            }

            // 추격자가 체포에 성공했을 떼
            else if (num == 1)
            {
                agent.AddReward(-0.5f);
            }
            else
            {
                continue;
            }
            
            agent.EndEpisode();
        }
    }
}
