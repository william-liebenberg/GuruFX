namespace GuruFX.Core
{
	public interface IComponent : IBaseObject
	{
		string Name { get; }

		IEntity Parent { get; set; }
		
		// TODO: Write tests for "Inactive" Components
		bool IsActive { get; set; }
	}
}
