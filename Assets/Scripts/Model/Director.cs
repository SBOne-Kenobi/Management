using Management;

namespace Management
{

    public class Director : IWorldObject
    {
        private int _mat;
        public int Materials => _mat;

        private int _prod;
        public int Product => _prod;

        private int _money;
        public int Money => _money;

        private Fabric[] _fabrics;
        public Fabric[] Fabrics => _fabrics;

        private bool _bankrupt;
        public bool IsBankrupt => _bankrupt;

        private int _sum_price;
        public int SummaryPrice => _sum_price;

        public Director()
        {
            //start kit
            _mat = 4;
            _prod = 2;
            _money = 10000;
            _bankrupt = false;
            _fabrics = new Fabric[8];
            _sum_price = _money;
            for (int i = 0; i < 2; i++)
                _fabrics[i] = new SimpleFabric();
            for (int i = 2; i < 8; i++)
                _fabrics[i] = null;
        }

        public void MakeRequestOfMat(int price, int get)
        {
            _money -= price * get;
            _mat += get;
        }

        public void MakeRequestOfProd(int price, int sold)
        {
            _money += price * sold;
            _prod -= sold;
        }

        public bool GetFixedCosts()
        {
            //magic constates (:
            if (_bankrupt)
                return false;
            _money -= (_mat * 300);
            _money -= (_prod * 500);
            foreach (Fabric f in _fabrics)
            {
                if (f == null)
                    continue;
                if (f is SimpleFabric)
                    _money -= 1000;
                else if (f is AutoFabric)
                    _money -= 1500;
            }
            if (_money < 0)
                _bankrupt = true;
            return !_bankrupt;
        }

        public void NextState(WorldState state)
        {
            throw new System.NotImplementedException();
        }
    }
}