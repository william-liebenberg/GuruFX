using System;
using System.Collections.Generic;
using System.Diagnostics;
using GuruFX.Core;
using GuruFX.Core.Entities;

namespace FactoryBenchmark
{
	internal class Program
	{
		private static readonly string[] RandomNameValues = { "Bob", "Jane", "Ed", "Rob", "Glen" };
		private static int _x = 0;
		private static string RandomName()
		{
			++_x;
			_x = _x % RandomNameValues.Length;
			return RandomNameValues[_x];
		}

		private const int MaxRandomValues = 1000000;
		private static readonly List<int> RandomValues = new List<int>(MaxRandomValues);
		private static int _r = 0;

		private static int RandomValue()
		{
			++_r;
			_r = _r % RandomValues.Count;
			return RandomValues[_r];
		}

		private static void InitRandomValues()
		{
			Random r = new Random();
			for (int j = 0; j < MaxRandomValues; j++)
			{
				RandomValues.Add(r.Next(MaxRandomValues));
			}
		}

		public interface ITestObject
		{
			string Name { get; set; }
		}

		public class TestObject : ITestObject
		{
			public TestObject()
			{
			}

			public TestObject(string name)
			{
				Name = name;
			}

			public string Name { get; set; } = string.Empty;
		}

		private static void Main(string[] args)
		{
			InitRandomValues();


			/*
				TestObjectFactory Results: 
					Warmup Iterations: 1000
					Iterations: 500000

												 FactoryFunc : 00:00:00.0276911
										FactoryFunc_WithArgs : 00:00:00.0301806
								   CompiledExpression<T>(id) : 00:00:00.0309615
								CompiledExpression(id, type) : 00:00:00.0348999
											  CreateInstance : 00:00:00.0277758
									  CreateInstanceWithArgs : 00:00:00.5938680
				
				EntityFactory Results:
					Warmup Iterations: 1000
					Iterations: 500000

												 FactoryFunc : 00:00:00.6093253
										FactoryFunc_WithArgs : 00:00:00.6281512
								   CompiledExpression<T>(id) : 00:00:00.6336916
								CompiledExpression(id, type) : 00:00:00.6302690
											  CreateInstance : 00:00:00.6294698
									  CreateInstanceWithArgs : 00:00:04.5770215
			
			 
			 Creating Entities is sooo much slower... perhaps the auto property initializers add to this.
			 Perhaps using ConcurrentDictionary instead of just Dictionary is also slowing things down?
			 */

			BenchmarkTestObjectFactory();

			BenchmarkEntityFactory();
		}

		private static void BenchmarkTestObjectFactory()
		{
			Factory<string, ITestObject> testObjectFactory = new Factory<string, ITestObject>();

			// NOTE: Removed the FastActivator method -- lots of work compared to the CompiledExpression methods and it was slower anyways.
			//componentFactory.Register_FastActivator<TestComponent>();

			testObjectFactory.Register("FactoryFunc", () => new TestObject());
			testObjectFactory.Register("FactoryFunc_WithArgs", () => new TestObject(RandomName()));
			testObjectFactory.Register<TestObject>("CompiledExpression<T>(id)");
			testObjectFactory.Register("CompiledExpression(id, type)", typeof(TestObject));
			testObjectFactory.Register("CreateInstance", typeof(TestObject));
			testObjectFactory.Register("CreateInstanceWithArgs", typeof(TestObject), () => new object[] { RandomName() });

			const int warmupIterations = 1000;
			const int iterations = 500000;

			string[] creationMethods =
			{
				// "FastActivator",
				"FactoryFunc",
				"FactoryFunc_WithArgs",
				"CompiledExpression<T>(id)",
				"CompiledExpression(id, type)",
				"CreateInstance",
				"CreateInstanceWithArgs"
			};

			Console.WriteLine($"Warmup Iterations: {warmupIterations}");
			Console.WriteLine($"Iterations: {iterations}");
			Console.WriteLine();

			foreach (string creationMethod in creationMethods)
			{
				BenchmarkTestObjectCreationMethod(warmupIterations, testObjectFactory, creationMethod, iterations);
			}
		}

		private static void BenchmarkTestObjectCreationMethod(int warmupIterations, Factory<string, ITestObject> factory, string creationMethod, int iterations)
		{
			// warmup
			int sum = 0;
			for (int j = 0; j < warmupIterations; j++)
			{
				ITestObject a = factory.Create(creationMethod);
				sum += a.Name.Length;
			}

			// bench it
			sum = 0;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int j = 0; j < iterations; j++)
			{
				ITestObject a = factory.Create(creationMethod);
				sum += a.Name.Length;
			}
			stopwatch.Stop();

			Console.WriteLine($"{creationMethod,40} : {stopwatch.Elapsed}");
		}

		private static void BenchmarkEntityFactory()
		{
			Factory<string, IEntity> entityFactory = new Factory<string, IEntity>();

			// NOTE: Removed the FastActivator method -- lots of work compared to the CompiledExpression methods and it was slower anyways.
			//componentFactory.Register_FastActivator<TestComponent>();

			entityFactory.Register("FactoryFunc", () => new Entity());
			entityFactory.Register("FactoryFunc_WithArgs", () => new Entity(RandomName()));
			entityFactory.Register<Entity>("CompiledExpression<T>(id)");
			entityFactory.Register("CompiledExpression(id, type)", typeof(Entity));
			entityFactory.Register("CreateInstance", typeof(Entity));
			entityFactory.Register("CreateInstanceWithArgs", typeof(Entity), () => new object[] { RandomName() });

			const int warmupIterations = 1000;
			const int iterations = 500000;

			string[] creationMethods =
			{
				// "FastActivator",
				"FactoryFunc",
				"FactoryFunc_WithArgs",
				"CompiledExpression<T>(id)",
				"CompiledExpression(id, type)",
				"CreateInstance",
				"CreateInstanceWithArgs"
			};

			Console.WriteLine($"Warmup Iterations: {warmupIterations}");
			Console.WriteLine($"Iterations: {iterations}");
			Console.WriteLine();

			foreach (string creationMethod in creationMethods)
			{
				BenchmarkEntityCreationMethod(warmupIterations, entityFactory, creationMethod, iterations);
			}
		}



		private static void BenchmarkEntityCreationMethod(int warmupIterations, Factory<string, IEntity> factory, string creationMethod, int iterations)
		{
			// warmup
			int sum = 0;
			for (int j = 0; j < warmupIterations; j++)
			{
				IEntity a = factory.Create(creationMethod);
				sum += a.Name.Length;
			}

			// bench it
			sum = 0;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int j = 0; j < iterations; j++)
			{
				IEntity a = factory.Create(creationMethod);
				sum += a.Name.Length;
			}
			stopwatch.Stop();

			Console.WriteLine($"{creationMethod,40} : {stopwatch.Elapsed}");
		}
	}
}
