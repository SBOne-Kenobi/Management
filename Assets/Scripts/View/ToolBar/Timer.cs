using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Management;
using Photon.Pun;

public class Timer : MonoBehaviour, IPunObservable
{
    float CurrentTime = 0;
    Controller controller;
    SwitchCanvas switcher;

    private void Awake()
    {
        controller = FindObjectOfType<Controller>();
        switcher = FindObjectOfType<SwitchCanvas>();
    }

    public IEnumerator StartTimer(int time)
    {
        if (time != 0)
        {
            CurrentTime = time;
            GameState state = controller.game.State.CurrentState;
            while (CurrentTime > 0 && state == controller.game.State.CurrentState)
            {
                yield return null;
                CurrentTime -= Time.deltaTime;
            }
            CurrentTime = 0f;
            if (state == controller.game.State.CurrentState)
            {
                switcher.GoCommon();
                foreach (PlayerControl player in controller.Players)
                    player.GetReady();
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!controller.IsReadyToGoNext && !controller.AllPlayersReady && CurrentTime == 0)
            {
                int AimTime = 0;
                switch (controller.game.State.CurrentState)
                {
                    case GameState.BuildUpgrade:
                        AimTime = 60 + 60;
                        break;
                    case GameState.MatRequest:
                        AimTime = 60 + 60;
                        break;
                    case GameState.ProdRequest:
                        AimTime = 60 + 60;
                        break;
                    case GameState.Production:
                        AimTime = 60 + 60;
                        break;
                }
                StartCoroutine(StartTimer(AimTime));
            }
        }
        int m, s;
        m = Mathf.CeilToInt(CurrentTime) / 60;
        s = Mathf.CeilToInt(CurrentTime) - 60 * m;
        string minutes = (m / 10).ToString() + (m % 10).ToString();
        string secs = (s / 10).ToString() + (s % 10).ToString();
        GetComponent<Text>().text = minutes + ":" + secs;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentTime);
        }
        else
        {
            CurrentTime = (float) stream.ReceiveNext();
        }
    }
}
