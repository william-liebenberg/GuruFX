using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GuruFX.Core
{
	public class Factory<TKey, TBaseObj>
	{
		private readonly Dictionary<TKey, Func<TBaseObj>> m_creationFuncs = new Dictionary<TKey, Func<TBaseObj>>();

		private void AddCreator(TKey id, Func<TBaseObj> ctor)
		{
			m_creationFuncs.Add(id, ctor);
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

			AddCreator(id, ctor);
		}

		public void Register(TKey id, Type itemType, Func<object[]> argsFunc)
		{
			Func<TBaseObj> f;
			if (m_creationFuncs.TryGetValue(id, out f))
			{
				throw new Exception("Factory Func already exists for id: " + id);
			}

			AddCreator(id, () => (TBaseObj)Activator.CreateInstance(itemType, argsFunc()));
		}

		public void Register<TObject>(TKey id) where TObject : class, TBaseObj, new()
		{
			Func<TBaseObj> f;
			if (m_creationFuncs.TryGetValue(id, out f))
			{
				throw new Exception("Compiled Expression Func already exists for id: " + id);
			}

			ConstructorInfo ctor = typeof(TObject).GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				throw new Exception("Compiled Expression Func already exists for id: " + id);
			}

			Func<TBaseObj> baseObjCreatorFunc = Expression.Lambda<Func<TBaseObj>>(Expression.New(ctor)).Compile();

			AddCreator(id, baseObjCreatorFunc);
		}

		public void Register(TKey id, Type itemType)
		{
			if (itemType == null)
			{
				throw new ArgumentNullException(nameof(itemType), "ItemType cannot be invalid!");
			}

			Func<TBaseObj> f;
			if (m_creationFuncs.TryGetValue(id, out f))
			{
				throw new Exception("Compiled Expression Func already exists for id: " + id);
			}

			ConstructorInfo ctor = itemType.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
			{
				throw new Exception("Compiled Expression Func already exists for id: " + id);
			}

			Func<TBaseObj> baseObjCreatorFunc = Expression.Lambda<Func<TBaseObj>>(Expression.New(ctor)).Compile();

			AddCreator(id, baseObjCreatorFunc);
		}
	}
}
