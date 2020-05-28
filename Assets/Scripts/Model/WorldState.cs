using System.Collections;
using System.Collections.Generic;
using System;
using Management;
using System.Linq;

namespace Management
{ 

    public interface IWorldObject
    {
        void NextState(WorldState state);
    }

    public enum GameState
    {
        Unknown = -3,
        Start = -1, 
        FixCosts = 0,
        UpdateMarket = 1,
        MatRequest = 2,
        Production = 3,
        ProdRequest = 4,
        BuildUpgrade = 5,
        NumberOfGameStates = 6
    };

    public class WorldState
    {
        public int CurrentMonth { get; private set; }
        public GameState CurrentState { get; private set; }
        public int CurrentMainDirector { get; private set; }
        public List<Fabric> Fabrics => Fabric.Fabrics;
        public List<Director> Directors { get; private set; }
        public Bank Bank { get; private set; }

        public List<Demand> RequestsOfMat { get; private set; }
        public List<Offer> RequestsOfProd { get; private set; }

        public WorldState(List<Director> directors)
        {
            CurrentMonth = 1;
            CurrentState = GameState.Start;
            CurrentMainDirector = 0;
            Directors = directors;
            Bank = new Bank(Directors.Count);
            foreach (Director director in Directors)
                director.Bank = Bank;
            RequestsOfMat = new List<Demand>();
            RequestsOfProd = new List<Offer>();
        }

        public string Description()
        {
            if (CurrentState == GameState.FixCosts)
                return "Getting fix costs";
            else if (CurrentState == GameState.UpdateMarket)
                return "Explore market";
            else if (CurrentState == GameState.MatRequest)
                return "Material requests";
            else if (CurrentState == GameState.Production)
                return "Production";
            else if (CurrentState == GameState.ProdRequest)
                return "Product requests";
            else if (CurrentState == GameState.Start)
                return "Start game";
            else if (CurrentState == GameState.BuildUpgrade)
                return "Build/Upgrade";
            return "Unknown GameState";
        }

        public GameState GetNextState {
            get {
                if (CurrentState == GameState.Start)
                    return GameState.MatRequest;
                int x = (int) CurrentState;
                x = (x + 1) % ((int)GameState.NumberOfGameStates);
                return (GameState)x;
            }
        }

        public int GetPriority(Director director)
        {
            for (int i = 0; i < Directors.Count; i++)
                if (Directors[i] == director)
                    return (i - CurrentMainDirector + Directors.Count) % Directors.Count;
            return Directors.Count;
        }

        public Director GetDirector(int Priority)
        {
            return Directors[(Priority + CurrentMainDirector) % Directors.Count];
        }

        public void AddRequestOfMat(int price, int amount, Director director)
        {
            RequestsOfMat.Add(new Demand(price, amount, GetPriority(director)));
        }

        public void AddRequestOfProd(int price, int amount, Director director)
        {
            RequestsOfProd.Add(new Offer(price, amount, GetPriority(director)));
        }

        public GameState GoNextState()
        {
            //end of state
            if (CurrentState == GameState.FixCosts)
            {
                //nothing
            }
            else if (CurrentState == GameState.UpdateMarket)
            {
                //nothing
            }
            else if (CurrentState == GameState.MatRequest)
            {
                RequestsOfMat = Bank.RequestOfMat(RequestsOfMat);
                foreach (Demand demand in RequestsOfMat)
                    GetDirector(demand.Priority).MakeRequestOfMat(demand.Price, demand.UMat);
                RequestsOfMat.Clear();
            }
            else if (CurrentState == GameState.Production)
            {
                foreach (Director director in Directors)
                    director.NextState(this);
            }
            else if (CurrentState == GameState.ProdRequest)
            {
                RequestsOfProd = Bank.RequestOfProd(RequestsOfProd);
                foreach (Offer offer in RequestsOfProd)
                    GetDirector(offer.Priority).MakeRequestOfProd(offer.Price, offer.UProd);
                RequestsOfProd.Clear();
            } else if (CurrentState == GameState.BuildUpgrade)
            {
                foreach (Director director in Directors)
                    director.UpdateFabCosts();
            }

            CurrentState = GetNextState;

            // start of new state
            if (CurrentState == GameState.FixCosts)
            {
                CurrentMonth += 1;
                Director currentMain = GetDirector(0);
                foreach (Director director in Directors.ToList())
                {
                    director.NextState(this);
                    if (director.IsBankrupt)
                    {
                        if (currentMain != null)
                            CurrentMainDirector--;
                        Directors.Remove(director);
                    }
                    if (currentMain == director)
                        currentMain = null;
                }
                CurrentMainDirector = (CurrentMainDirector + 1) % Directors.Count;
            }
            else if (CurrentState == GameState.UpdateMarket)
            {
                Bank.NextState(this);
            }
            else if (CurrentState == GameState.MatRequest)
            {
                //wait for requests
            }
            else if (CurrentState == GameState.Production)
            {
                //wait for production
            }
            else if (CurrentState == GameState.ProdRequest)
            {
                //wait for requests
            }
            else if (CurrentState == GameState.BuildUpgrade)
            {
                foreach (Fabric fabric in Fabrics.ToList())
                    fabric.NextState(this);
            }

            return CurrentState;
        }
    }
}
