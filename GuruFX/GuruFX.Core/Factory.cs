using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GuruFX.Core
{
	public class Factory<TKey, TBaseObj>
	{
		private readonly Dictionary<TKey, Func<TBaseObj>> m_creationFuncs;

		public Factory()
		{
			m_creationFuncs = new Dictionary<TKey, Func<TBaseObj>>();
		}

		public TBaseObj Create(TKey id)
		{
			Func<TBaseObj> constructor;
			if (m_creationFuncs.TryGetValue(id, out constructor))
			{
				return constructor();
			}

			throw new ArgumentException("No type registered for this ID");
		}

		public void Register(TKey id, Func<TBaseObj> ctor)
		{
			Func<TBaseObj> f;
			if (m_creationFuncs.TryGetValue(id, out f))
			{
				throw new Exception("Factory Func already exists for id: " + id);
			}

			m_creationFuncs.Add(id, ctor);
		}

		public void Register(TKey id, Type itemType)
		{
			if (itemType == null)
			{
				throw new ArgumentNullException(nameof(itemType), "ItemType cannot be invalid!");
			}

			if (itemType.IsInterface || itemType.IsAbstract || itemType.IsValueType || !typeof(TBaseObj).IsAssignableFrom(itemType))
			{
				throw new Exception("Cannot register this Type: " + itemType.FullName);
			}

			Func<TBaseObj> f;
			if (m_creationFuncs.TryGetValue(id, out f))
			{
				throw new Exception("Compiled Expression Func already exists for id: " + id);
			}

			ConstructorInfo ctor = itemType.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				throw new Exception("Could not get Parameterless Constructor for type: " + itemType.FullName);
			}

			Func<TBaseObj> baseObjCreatorFunc = Expression.Lambda<Func<TBaseObj>>(Expression.New(ctor)).Compile();

			m_creationFuncs.Add(id, baseObjCreatorFunc);
		}
	}
}
