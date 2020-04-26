using Management;

namespace Management
{

    public class Director : IWorldObject
    {
        public int Materials { get; private set; }

        public int Product { get; private set; }

        public int Money { get; private set; }

        public Fabric[] Fabrics { get; set; }

        private bool _bankrupt;
        public bool IsBankrupt => _bankrupt;

        public Bank Bank { get; set; }

        public int SummaryPrice {
            get
            {
                return Money + GetPriceOfMat() + GetPriceOfProd();
            }
        }

        private int _fab_fix_costs;
        public int FixCosts
        {
            get
            {
                return (Materials * 300) + (Product * 500) + _fab_fix_costs;
            }
        }

        public Director()
        {
            //start kit
            Bank = null;
            Materials = 4;
            Product = 2;
            Money = 10000;
            _bankrupt = false;
            Fabrics = new Fabric[8];
            _fab_fix_costs = 0;
            for (int i = 0; i < 2; i++)
                Fabrics[i] = new SimpleFabric(this, 0);
            for (int i = 2; i < Fabrics.Length; i++)
                Fabrics[i] = null;
        }

        public void RemoveFabric(Fabric fab)
        {
            for (int i = 0; i < Fabrics.Length; i++)
                if (Fabrics[i] == fab)
                {
                    Fabrics[i] = null;
                    break;
                }
        }

        public void ExchangeFabric(Fabric fab, Fabric new_fab)
        {
            for (int i = 0; i < Fabrics.Length; i++)
                if (Fabrics[i] == fab)
                {
                    Fabrics[i] = new_fab;
                    break;
                }
        }

        public void MakeRequestOfMat(int price, int get)
        {
            Money -= price * get;
            Materials += get;
        }

        public void MakeRequestOfProd(int price, int sold)
        {
            Money += price * sold;
            Product -= sold;
        }

        public bool GetFixedCosts()
        {
            //magic constates (:
            if (_bankrupt)
                return false;
            Money -= FixCosts;
            if (Money < 0)
            {
                _bankrupt = true;
                Bank = null;
                Money = 0;
            }
            return !_bankrupt;
        }

        private int GetPriceOfMat()
        {
            if (Bank == null)
                return 0;
            return Materials * Bank.GetInfo.MinPrice;
        }

        private int GetPriceOfProd()
        {
            if (Bank == null)
                return 0;
            return Product * Bank.GetInfo.MaxPrice;
        }

        public int GetIndex(Fabric fabric)
        {
            for (int i = 0; i < Fabrics.Length; i++)
                if (Fabrics[i] == fabric)
                    return i;
            return -1;
        }

        public bool AddMaterial(int index)
        {
            if (Fabrics[index] == null)
                return false;
            bool ret = false;
            Money += Fabrics[index].ProcPrice;
            if (Fabrics[index].AddMat())
            {
                if (Money - Fabrics[index].ProcPrice >= 0)
                    ret = true;
                else
                    Fabrics[index].RemoveMat();
            }
            Money -= Fabrics[index].ProcPrice;
            if (ret)
                Materials--;
            return ret;
        }

        public bool RemoveMaterial(int index)
        {
            if (Fabrics[index] == null)
                return false;
            Money += Fabrics[index].ProcPrice;
            bool ret = Fabrics[index].RemoveMat();
            Money -= Fabrics[index].ProcPrice;
            if (ret)
                Materials++;
            return ret;
        }

        public void NextState(WorldState state)
        {
            if (state.CurrentState == GameState.FixCosts)
            {
                if (!GetFixedCosts())
                {
                    foreach (Fabric fabric in Fabrics)
                    {
                        if (fabric == null)
                            continue;
                        fabric.Remove();
                    }
                }
            }
            else
            {
                foreach (Fabric fabric in Fabrics)
                {
                    if (fabric == null)
                        continue;
                    Product += fabric.DoProcessing();
                }
            }
        }

        public void UpdateFabCosts()
        {
            _fab_fix_costs = 0;
            foreach (Fabric fabric in Fabrics)
            {
                if (fabric == null)
                    continue;
                if (fabric is SimpleFabric)
                    _fab_fix_costs += 1000;
                if (fabric is AutoFabric)
                    _fab_fix_costs += 1500;
            }
        }
    }
}