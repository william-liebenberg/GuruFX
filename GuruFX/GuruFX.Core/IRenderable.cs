namespace GuruFX.Core
{
	public interface IRenderable : IBaseObject
	{
		void Render(double elapsedTime, double deltaTime);
	}
}
