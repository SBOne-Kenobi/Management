using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject InfoPanel;
    public GameObject Name;
    public Player player;
    private GameObject PlayerCanvas;
    public GameObject PlayerCanvasPrefab;
    private SwitchCanvas switchCanvas;

    void Start()
    {
        switchCanvas = FindObjectOfType<SwitchCanvas>();
        PlayerCanvas = Instantiate(PlayerCanvasPrefab);
        PlayerCanvas.transform.Find("PlayerInfo").GetComponent<PlayerInfo>().player = player;
        PlayerCanvas.GetComponent<PlayerCanvas>().player = player;
        PlayerCanvas.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerExit(eventData);
        switchCanvas.GoCanvas(PlayerCanvas);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanel.SetActive(true);
        Name.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanel.SetActive(false);
        Name.SetActive(true);
    }



}
