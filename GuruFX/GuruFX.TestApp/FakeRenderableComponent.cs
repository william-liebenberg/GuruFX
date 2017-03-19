using GuruFX.Core;
using GuruFX.Core.Components;

namespace GuruFX.TestApp
{
	internal class FakeRenderableComponent : Component, IRenderable
	{
		public override string Name { get; set; } = nameof(FakeRenderableComponent);

		public void Render(double elapsedTime, double deltaTime)
		{
		}
	}
}
