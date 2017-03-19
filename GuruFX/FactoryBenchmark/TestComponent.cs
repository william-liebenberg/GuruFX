using GuruFX.Core;
using GuruFX.Core.Entities;

namespace FactoryBenchmark
{
	public class TestComponent : TestComponentBase
	{
		public override string Name { get; set; } = nameof(TestComponent);

		public override int Value { get; set; }
		
		public TestComponent(int v)
		{
			Value = v;
		}
	}
}
