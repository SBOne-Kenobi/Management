using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayers : MonoBehaviour
{

    void FixPos(RectTransform obj, int index, int size)
    {
        if (size == 2)
        {
            if (index == 0)
            {
                obj.anchorMin = new Vector2(0.05f, 0.3f);
                obj.anchorMax = new Vector2(0.3f, 0.6f);
            } else
            {
                obj.anchorMin = new Vector2(1f - 0.3f, 0.3f);
                obj.anchorMax = new Vector2(1f - 0.05f, 0.6f);
            }
            obj.offsetMin = new Vector2(0, 0);
            obj.offsetMax = new Vector2(0, 0);
            obj.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SpawnPlayers(List<PlayerControl> players)
    {
        for (int i = 0; i < players.Count; i++)
            FixPos(players[i].GetComponent<RectTransform>(), i, players.Count);
    }
}
