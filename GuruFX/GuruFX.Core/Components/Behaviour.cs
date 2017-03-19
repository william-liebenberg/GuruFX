namespace GuruFX.Core.Components
{
	/// <summary>
	/// Very basic behaviour for now. This class is what we will extend to add Scriptable behaviour (via C# scripts).
	/// </summary>
	public class Behaviour : Component, IUpdateable
	{
		public override string Name { get; set; } = nameof(Behaviour);

		public double LastElapsedTime { get; set; }
		
		public void Update(double elapsedTime, double deltaTime)
		{
			this.LastElapsedTime = elapsedTime;
		}
	}
}
