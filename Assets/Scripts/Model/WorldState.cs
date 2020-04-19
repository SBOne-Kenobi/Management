using System.Collections;
using System.Collections.Generic;
using System;
using Management;

namespace Management
{ 

    public interface IWorldObject
    {
        void NextState(WorldState state);
    }

    public enum GameState
    {
        Start = -1, 
        FixCosts = 0,
        UpdateMarket = 1,
        MatRequest = 2,
        Production = 3,
        ProdRequest = 4,
        NumberOfStates = 5 
    };

    public class WorldState
    {
        public int CurrentMonth { get; private set; }
        public GameState CurrentState { get; private set; }
        public int CurrentMainDirector { get; private set; }
        public List<Fabric> Fabrics => Fabric.Fabrics;
        public List<Director> Directors { get; private set; }
        public Bank Bank { get; private set; }

        public WorldState(List<Director> directors)
        {
            CurrentMonth = 0;
            CurrentState = GameState.Start;
            CurrentMainDirector = 0;
            Directors = directors;
            Bank = new Bank(Directors.Count);
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
            return "Undefined state go method Management::ToString()";
        }

    }
}
