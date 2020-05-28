using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private GameObject Icon;

    [SerializeField]
    private GameObject ChoosesIcon;

    [SerializeField]
    private GameObject Sets;
    
    private ListIcons Icons;

    [SerializeField]
    private GameObject ListIconsPrefab;

    private int CurrentImage;

    private void Start()
    {
        Icons = Instantiate(ListIconsPrefab).GetComponent<ListIcons>();
        CurrentImage = PlayerPrefs.GetInt("Icon", 0);
        var drop = ChoosesIcon.GetComponent<Dropdown>();
        drop.AddOptions(Icons.Images.Select(p => p.name).ToList());
        drop.value = CurrentImage;
        SyncIcon();
        Close();
    }

    public void SyncIcon()
    {
        CurrentImage = ChoosesIcon.GetComponent<Dropdown>().value;
        PlayerPrefs.SetInt("Icon", CurrentImage);

        var texture = Icons.Images[CurrentImage];
        Icon.GetComponent<Image>().sprite = Sprite.Create(
            texture, 
            new Rect(0, 0, texture.width, texture.height), 
            new Vector2(0.5f, 0.5f));
    }

    public void Close()
    {
        Sets.SetActive(false);
    }

    public void Open()
    {
        Sets.SetActive(true);
    }

}
