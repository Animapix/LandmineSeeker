using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LandmineSeeker
{
    public interface ScenesService
    {
        public void Load(String name);
    }

    public sealed class SceneManager: ScenesService
    {
        private Scene currentScene;
        private Dictionary<String, Scene> scenes = new Dictionary<String, Scene>();

        public void Register(String name, Scene scene)
        {
            scenes[name] = scene;
        }

        public void Load(String name)
        {
            if (!scenes.ContainsKey(name)) throw new Exception("Scene not registered");
            if (currentScene != null) currentScene.Unload();
            currentScene = scenes[name];
            currentScene.Load();
        }

        public void Update(GameTime gameTime)
        {
            if (currentScene == null) throw new Exception("No scene loaded");
            currentScene.Update(gameTime);
        }

        public void Draw()
        {
            if (currentScene == null) throw new Exception("No scene loaded");
            currentScene.Draw();
        }
    }
}
