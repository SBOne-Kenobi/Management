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
    public Player player = null;

    virtual protected void Awake()
    {
        if (Prod == null)
            Prod = transform.Find("Prod/ProdVal");
        if (Mat == null)
            Mat = transform.Find("Mat/MatVal");
        if (Mon == null)
            Mon = transform.Find("Mon/MonVal");
        if (PlayerName == null)
            PlayerName = transform.Find("PlayerName");
    }

    virtual protected void Update()
    {
        if (Prod != null)
            Prod.GetComponent<Text>().text = player.Director.Product.ToString();
        if (Mat != null)
            Mat.GetComponent<Text>().text = player.Director.Materials.ToString();
        if (Mon != null)
            Mon.GetComponent<Text>().text = player.Director.Money.ToString();
        if (PlayerName != null)
            PlayerName.GetComponent<Text>().text = player.Name;
    }
}
