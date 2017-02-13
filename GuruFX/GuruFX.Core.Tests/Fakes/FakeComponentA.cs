using GuruFX.Core.Components;

namespace GuruFX.Core.Tests.Components
{
	internal class FakeComponentA : Component
	{
		public override string Name => "Fake Component A";

		public FakeComponentA()
		{

		}

		public FakeComponentA(IEntity parent) : base(parent)
		{

		}
	}
}
