using GuruFX.Core;
using GuruFX.Core.Entities;

namespace FactoryBenchmark
{
	public class TestComponent : TestComponentBase
	{
		public override string Name => "Test Component";

		public override int Value { get; set; }

		public TestComponent() : base(new GameObject("Dummy"))
		{

		}

		public TestComponent(int v) : this()
		{
			Value = v;
		}

		protected TestComponent(IEntity parent) : base(parent)
		{

		}
	}
}
