using Management;

namespace Management
{

    public class Director : IWorldObject
    {
        public int Materials { get; set; }

        public int Product { get; set; }

        public int Money { get; set; }

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

        public bool HasFabric(int index)
        {
            return index >= 0 && Fabrics[index] != null;
        }

        public Director()
        {
            //start kit
            Bank = null;
            Materials = 4;
            Product = 2;
            Money = 10000;
            _bankrupt = false;
            Fabrics = new Fabric[6];
            _fab_fix_costs = 0;
            for (int i = 0; i < 2; i++)
                Fabrics[i] = new SimpleFabric(this, 0);
            UpdateFabCosts();
        }

        public int GetFixCostFabric(int index)
        {
            if (HasFabric(index))
                if (Fabrics[index] is SimpleFabric)
                    return 1000;
                else
                    return 1500;
            return 0;
        }

        public void SellFabric(int index)
        {
            if (HasFabric(index))
            {
                Money += Fabrics[index].BuildPrice;
                _fab_fix_costs -= GetFixCostFabric(index);
                Fabrics[index].Remove();
            }
        }

        public void BuyFabric(int index)
        {
            if (!HasFabric(index))
            {
                Fabrics[index] = new SimpleFabric(this);
                if (Money >= Fabrics[index].BuildPrice)
                {
                    Money -= Fabrics[index].BuildPrice;
                    _fab_fix_costs += GetFixCostFabric(index);
                }
                else
                    Fabrics[index].Remove();
            }
        }

        public void RemoveFabric(Fabric fab)
        {
            int index = GetIndex(fab);
            if (HasFabric(index))
                Fabrics[index] = null;
        }

        public void UpgradeFabric(int index)
        {
            if (HasFabric(index))
            {
                if (Fabrics[index] is SimpleFabric)
                {
                    if (Money >= (Fabrics[index] as SimpleFabric).UpgradePrice)
                    {
                        Money -= (Fabrics[index] as SimpleFabric).UpgradePrice;
                        (Fabrics[index] as SimpleFabric).StartUpgade();
                    }
                }
            }
        }

        public void ExchangeFabric(Fabric oldFabric, Fabric newFabric)
        {
            int oldFaricIndex = GetIndex(oldFabric);
            if (HasFabric(oldFaricIndex)) {
                Fabrics[oldFaricIndex] = newFabric;
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
            if (!HasFabric(index))
                return false;
            bool added = false;
            Money += Fabrics[index].ProcPrice;
            if (Fabrics[index].AddMat())
            {
                if (Money - Fabrics[index].ProcPrice >= 0)
                    added = true;
                else
                    Fabrics[index].RemoveMat();
            }
            Money -= Fabrics[index].ProcPrice;
            if (added)
                Materials--;
            return added;
        }

        public bool RemoveMaterial(int index)
        {
            if (!HasFabric(index))
                return false;
            Money += Fabrics[index].ProcPrice;
            bool removed = Fabrics[index].RemoveMat();
            Money -= Fabrics[index].ProcPrice;
            if (removed)
                Materials++;
            return removed;
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
            for (int i = 0; i < Fabrics.Length; i++)
                _fab_fix_costs += GetFixCostFabric(i);
        }
    }
}
