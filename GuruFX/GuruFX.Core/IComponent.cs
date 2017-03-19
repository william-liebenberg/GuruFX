namespace GuruFX.Core
{
	public interface IComponent : IBaseObject
	{
		IEntity Parent { get; set; }
		
		// TODO: Write tests for "Inactive" Components
		bool IsActive { get; set; }
	}
}
