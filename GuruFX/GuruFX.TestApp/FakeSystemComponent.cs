using GuruFX.Core.SystemComponents;

namespace GuruFX.TestApp
{
	internal class FakeSystemComponent : SystemComponent
	{
		public override string Name { get; set; } = nameof(FakeSystemComponent);

		public override void Destroy()
		{
		}

		public override void Init()
		{
		}
	}
}