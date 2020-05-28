using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Transform Prod = null;
    public Transform Mat = null;
    public Transform Mon = null;
    public Transform PlayerName = null;
    public PlayerControl player = null;

    virtual protected void Awake()
    {
        if (Prod == null)
            Prod = transform.Find("Prod");
        if (Mat == null)
            Mat = transform.Find("Mat");
        if (Mon == null)
            Mon = transform.Find("Mon");
        if (PlayerName == null)
            PlayerName = transform.Find("PlayerName");
    }

    virtual protected void Update()
    {
        if (Prod != null)
            Prod.GetComponent<Text>().text = "P: " + player.Director.Product.ToString();
        if (Mat != null)
            Mat.GetComponent<Text>().text = "M: " + player.Director.Materials.ToString();
        if (Mon != null)
            Mon.GetComponent<Text>().text = "$: " + player.Director.Money.ToString();
        if (PlayerName != null)
            PlayerName.GetComponent<Text>().text = player.Name;
    }
}
