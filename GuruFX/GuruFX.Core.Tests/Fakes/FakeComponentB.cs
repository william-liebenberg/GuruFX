using GuruFX.Core.Components;

namespace GuruFX.Core.Tests.Components
{
	internal class FakeComponentB : Component
	{
		public override string Name => "Fake Component B";

		public FakeComponentB()
		{
			
		}

		public FakeComponentB(IEntity parent) : base(parent)
		{

		}
	}
}
