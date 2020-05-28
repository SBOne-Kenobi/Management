using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject InfoPanel;

    [SerializeField]
    private GameObject Name;

    [SerializeField]
    private PlayerControl Player;
    
    private GameObject PlayerCanvas;

    [SerializeField]
    private GameObject PlayerCanvasPrefab;

    private SwitchCanvas SwitchCanvas;

    void Start()
    {
        var controller = FindObjectOfType<Controller>();
        SwitchCanvas = FindObjectOfType<SwitchCanvas>();
        PlayerCanvas = Instantiate(PlayerCanvasPrefab);
        PlayerCanvas.transform.Find("PlayerInfo").GetComponent<PlayerInfo>().player = Player;
        PlayerCanvas.transform.Find("Visitor").GetComponent<PlayerInfo>().player = controller.MinePlayer;
        PlayerCanvas.GetComponent<PlayerCanvas>().player = Player;
        PlayerCanvas.SetActive(false);
        InfoPanel.SetActive(false);
        Name.SetActive(true);
        Name.GetComponent<Text>().text = Player.PhotonView.Owner.NickName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerExit(eventData);
        SwitchCanvas.GoCanvas(PlayerCanvas);
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
