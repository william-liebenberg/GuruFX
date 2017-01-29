using System;

namespace GuruFX.Core
{
#pragma warning disable RECS0014 // If all fields, properties and methods members are static, the class can be made static.
	public class Singleton<T>
#pragma warning restore RECS0014 // If all fields, properties and methods members are static, the class can be made static.
		where T : class, new()
	{
		static readonly Lazy<T> mSingleton = new Lazy<T>(() => new T());

		public static T Instance => mSingleton.Value;
	}
}
