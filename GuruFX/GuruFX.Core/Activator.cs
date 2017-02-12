using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GuruFX.Core
{
	public class Activator<TKey, TBaseObj>
	{
		private delegate T ObjectActivator<out T>(params object[] args);

		// full typename, <constructor signature, activator>
		private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ObjectActivator<TBaseObj>>> m_activatorCache;

		public Activator()
		{
			m_activatorCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, ObjectActivator<TBaseObj>>>();
		}

		public TBaseObj ActivateInstance(Type itemType) => (TBaseObj)Activator.CreateInstance(itemType);

		//public TBaseObj Activate(Type itemType)
		//{
		//	string sig = GenerateSignature(null);

		//	ObjectActivator<TBaseObj> activator = GetOrAddActivator(itemType, sig);

		//	if (activator == null)
		//	{
		//		throw new Exception("Cannot Activate item type: " + itemType.FullName);
		//	}

		//	return activator();
		//}

		public TBaseObj ActivateInstance(Type itemType, params object[] args)
		{
			if(args == null || args.Length < 1)
			{
				return this.ActivateInstance(itemType);
			}

			string sig = GenerateSignature(args);

			ObjectActivator<TBaseObj> activator = GetOrAddActivator(itemType, sig);

			if (activator == null)
			{
				throw new Exception("Cannot Activate item type: " + itemType.FullName);
			}

			return activator(args);
		}

		private ObjectActivator<TBaseObj> GetOrAddActivator(Type itemType, string sig)
		{
			ObjectActivator<TBaseObj> activator = GetObjectActivator(itemType, sig);

			if (activator == null)
			{
				// try registering the object activator
				if (Register(itemType))
				{
					activator = GetObjectActivator(itemType, sig);
				}
			}

			return activator;
		}

		private bool Register(Type itemType)
		{
			if (itemType.IsInterface || itemType.IsAbstract || itemType.IsValueType || !typeof(TBaseObj).IsAssignableFrom(itemType))
			{
				//throw new Exception("Cannot register this Type!");
				return false;
			}

			ConcurrentDictionary<string, ObjectActivator<TBaseObj>> creatorSigs = new ConcurrentDictionary<string, ObjectActivator<TBaseObj>>();

			foreach (ConstructorInfo ctorInfo in itemType.GetConstructors())
			{
				string sig = GenerateSignature(ctorInfo.GetParameters().Select(p => p.ParameterType).ToArray());
				ObjectActivator<TBaseObj> createdActivator = GenerateObjectActivator<TBaseObj>(ctorInfo);
				creatorSigs[sig] = createdActivator;
			}

			if (!m_activatorCache.TryAdd(itemType.FullName, creatorSigs))
			{
				//throw new Exception("Could not add Activator to ActivatorCache!");
				return false;
			}

			return true;
		}

		private ObjectActivator<TBaseObj> GetObjectActivator(Type itemType, string sig)
		{
			ConcurrentDictionary<string, ObjectActivator<TBaseObj>> createorSigs;

			if (!m_activatorCache.TryGetValue(itemType.FullName, out createorSigs))
			{
				//throw new ArgumentException("No Activator Signatures found for ID: " + id);
				return null;
			}

			if (createorSigs == null)
			{
				//throw new NullReferenceException("Signature Store is invalid for ID: " + id);
				return null;
			}

			ObjectActivator<TBaseObj> activator;

			if (!createorSigs.TryGetValue(sig, out activator))
			{
				//throw new ArgumentException("No Activator registered for this Signature: " + sig);
				return null;
			}

			if (activator == null)
			{
				//throw new ArgumentException("No Activator registered for this Signature: " + sig);
				return null;
			}

			return activator;
		}

		private static string GenerateSignature(IEnumerable<object> args) => GenerateSignature(args.Select(a => a.GetType()).ToArray());

		private static string GenerateSignature(Type[] args) => args == null ? string.Empty : string.Join(",", args.Select(a => a.FullName).ToArray());
		
		private static ObjectActivator<T> GenerateObjectActivator<T>(ConstructorInfo ctor)
		{
			Type type = ctor.DeclaringType;
			ParameterInfo[] paramsInfo = ctor.GetParameters();

			// create a single param of type object[]
			ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

			Expression[] argsExp = new Expression[paramsInfo.Length];

			// pick each arg from the params array 
			// and create a typed expression of them
			for (int i = 0; i < paramsInfo.Length; i++)
			{
				Expression index = Expression.Constant(i);
				Type paramType = paramsInfo[i].ParameterType;
				Expression paramAccessorExp = Expression.ArrayIndex(param, index);
				Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
				argsExp[i] = paramCastExp;
			}

			// make a NewExpression that calls the
			// ctor with the args we just created
			NewExpression newExp = Expression.New(ctor, argsExp);

			// create a lambda with the New
			// Expression as body and our param object[] as arg
			LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

			// compile it
			ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
			return compiled;
		}
	}
}
