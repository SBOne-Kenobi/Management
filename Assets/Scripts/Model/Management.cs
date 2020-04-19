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

        public WorldState State { get; private set; }

        public List<Demand> _requests_of_mat = new List<Demand>();
        public List<Offer> _requests_of_prod = new List<Offer>();

        public Management(List<Director> directors)
        {
            State = new WorldState(directors);
        }

        public void NextState()
        {
            /*
            if (State == -1)
            {
                //start game
                State = 1;
                return;
            }
            State = (State + 1) % NumberOfStates;
            if (State == 0)
            {
                Month++;
                DirectorsOrder = (DirectorsOrder + 1) % _directors.Count;
                foreach (Director d in _directors)
                    if (d.GetFixedCosts())
                        Alive++;
                //dangerous code
                foreach (Fabric f in Fabric.Fabrics.ToList())
                {
                    f.DecreaseTiming();
                    AutoFabric a = null;
                    if (f is SimpleFabric h)
                        a = h.Upgrade(); //this danger (add in list)
                    if (a != null)
                        Fabric.Fabrics.Remove(f); //and this danger (remove form list)
                }
                // sum up results
            } else if (State == 1)
                Bank.SetNewPriceLevel(Alive);
            else if (State == 2)
            {
                // wait requests of materials
                _requests_of_mat = Bank.RequestOfMat(_requests_of_mat);
                // sum up results
                foreach (Demand demand in _requests_of_mat)
                {
                    _directors[(demand.Priority + DirectorsOrder) % _directors.Count].MakeRequestOfMat(demand.Price, demand.UMat);
                }
            } else if (State == 3)
            {
                // wait for ready
                foreach (Fabric f in Fabric.Fabrics)
                    f.DoProcessing();
            } else if (State == 4)
            {
                // wait requests of prod
                _requests_of_prod = Bank.RequestOfProd(_requests_of_prod);
                // sum up results
                foreach (Offer demand in _requests_of_prod)
                {
                    _directors[(demand.Priority + DirectorsOrder) % _directors.Count].MakeRequestOfProd(demand.Price, demand.UProd);
                }
            }
            */
        }
    }
}
