using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GuruFX.Core.Entities;

namespace GuruFX.Core.Scenes
{
	public class Scene : Entity, IScene, IUpdateable, ISystem
	{
		public double LastElapsedTime { get; set; }

		public override string Name { get; set; } = "Scene";

		public ConcurrentDictionary<Guid, ISystem> Systems { get; set; } = new ConcurrentDictionary<Guid, ISystem>();
		public ConcurrentDictionary<Guid, IUpdateable> Updateables { get; set; } = new ConcurrentDictionary<Guid, IUpdateable>();
		
		public void Init()
		{
			foreach(KeyValuePair<Guid, ISystem> system in this.Systems)
			{
				system.Value?.Init();
			}
		}

		public void Destroy()
		{
			foreach(KeyValuePair<Guid, ISystem> system in this.Systems)
			{
				system.Value?.Destroy();
			}
		}

		/// <summary>
		/// Update all the System Components
		/// </summary>
		/// <param name="elapsedTime">Total Elapsed Time since the first update</param>
		/// <param name="deltaTime">Time passed since last Update</param>
		public void Update(double elapsedTime, double deltaTime)
		{
			this.LastElapsedTime = elapsedTime;

			// ok.. so each scene has to go through its "System Components" and Update them
			// the Scene itself does not update/process the entities that it owns.
			// It is up to this Scene's "System Components" to deal with all the entities and their components

			foreach(KeyValuePair<Guid, IUpdateable> updateable in this.Updateables)
			{
				updateable.Value?.Update(elapsedTime, deltaTime);
			}
		}
	}
}
