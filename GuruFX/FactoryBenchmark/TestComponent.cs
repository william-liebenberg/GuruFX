using GuruFX.Core;
using GuruFX.Core.Entities;

namespace FactoryBenchmark
{
	public class TestComponent : TestComponentBase
	{
		public override string Name => "Test Component";

		public override int Value { get; set; }
		
		public TestComponent(int v)
		{
			Value = v;
		}
	}
}
