using Management;

namespace Management
{
    public class SimpleFabric : Fabric
    {
        public new static int StartBuildTime { get; } = 5;
        public static int UpgradePrice { get; } = 7000;
        public static int StartUpgradeTime { get; } = 9;
        public new static int MaxMat { get; } = 1;
        public new static int BuildPrice { get; } = 5000;
        protected new static int[] _proc_price { get; } = { 0, 2000 };

        private int _upgrade_time;

        public SimpleFabric(Director owner, int pos) : base(owner, pos, StartBuildTime)
        {
            _upgrade_time = -1;
        }

        internal SimpleFabric(Director owner, int pos, int build_time) : base(owner, pos, build_time)
        {
            _upgrade_time = -1;
        }

        public void StartUpgade()
        {
            _upgrade_time = StartUpgradeTime;
        }

        public AutoFabric Upgrade()
        {
            return new AutoFabric(Owner, Pos, 0);
        }

        public override void DecreaseTiming()
        {
            base.DecreaseTiming();
            if (_upgrade_time > 0)
                _upgrade_time--;
            if (_upgrade_time == 0)
            {
                Remove();
                Upgrade();
            }
        }
    }
}
