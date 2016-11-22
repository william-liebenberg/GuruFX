using System.Collections.Generic;

namespace GuruFX.Core
{
	public class Scene
	{
		List<Entity> m_entities = new List<Entity>();

		public Entity CreateEntity()
		{
			Entity entity = new Entity();
			m_entities.Add(entity);
			return entity;
		}
	}
}
