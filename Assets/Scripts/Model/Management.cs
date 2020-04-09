using Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Management
{
    public class Management
    {
        private Bank _bank;
        public Bank Bank => _bank;

        private int _order;
        public int DirectorsOrder => _order;

        public List<Director> _directors { get; }

        private int _alive;

        private int _state;
        public int State => _state;

        private int _num_states;
        public int NumberOfStates => _num_states;

        public List<Demand> _requests_of_mat = new List<Demand>();
        public List<Offer> _requests_of_prod = new List<Offer>();

        private int _cur_month;
        public int Month => _cur_month;

        public Management(List<Director> directors)
        {
            _directors = directors;
            _cur_month = 1;
            _state = 1;
            _order = 0;
            _alive = directors.Count;
            _bank = new Bank(_alive);
            _num_states = 5;
        }

        public void NextState()
        {
            _state = (_state + 1) % _num_states;
            if (_state == 0)
            { 
                _order = (_order + 1) % _directors.Count;
                foreach (Director d in _directors)
                    if (d.GetFixedCosts())
                        _alive++;
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
            } else if (_state == 1)
                _bank.SetNewPriceLevel(_alive);
            else if (_state == 2)
            {
                // wait requests of materials
                _requests_of_mat = _bank.RequestOfMat(_requests_of_mat);
                // sum up results
            } else if (_state == 3)
            {
                // wait for ready
                foreach (Fabric f in Fabric.Fabrics)
                    f.DoProcessing();
            } else if (_state == 4)
            {
                // wait requests of prod
                _requests_of_prod = _bank.RequestOfProd(_requests_of_prod);
                // sum upp results
            }
        }
    }
}
