using GuruFX.Core.Components;

namespace GuruFX.Core.Tests.Fakes
{
	internal class FakeComponent : Component
	{
		public override string Name { get; set; } = nameof(FakeComponent);
	}
}
