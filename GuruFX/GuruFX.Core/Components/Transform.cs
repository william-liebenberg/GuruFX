namespace GuruFX.Core.Components
{
	/// <summary>
	/// This is just a dummy Transform Component for now.
	/// </summary>
	public class Transform : Component
	{
		public double LastElapsedTime { get; set; }

		public override string Name { get; set; } = nameof(Transform);

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		// public Matrix FinalTransform { get { return new Matrix(Position, Rotation, Scale); } }
	}
}
