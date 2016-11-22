using System;

namespace GuruFX.Core
{
	public class Singleton<T> where T : class, new()
	{
		static readonly Lazy<T> mSingleton = new Lazy<T>(() => new T());

		public static T Instance => mSingleton.Value;
	}
}
