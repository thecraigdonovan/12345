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
    class ToysButton : MenuItem
    {
        public ToysButton(Vector2 _startPosition) : base(_startPosition)
        {
            texture = Main.Textures["2Button"];
            text = "TOYS";
        }
    }
}