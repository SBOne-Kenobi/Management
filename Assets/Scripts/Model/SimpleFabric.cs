using Management;

namespace Management
{
    public class SimpleFabric : Fabric
    {
        override public int StartBuildTime { get; } = 5;
        public int UpgradePrice { get; } = 7000;
        public int StartUpgradeTime { get; } = 9;
        override public int MaxMat { get; } = 1;
        override public int BuildPrice { get; } = 5000;
        override protected int[] _proc_price { get; } = { 0, 2000 };

        private int _upgrade_time;

        public SimpleFabric(Director owner) : base(owner, 5)
        {
            _upgrade_time = -1;
        }

        internal SimpleFabric(Director owner, int build_time) : base(owner, build_time)
        {
            _upgrade_time = -1;
        }

        public void StartUpgade()
        {
            _upgrade_time = StartUpgradeTime;
        }

        public void Upgrade()
        {
            Owner.ExchangeFabric(this, new AutoFabric(Owner, 0));
        }

        public override void DecreaseTiming()
        {
            base.DecreaseTiming();
            if (_upgrade_time > 0)
                _upgrade_time--;
            if (_upgrade_time == 0)
                Upgrade();
        }
    }
}
