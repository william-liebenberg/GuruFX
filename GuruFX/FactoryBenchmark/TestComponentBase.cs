using GuruFX.Core;
using GuruFX.Core.Components;

namespace FactoryBenchmark
{
	public abstract class TestComponentBase : Component
	{
		protected TestComponentBase(IEntity parent) : base(parent)
		{
		}

		public abstract int Value { get; set; }
	}
}
