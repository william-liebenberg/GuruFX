using System;
using System.Diagnostics;
using GuruFX.Core;
using GuruFX.Core.Components;
using GuruFX.Core.Entities;
using GuruFX.Core.Scenes;
using GuruFX.Core.SystemComponents;

namespace GuruFX.TestApp
{
	internal static class Program
	{
		private static void Main()
		{
			SceneUpdater sceneUpdater = new SceneUpdater();
			SceneRenderer sceneRenderer = new SceneRenderer();

			Scene s = new Scene()
			{
				Name = "I ARE SCENE!"
			};

			Console.WriteLine("Scene Name: " + s.Name);
			Console.WriteLine("Root Name: " + s.Root.Name);

			s.AddComponent(sceneUpdater);
			s.AddComponent(sceneRenderer);
			// s.AddComponent(new Behaviour()); --> Yay it works! as in... can only add SystemComponents to the root Scene node.

			IEntity e = new GameObject();
			s.Root.AddEntity(e);
			e.AddComponent(new Behaviour()); 
			e.AddComponent(new FakeRenderableComponent());
			e.AddComponent(new FakeSystemComponent()); // hmm should this be allowed????...
			
			// simple game loop
			double elapsedTime = 0;
			double lastElapsedTime = 0;

			Stopwatch w = new Stopwatch();
			w.Start();

			const double fpsInterval = 1.0;
			double fpsElapsed = 0;
			double fps = 0;
			double minFps = double.MaxValue;
			double maxFps = double.MinValue;

			double displayMinFps = 0;
			double displayMaxFps = 0;
			double displayAvgFps = 0;

			double fpsSum = 0;
			const double avgFpsInterval = 10;
			double avgFpsElapsed = 0;
			int numFrames = 0;

			while(true)
			{
				// frame start
				int line = 3;
				sceneUpdater.EntitiesUpdated = 0;
				sceneRenderer.EntitiesRendered = 0;
				
				double deltaTime = elapsedTime - lastElapsedTime;
				fpsElapsed += deltaTime;
				avgFpsElapsed += deltaTime;

				if(fpsElapsed >= fpsInterval)
				{
					fps = numFrames / fpsElapsed;
					fpsElapsed = 0;
					numFrames = 0;

					fpsSum += fps;
					if(fps < minFps)
						minFps = fps;
					if(fps > maxFps)
						maxFps = fps;

					displayMinFps = minFps;
					displayMaxFps = maxFps;
				}
				
				if(avgFpsElapsed >= avgFpsInterval)
				{
					displayAvgFps = fpsSum / avgFpsElapsed;
					avgFpsElapsed = 0;
					fpsSum = 0;
				}

				// frame update
				s.Update(elapsedTime, deltaTime);

				// frame debug
				Print(0, line++, $"Elapsed Time: {elapsedTime}");
				Print(0, line++, $"Delta Time: {deltaTime}");
				Print(0, line++, $"FPS (current/min/max/avg): {fps}/{displayMinFps}/{displayMaxFps}/{displayAvgFps}");
				Print(0, line++, $"# Scene.System Components: {s.Systems.Count}");
				Print(0, line++, $"# Scene.Update Components: {s.Updateables.Count}");
				Print(0, line++, $"# Components Updated: {sceneUpdater.EntitiesUpdated}");
				Print(0, line, $"# Components Rendered: {sceneRenderer.EntitiesRendered}");
				
				// frame end
				lastElapsedTime = elapsedTime;
				elapsedTime = (w.ElapsedMilliseconds / 1000.0);
				++numFrames;

				// add some stupid vsync -- should really be calculated better... but what the heck... do it another day.
				//Thread.Sleep((int)(1.0 / 60.0 * 1000));

				if (!Console.KeyAvailable) continue;
				ConsoleKeyInfo ki = Console.ReadKey(true);
				if (ki.Key == ConsoleKey.Escape)
				{
					break;
				}
			}
		}

		private static void Print(int x, int y, string msg)
		{
			Console.SetCursorPosition(x, y);
			Console.Write(msg);
		}
	}
}
