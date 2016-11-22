namespace GuruFX.Core
{
	public class Component
	{
		public string Name { get; set; }
		public Entity Entity { get; private set; }

		public Component(Entity entity)
		{
			this.Entity = entity;
		}
	}
}
