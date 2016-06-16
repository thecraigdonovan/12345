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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _12345.Screens.Menu.MenuItems;

namespace _12345.Screens.Menu
{
    public class MainMenu
    {
        List<MenuItem> MenuItems;

        public enum MovementState
        {
            MovingIn,
            In,
            MovingOut,
            Out
        }

        public MovementState CurrentMovementState = MovementState.In;


        public MainMenu()
        {
            MenuItems = new List<MenuItem>();
            MenuItems.Add(new PlayButton(new Vector2(0, 0)));
            MenuItems.Add(new ToysButton(new Vector2(0, 384)));
            MenuItems.Add(new ScoresButton(new Vector2(0, 768)));
            MenuItems.Add(new SettingsButton(new Vector2(0, 1152)));
            MenuItems.Add(new QuitButton(new Vector2(0, 1536)));
            ScreenManager.MenuActive = true;
        }

        public void Update(GameTime gameTime)
        {
            foreach(MenuItem menuItem in MenuItems)
            {
                menuItem.Update(gameTime);

                if (CurrentMovementState == MovementState.In)
                {
                    if (Main.CurrentTouchCollection.Count == 0 && Main.LastTouchCollection.Count > 0)
                    {
                        if (menuItem.Hitbox.Contains(new Point((int)((Main.LastTouchCollection[0].Position.X + (GameScreen.Camera.Pos.X - 540)) * Main.xScale), (int)((Main.LastTouchCollection[0].Position.Y + (GameScreen.Camera.Pos.Y - 960)) * Main.yScale))))
                        {
                            menuItem.OnClick();
                        }
                    }

                    else
                    {
                        if (Main.CurrentTouchCollection.Count > 0)
                        {
                            if (menuItem.Hitbox.Contains(new Point((int)((Main.CurrentTouchCollection[0].Position.X + (GameScreen.Camera.Pos.X - 540)) * Main.xScale), (int)((Main.CurrentTouchCollection[0].Position.Y + (GameScreen.Camera.Pos.Y - 960)) * Main.yScale))))
                            {
                                menuItem.OnHover();
                            }
                        }
                    }
                }
            }

            if (CurrentMovementState == MovementState.MovingOut)
            {
                bool allOut = true;

                foreach (MenuItem menuItem in MenuItems)
                {
                    if (menuItem.CurrentMovementState != MenuItem.MovementState.Out)
                    {
                        allOut = false;
                        break;
                    }
                }

                if (allOut == true)
                {
                    CurrentMovementState = MovementState.Out;
                    ScreenManager.MenuActive = false;
                }
            }

            if(CurrentMovementState == MovementState.MovingIn)
            {
                bool allIn = true;
                foreach(MenuItem menuItem in MenuItems)
                {
                    if (menuItem.CurrentMovementState != MenuItem.MovementState.In)
                    {
                        allIn = false;
                        break;
                    }
                }

                if (allIn == true)
                {
                    CurrentMovementState = MovementState.In;
                }
            }
        }


        public void MoveOut()
        {
            CurrentMovementState = MovementState.MovingOut;

            foreach (MenuItem menuItem in MenuItems)
            {
                menuItem.CurrentMovementState = MenuItem.MovementState.MovingOut;
            }
        }

        public void MoveIn()
        {
            CurrentMovementState = MovementState.MovingIn;

            foreach (MenuItem menuItem in MenuItems)
            {
                menuItem.CurrentMovementState = MenuItem.MovementState.MovingIn;
            }

            ScreenManager.MenuActive = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (MenuItem menuItem in MenuItems)
            {
                menuItem.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}