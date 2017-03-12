using System;

namespace GuruFX.Core.Components
{
	/// <summary>
	/// This is just a dummy Transform Component for now.
	/// </summary>
	public class Transform : Component, IUpdateable
	{
		public double LastElapsedTime { get; set; }

		public override string Name => "Transform";

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public void Update(double elapsedTime, double deltaTime)
		{
			LastElapsedTime = elapsedTime;

			// calculate the final transform matrix for this Transform??
		}
	}
}
