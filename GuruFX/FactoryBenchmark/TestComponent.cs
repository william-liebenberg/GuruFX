using GuruFX.Core;
using GuruFX.Core.Entities;

namespace FactoryBenchmark
{
	public class TestComponent : TestComponentBase
	{
		public TestComponent() : base(new Entity("Dummy"))
		{

		}

		public TestComponent(int v) : this()
		{
			Value = v;
		}

		protected TestComponent(IEntity parent) : base(parent)
		{

		}

		public sealed override int Value { get; set; }
	}
}
