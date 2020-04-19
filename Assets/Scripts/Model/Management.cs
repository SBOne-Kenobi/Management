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

        public int Alive => State.Directors.Count;

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
