using System;
using System.Collections.Generic;

namespace GuruFX.Core
{
	public class Entity
	{
		Dictionary<string, Component> m_components = new Dictionary<string, Component>();
		Dictionary<string, Entity> m_entities = new Dictionary<string, Entity>();

		public Dictionary<string, Component> Components
		{
			get { return m_components; }
			set { m_components = value; }
		}

		public Dictionary<string, Entity> Entities
		{
			get { return m_entities; }
			set { m_entities = value; }
		}

		public Guid InstanceID { get; set; } = Guid.NewGuid();
	}
}
