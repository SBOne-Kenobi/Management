using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerControl : MonoBehaviour, IPunObservable
{
    public Director Director { get; private set; } = new Director();
    public PhotonView PhotonView { get; private set; }
    public int Order = 0;
    public bool IsReady { get; private set; } = true;
    public bool Mutable { get; set; } = false;
    public string Name = "Default";
    public Controller controller;
    public int IconNum { get; private set; } = -1;

    public void SetIcon(int id)
    {
        if (IconNum == id || controller == null)
            return;

        IconNum = id;
        var texture = controller.Icons.Images[id];
        GetComponent<Image>().sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        
    }

    public void Start()
    {
        var parent = FindObjectOfType<GameManager>().Game.transform.Find("CommonCanvas");
        transform.SetParent(parent);

        PhotonView = GetComponent<PhotonView>();
        Name = PhotonView.Owner.NickName;
        var color = new Color();
        if (!PhotonView.IsMine)
            color = new Color(0.990566f, 0.4835739f, 0.03270739f);
        else
            color = new Color(0.3553638f, 1f, 0f);
        transform.Find("PlayerName").GetComponent<Text>().color = color;

        controller = FindObjectOfType<Controller>();
        controller.AddPlayer(this);
    }

    public void GetReady()
    {
        if (Mutable)
            IsReady = true;
    }

    public IEnumerator WaitForReady()
    {
        IsReady = false;
        while (!IsReady)
        {
            //ждем изменения состояния ready 
            yield return null;
        }
        Mutable = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync IsReady, Mutable
        //+ Money, product, materials
        //+ Icon
        if (stream.IsWriting)
        {
            stream.SendNext(IsReady);
            stream.SendNext(Mutable);
            stream.SendNext(Director.Money);
            stream.SendNext(Director.Product);
            stream.SendNext(Director.Materials);
            stream.SendNext(IconNum);
        }
        else
        {
            IsReady = (bool) stream.ReceiveNext();
            Mutable = (bool) stream.ReceiveNext();
            Director.Money = (int) stream.ReceiveNext();
            Director.Product = (int) stream.ReceiveNext();
            Director.Materials = (int) stream.ReceiveNext();
            SetIcon((int)stream.ReceiveNext());
        }
    }
}
