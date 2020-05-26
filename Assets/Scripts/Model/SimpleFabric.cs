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

        public int UpgradeTime { get; private set; }

        public SimpleFabric(Director owner) : base(owner, 5)
        {
            UpgradeTime = -1;
        }

        internal SimpleFabric(Director owner, int build_time) : base(owner, build_time)
        {
            UpgradeTime = -1;
        }

        public void StartUpgade()
        {
            UpgradeTime = StartUpgradeTime;
        }

        public void Upgrade()
        {
            Owner.ExchangeFabric(this, new AutoFabric(Owner, 0));
        }

        public override void DecreaseTiming()
        {
            base.DecreaseTiming();
            if (UpgradeTime > 0)
                UpgradeTime--;
            if (UpgradeTime == 0)
            {
                Upgrade();
            }
        }
    }
}
