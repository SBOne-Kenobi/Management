using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Management;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Timer : MonoBehaviour, IOnEventCallback
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
            var last_tick = PhotonNetwork.Time;
            while (CurrentTime > 0 && state == controller.game.State.CurrentState)
            {
                yield return null;
                var now = PhotonNetwork.Time;
                CurrentTime -= (float)(now - last_tick);
                last_tick = now;
            }
            CurrentTime = 0f;
            if (state == controller.game.State.CurrentState)
            {
                PhotonNetwork.RaiseEvent(12, null, controller.RaiseEventOptions, controller.SendOptions);
                switcher.GoCommon();
                controller.MinePlayer.GetReady();
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
            PhotonNetwork.RaiseEvent(11, CurrentTime, controller.RaiseEventOptions, controller.SendOptions);
        }
        int m, s;
        m = Mathf.CeilToInt(CurrentTime) / 60;
        s = Mathf.CeilToInt(CurrentTime) - 60 * m;
        string minutes = (m / 10).ToString() + (m % 10).ToString();
        string secs = (s / 10).ToString() + (s % 10).ToString();
        GetComponent<Text>().text = minutes + ":" + secs;
    }

    public void OnEvent(EventData photonEvent)
    {
        switch(photonEvent.Code)
        {
            case 11:
                CurrentTime = (float)photonEvent.CustomData;
                break;
            case 12:
                switcher.GoCommon();
                controller.MinePlayer.GetReady();
                break;
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
