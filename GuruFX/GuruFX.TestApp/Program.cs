using System;
using System.Diagnostics;
using GuruFX.Core;
using GuruFX.Core.Components;
using GuruFX.Core.Entities;
using GuruFX.Core.Scenes;
using GuruFX.Core.SystemComponents;

namespace GuruFX.TestApp
{
	internal class Program
	{
		private static void Main()
		{
			SceneUpdater sceneUpdater = new SceneUpdater();
			SceneRenderer sceneRenderer = new SceneRenderer();

			Scene s = new Scene();
			s.AddComponent(sceneUpdater);
			s.AddComponent(sceneRenderer);

			IEntity e = new GameObject();
			s.AddEntity(e);
			e.AddComponent(new Behaviour()); 
			e.AddComponent(new FakeRenderable());

			// simple game loop
			double elapsedTime = 0;
			double lastElapsedTime = 0;
			double deltaTime = 0;

			Stopwatch w = new Stopwatch();
			w.Start();
			
			while(true)
			{
				int line = 0;
				sceneUpdater.EntitiesUpdated = 0;
				sceneRenderer.EntitiesRendered = 0;
				
				deltaTime = elapsedTime - lastElapsedTime;

				s.Update(elapsedTime, deltaTime);
							
				Print(0, line++, $"Elapsed Time: {elapsedTime}");
				Print(0, line++, $"Delta Time: {deltaTime}");
				Print(0, line++, $"Entities Updated: {sceneUpdater.EntitiesUpdated}");
				Print(0, line++, $"Entities Rendered: {sceneRenderer.EntitiesRendered}");

				lastElapsedTime = elapsedTime;
				elapsedTime = (w.ElapsedMilliseconds / 1000.0);
			}
		}

		static void Print(int x, int y, string msg)
		{
			Console.SetCursorPosition(x, y);
			Console.Write(msg);
		}
	}
}
