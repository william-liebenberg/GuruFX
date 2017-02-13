namespace GuruFX.Core
{
	public interface ISystem
	{
		string Name { get; }
		void Init();
		void Destroy();
	}
}
