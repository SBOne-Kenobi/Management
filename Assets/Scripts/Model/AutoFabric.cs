using Management;
using System.Runtime.CompilerServices;

namespace Management
{
    public class AutoFabric : Fabric
    {
        public new static int StartBuildTime { get; } = 7;
        public new static int MaxMat { get; } = 2;
        public new static int BuildPrice { get; } = 10000;
        protected new static int[] _proc_price { get; } = { 0, 2000, 3000 };

        public AutoFabric() : base(StartBuildTime) { }

        internal AutoFabric(int build_time) : base(build_time) { }
    }
}
