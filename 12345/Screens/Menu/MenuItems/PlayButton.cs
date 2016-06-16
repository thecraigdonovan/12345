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

namespace _12345.Screens.Menu.MenuItems
{
    class PlayButton : MenuItem
    {
        public PlayButton(Vector2 _startPosition) : base(_startPosition)
        {
            texture = Main.Textures["1Button"];
            text = "PLAY";
        }

        public override void OnClick()
        {
            GameScreen.StartGame();
            ScreenManager.MainMenu.MoveOut();
            base.OnClick();
        }
    }
}