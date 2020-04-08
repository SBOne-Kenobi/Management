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

        public SimpleFabric() : base(StartBuildTime)
        {
            _upgrade_time = -1;
        }

        internal SimpleFabric(int build_time) : base(build_time)
        {
            _upgrade_time = -1;
        }

        public bool StartUpgade()
        {
            if (_upgrade_time == -1 && _build_time <= 0)
            {
                _upgrade_time = StartUpgradeTime;
                return true;
            }
            return false;
        }

        public AutoFabric Upgrade()
        {
            if (_upgrade_time == 0)
                return new AutoFabric(0);
            else
                return null;
        }

        public override void DecreaseTiming()
        {
            base.DecreaseTiming();
            if (_upgrade_time >= 0)
                _upgrade_time--;
        }
    }
}
