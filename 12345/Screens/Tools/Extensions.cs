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

namespace _12345.Screens.Tools
{
    public static class Extensions
    {
        public static float GetAngle(Vector2 a, Vector2 b)
        {
            float angle = (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
            return angle;
        }
    }
}