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
            if (State == 4)
                return "Getting fix costs";
            else if (State == 0)
                return "Explore market";
            else if (State == 1)
                return "Material requests";
            else if (State == 2)
                return "Production";
            else if (State == 3)
                return "Product requests";
            else if (State == -1)
                return "Start game";
            return "Undefined state go method Management::ToString()";
        }
        public Bank Bank { get; }
        public int DirectorsOrder { get; private set; }

        public List<Director> _directors { get; }

        public int Alive { get; private set; }

        public int State { get; private set; }
        public int NumberOfStates { get; }

        public List<Demand> _requests_of_mat = new List<Demand>();
        public List<Offer> _requests_of_prod = new List<Offer>();
        public int Month { get; private set; }

        public Management(List<Director> directors)
        {
            _directors = directors;
            Month = 1;
            State = -1;
            DirectorsOrder = 0;
            Alive = directors.Count;
            Bank = new Bank(Alive);
            NumberOfStates = 5;
        }

        public void NextState()
        {
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
        }
    }
}
