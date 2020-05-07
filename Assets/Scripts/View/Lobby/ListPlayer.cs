using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPlayer : MonoBehaviour
{
    public List<PlayerItem> List { get; private set; } = new List<PlayerItem>();

    void FixRect(int index, RectTransform rt)
    {
        rt.anchorMin = new Vector2(0f, 1f - ((index + 1) / 8f));
        rt.anchorMax = new Vector2(1f, 1f - (index / 8f));
        rt.offsetMax = new Vector2(-1, -3);
        rt.offsetMin = new Vector2(1, 3);
    }

    void FixRectNew(RectTransform rt)
    {
        FixRect(List.Count, rt);
    }

    public void Add(PlayerItem newPlayer)
    {
        GameObject obj = newPlayer.gameObject;

        obj.transform.SetParent(transform);
        FixRectNew(obj.GetComponent<RectTransform>());

        List.Add(newPlayer);
    }

    public void Remove(PlayerItem Player)
    {
        List.Remove(Player);
        for (int i = 0; i < List.Count; i++)
        {
            FixRect(i, List[i].GetComponent<RectTransform>());
        }
    }
}
