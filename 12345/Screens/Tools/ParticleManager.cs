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
using _12345.Screens.Tools.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _12345.Screens.Tools
{
    public class ParticleManager
    {
        List<Particle> Particles;

        List<Particle> ParticlesToRemove;

        List<Particle> ParticlesToAdd;
        public ParticleManager()
        {
            Particles = new List<Particle>();
            ParticlesToRemove = new List<Particle>();
            ParticlesToAdd = new List<Particle>();

        }

        public void Update(GameTime gameTime)
        {
            ParticlesToRemove = new List<Particle>();

            foreach (Particle p in ParticlesToAdd)
            {
                Particles.Add(p);
            }

            foreach (Particle p in Particles)
            {
                p.Update(gameTime);
                if (p.Finished)
                    ParticlesToRemove.Add(p);
            }

            foreach (Particle p in ParticlesToRemove)
            {
                Particles.Remove(p);
            }

            ParticlesToAdd = new List<Particle>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in Particles)
            {
                p.Draw(spriteBatch);
            }
        }

        public void Add(Particle p)
        {
            ParticlesToAdd.Add(p);
        }
    }
}