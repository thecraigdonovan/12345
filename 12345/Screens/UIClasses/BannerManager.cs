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

namespace _12345.Screens.UIClasses
{
    public class BannerManager
    {
        public Banner TopBanner, BottomBanner;

        public BannerManager()
        {
            TopBanner = new Banner(Vector2.Zero, new Vector2(0, 138));
            BottomBanner = new Banner(new Vector2(0, 1855), new Vector2(0, 1855 - 138));
        }

        public void Update(GameTime gameTime)
        {
            TopBanner.Update(gameTime);
            BottomBanner.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TopBanner.Draw(spriteBatch);
            BottomBanner.Draw(spriteBatch);
        }
    }
}