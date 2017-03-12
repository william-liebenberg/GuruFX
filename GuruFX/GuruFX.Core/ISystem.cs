namespace GuruFX.Core
{
	public interface ISystem : IBaseObject
	{
		string Name { get; }
		void Init();
		void Destroy();
	}
}
