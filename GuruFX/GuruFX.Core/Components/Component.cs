using System;

namespace GuruFX.Core.Components
{
	public class Component : IComponent
	{
		IEntity mParentEntity;

		public Component()
		{

		}

		public Component(IEntity parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException(nameof(parent), "Parent Entity cannot be invalid!");
			}

			Parent = parent;
		}

		public IEntity Parent
		{
			get
			{
				return mParentEntity;
			}
			private set
			{
				mParentEntity = value;
			}
		}

		public string Name { get; set; }

		public Guid InstanceID { get; set; } = Guid.NewGuid();
		
		/// <summary>
		/// Returns the first sibling component of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <returns>The first sibling component matching the type <typeparamref name="T"/>, otherwise NULL.</returns>
		public IComponent GetComponent<T>() where T : IComponent
		{
			if(Parent == null)
			{
				throw new NullReferenceException("Parent Entity is Invalid");
			}

			return Parent.GetComponent<T>(this);
		}

		/// <summary>
		/// Get all the components of type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">The type of component to search for.</typeparam>
		/// <returns>All the components matching the type <typeparamref name="T"/>, otherwise an empty array.</returns>
		public IComponent[] GetComponents<T>() where T : IComponent
		{
			if (Parent == null)
			{
				throw new NullReferenceException("Parent Entity is Invalid");
			}

			return Parent.GetComponents<T>(this);
		}
	}
}
