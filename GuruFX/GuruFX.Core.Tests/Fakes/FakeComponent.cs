using GuruFX.Core.Components;

namespace GuruFX.Core.Tests.Components
{
	internal class FakeComponent : Component
	{
		public override string Name => "Fake Component";

		public FakeComponent()
		{
			
		}

		public FakeComponent(IEntity parent) : base(parent)
		{

		}
	}
}
