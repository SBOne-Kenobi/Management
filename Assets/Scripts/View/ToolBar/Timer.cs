using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Management;

public class Timer : MonoBehaviour
{
    float CurrentTime = 0;
    Controller controller;

    private void Awake()
    {
        controller = FindObjectOfType<Controller>();
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
                foreach (Player player in controller.Players)
                    player.GetReady();
            }
        }
    }

    private void Update()
    {
        if (!controller.IsReadyToGoNext && !controller.AllPlayersReady && CurrentTime == 0)
        {
            int AimTime = 0;
            switch (controller.game.State.CurrentState)
            {
                case GameState.FixCosts:
                    AimTime = 30;
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
        int m, s;
        m = Mathf.CeilToInt(CurrentTime) / 60;
        s = Mathf.CeilToInt(CurrentTime) - 60 * m;
        string minutes = (m / 10).ToString() + (m % 10).ToString();
        string secs = (s / 10).ToString() + (s % 10).ToString();
        GetComponent<Text>().text = minutes + ":" + secs;
    }
}
