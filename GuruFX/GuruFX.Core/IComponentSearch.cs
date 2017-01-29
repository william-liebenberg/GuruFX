namespace GuruFX.Core
{
	public interface IComponentSearch
	{
		IComponent GetComponent<T>() where T : IComponent;
		IComponent GetComponent<T>(IComponent excludedComponent) where T : IComponent;
		IComponent GetComponent<T>(IComponent[] excludedComponents) where T : IComponent;

		IComponent[] GetComponents<T>() where T : IComponent;
		IComponent[] GetComponents<T>(IComponent excludedComponent) where T : IComponent;
		IComponent[] GetComponents<T>(IComponent[] excludedComponents) where T : IComponent;
	}
}
