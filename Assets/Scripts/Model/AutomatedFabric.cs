using Management;
using System.Runtime.CompilerServices;

namespace Management
{
    public class AutomatedFabric : Fabric
    {
        override public int StartBuildTime { get; } = 7;
        override public int MaxMat { get; } = 2;
        override public int BuildPrice { get; } = 10000;
        override protected int[] _process_price { get; } = { 0, 2000, 3000 };
        override public int TaxPrice { get; } = 1500;

        public AutomatedFabric(Director owner) : base(owner, 7) { }

        internal AutomatedFabric(Director owner, int build_time) : base(owner, build_time) { }
    }
}
