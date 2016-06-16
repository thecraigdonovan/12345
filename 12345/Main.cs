using _12345.Screens;
using _12345.Screens.Tools;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace _12345
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SpriteFont> Fonts;

        public static ScreenManager ScreenManager;

        public static Random Rand;

        public static float xScale = 1, yScale = 1;
        RenderTarget2D rt;

        public static TouchCollection CurrentTouchCollection, LastTouchCollection;

        public static ParticleManager ParticleManager;

        public static Accelerometer Accel;

        static bool Quit = false;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            rt = new RenderTarget2D(graphics.GraphicsDevice, 1080, 1920);
            xScale = 1080f / (float)graphics.GraphicsDevice.Viewport.Width;
            yScale = 1920f / (float)graphics.GraphicsDevice.Viewport.Height;
            Accel = new Accelerometer();
            Accel.Start();
            Rand = new Random();
            ScreenManager = new ScreenManager();
            ParticleManager = new ParticleManager();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadTextures();
            LoadFonts();
            // TODO: use this.Content to load your game content here
            ScreenManager.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Quit)
                System.Environment.Exit(0);

            // TODO: Add your update logic here
            CurrentTouchCollection = TouchPanel.GetState();
            ScreenManager.Update(gameTime);
            ParticleManager.Update(gameTime);
            LastTouchCollection = CurrentTouchCollection;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            graphics.GraphicsDevice.SetRenderTarget(rt);
            ScreenManager.Draw(spriteBatch);
            graphics.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();

            spriteBatch.Draw(rt, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadTextures()
        {
            Textures = new Dictionary<string, Texture2D>();

            Textures.Add("Background", Content.Load<Texture2D>("12345/Images/Gameplay/Background"));
            Textures.Add("TileBoard", Content.Load<Texture2D>("12345/Images/Gameplay/TileBoard"));
            Textures.Add("TileSlots", Content.Load<Texture2D>("12345/Images/Gameplay/TileSlot"));
            Textures.Add("ScoreBoard", Content.Load<Texture2D>("12345/Images/Gameplay/ScoreBoard"));

            Textures.Add("BottomBoard", Content.Load<Texture2D>("12345/Images/Gameplay/BottomBoard"));

            Textures.Add("BlankTile", Content.Load<Texture2D>("12345/Images/Gameplay/BlankTile"));
            Textures.Add("Ribbon", Content.Load<Texture2D>("12345/Images/Gameplay/Ribbon"));
            Textures.Add("Banner", Content.Load<Texture2D>("12345/Images/Gameplay/Banner"));

            Textures.Add("1Button", Content.Load<Texture2D>("12345/Images/Menu/1Button"));
            Textures.Add("2Button", Content.Load<Texture2D>("12345/Images/Menu/2Button"));
            Textures.Add("3Button", Content.Load<Texture2D>("12345/Images/Menu/3Button"));
            Textures.Add("4Button", Content.Load<Texture2D>("12345/Images/Menu/4Button"));
            Textures.Add("5Button", Content.Load<Texture2D>("12345/Images/Menu/5Button"));

        }

        private void LoadFonts()
        {
            Fonts = new Dictionary<string, SpriteFont>();
            Fonts.Add("TileFont", Content.Load<SpriteFont>("12345/Fonts/TileFont"));
            Fonts.Add("BannerFont", Content.Load<SpriteFont>("12345/Fonts/BannerFont"));
            Fonts.Add("MenuFont", Content.Load<SpriteFont>("12345/Fonts/MenuFont"));
            Fonts["MenuFont"].LineSpacing = -20;

        }

        public static void ExitGame()
        {
            Quit = true;
        }
    }
}