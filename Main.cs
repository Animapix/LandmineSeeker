using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace LandmineSeeker
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;

        public static Rectangle canvas = new Rectangle(0, 0, 60*16, 30*16);

        private SceneManager sceneManager;
        private AssetsManager assetsManager;
        private InputsManager inputsManager;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            renderTarget = new RenderTarget2D(GraphicsDevice, canvas.Width, canvas.Height);

            graphics.PreferredBackBufferWidth = canvas.Width*2;
            graphics.PreferredBackBufferHeight = canvas.Height*2;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            ServiceLocator.RegisterService(Content);
            inputsManager = new InputsManager();
            ServiceLocator.RegisterService((InputsService)inputsManager);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ServiceLocator.RegisterService(spriteBatch);
            sceneManager = new SceneManager();
            ServiceLocator.RegisterService((ScenesService)sceneManager);
            assetsManager = new AssetsManager();
            ServiceLocator.RegisterService((AssetsService)assetsManager);

            // Load textures
            assetsManager.LoadAsset<Texture2D>("Player");
            assetsManager.LoadAsset<Texture2D>("Infantry");
            assetsManager.LoadAsset<Texture2D>("Mine");
            assetsManager.LoadAsset<Texture2D>("Detector");
            assetsManager.LoadAsset<Texture2D>("TileSet");
            assetsManager.LoadAsset<Texture2D>("MineHole");
            assetsManager.LoadAsset<Texture2D>("explosion");
            assetsManager.LoadAsset<Texture2D>("Warning");
            // Load Sounds
            assetsManager.LoadAsset<SoundEffect>("beep-sound");
            assetsManager.LoadAsset<SoundEffect>("Explosion_Fast");

            // Load Fonts
            assetsManager.LoadAsset<SpriteFont>("defaultFont"); 

            // Load scenes
            sceneManager.Register("Game", new SceneGame());
            sceneManager.Register("Menu", new SceneMenu());
            sceneManager.Load("Menu");
        }

        protected override void Update(GameTime gameTime)
        {
            
            sceneManager.Update(gameTime);
            inputsManager.UpdateState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            sceneManager.Draw();
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            
            DrawCanvas();

            base.Draw(gameTime);
        }

        private void DrawCanvas()
        {
            float ratio = 1;
            int marginV = 0;
            int marginH = 0;
            float currentAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float virtualAspect = (float)canvas.Width / (float)canvas.Height;
            
            if (canvas.Height != this.Window.ClientBounds.Height)
            {
                if (currentAspect > virtualAspect)
                {
                    ratio = Window.ClientBounds.Height / (float)canvas.Height;
                    marginH = (int)((Window.ClientBounds.Width - canvas.Width * ratio) / 2);
                }
                else
                {
                    ratio = Window.ClientBounds.Width / (float)canvas.Width;
                    marginV = (int)((Window.ClientBounds.Height - canvas.Height * ratio) / 2);
                }
            }

            Rectangle dst = new Rectangle(marginH, marginV, (int)(canvas.Width * ratio), (int)(canvas.Height * ratio));

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, dst, Color.White);
            spriteBatch.End();

        }
    }
}