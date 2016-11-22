namespace GuruFX.Core
{
	public interface ISystem : IUpdateable
	{
		void Init();
		void Destroy();
	}
}
