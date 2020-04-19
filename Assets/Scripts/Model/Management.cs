using Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Management
{
    public class Management
    {

        public override string ToString()
        {
            return State.Description();
        }

        public WorldState State { get; }

        public List<Demand> _requests_of_mat = new List<Demand>();
        public List<Offer> _requests_of_prod = new List<Offer>();

        public Management(List<Director> directors)
        {
            State = new WorldState(directors);
        }

        public GameState NextState()
        {
            return State.GoNextState();
        }
    }
}
