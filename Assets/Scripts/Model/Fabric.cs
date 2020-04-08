using System;
using System.Collections.Generic;

namespace Management
{
    public class Fabric
    {
        public static int StartBuildTime { get; }

        protected int _build_time;
        public int BuildTime => _build_time;

        static public int MaxMat { get; }

        protected int _current_mat;
        public int CurrentMat => _current_mat;

        static public int BuildPrice { get; }

        static protected int[] _proc_price { get; }
        public int ProcPrice => _proc_price[CurrentMat];
        
        static public List<Fabric> Fabrics = new List<Fabric>();

        public Fabric(int build_time)
        {
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
            if (_current_mat < MaxMat)
            {
                _current_mat++;
                return true;
            }
            return false;
        }

        public bool RemoveMat()
        {
            if (_current_mat > 0)
            {
                _current_mat--;
                return true;
            }
            return false;
        }

        virtual public void DecreaseTiming()
        {
            if (_build_time >= 0)
                _build_time--;
        }

        ~Fabric()
        {
            Fabrics.Remove(this);
        }
    }
}
