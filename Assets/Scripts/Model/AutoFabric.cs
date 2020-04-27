using Management;
using System.Runtime.CompilerServices;

namespace Management
{
    public class AutoFabric : Fabric
    {
        override public int StartBuildTime { get; } = 7;
        override public int MaxMat { get; } = 2;
        override public int BuildPrice { get; } = 10000;
        override protected int[] _proc_price { get; } = { 0, 2000, 3000 };

        public AutoFabric(Director owner) : base(owner, 7) { }

        internal AutoFabric(Director owner, int build_time) : base(owner, build_time) { }
    }
}
