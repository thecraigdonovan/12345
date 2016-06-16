using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using _12345.Screens.Menu;

namespace _12345.Screens
{
    public class ScreenManager
    {
        Dictionary<string, Screen> Screens;

        public GameScreen gameScreen;

        public static bool MenuActive = false;

        public static MainMenu MainMenu;

        public ScreenManager()
        {
            Screens = new Dictionary<string, Screen>();
            gameScreen = new GameScreen();
        }

        public void Load()
        {
            gameScreen.Load();
            MainMenu = new MainMenu();
        }

        public void Update(GameTime gameTime)
        {
            if(!MenuActive)
                gameScreen.Update(gameTime);


            MainMenu.Update(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            gameScreen.Draw(spriteBatch);
            MainMenu.Draw(spriteBatch);
        }

        public void ActivateScreen(string _screenName)
        {
            try
            {

            }

            catch
            {

            }
        }
    }
}