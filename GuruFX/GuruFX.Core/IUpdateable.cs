namespace GuruFX.Core
{
	public interface IUpdateable
	{
		double LastElapsedTime { get; set; }
		void Update(double elapsedTime, double deltaTime);
	}
}
