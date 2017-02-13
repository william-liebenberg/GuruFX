namespace GuruFX.Core.Components
{
	/// <summary>
	/// This is just a dummy Transform Component for now.
	/// </summary>
	public class Transform : Component
	{
		public override string Name => "Transform";

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
	}
}
