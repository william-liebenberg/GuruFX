using System;

namespace GuruFX.Core
{
	public class Singleton<T> where T : class, new()
	{
		static readonly Lazy<T> m_singletonInstance = new Lazy<T>(() => new T());

		public static T Instance => m_singletonInstance.Value;
	}
}
