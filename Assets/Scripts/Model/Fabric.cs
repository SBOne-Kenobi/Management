using System;
using System.Collections.Generic;
using Management;

namespace Management
{
    public class Fabric : IWorldObject
    {
        public virtual int StartBuildTime { get; }
        public Director Owner { get; private set; }

        protected int _build_time;
        public int BuildTime => _build_time;

        virtual public int MaxMat { get; }

        protected int _current_mat;
        public int CurrentMat => _current_mat;

        virtual public int BuildPrice { get; }

        virtual protected int[] _process_price { get; }
        public int ProcPrice => _process_price[CurrentMat];

        static public List<Fabric> Fabrics = new List<Fabric>();

        virtual public int TaxPrice { get; }

        public Fabric(Director owner, int build_time)
        {
            Owner = owner;
            _current_mat = 0;
            _build_time = build_time;
            Fabrics.Add(this);
        }

        public int DoProcessing()
        {
            int res = _current_mat;
            _current_mat = 0;
            return res;
        }

        public bool AddMat()
        {
            if (_current_mat < MaxMat && _build_time == 0)
            {
                _current_mat++;
                return true;
            }
            return false;
        }

        public bool RemoveMat()
        {
            if (_current_mat > 0 && _build_time == 0)
            {
                _current_mat--;
                return true;
            }
            return false;
        }

        virtual public void DecreaseTiming()
        {
            if (_build_time > 0)
                _build_time--;
        }

        public void NextState(WorldState state)
        {
            DecreaseTiming();
        }

        public void Remove()
        {
            Owner.RemoveFabric(this);
            Fabrics.Remove(this);
        }

        ~Fabric()
        {
            Remove();
        }
    }
}
