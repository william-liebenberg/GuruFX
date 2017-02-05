namespace GuruFX.Core
{
	public interface ISystem : IUpdateable
	{
		string Name { get; set; }
		void Init();
		void Destroy();
	}
}
