using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuruFX.Core;
using GuruFX.Core.Components;

namespace GuruFX.TestApp
{
	internal class FakeRenderable : Component, IRenderable
	{
		public double LastRenderTime { get; set; }

		public override string Name => "Fake Renderable";

		public void Render(double elapsedTime, double deltaTime)
		{
			this.LastRenderTime = elapsedTime;
		}
	}
}
