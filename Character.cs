#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion
#region Shortcuts
using GDM = Microsoft.Xna.Framework.GraphicsDeviceManager;
using GD = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using CM = Microsoft.Xna.Framework.Content.ContentManager;
using SB = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using V2 = Microsoft.Xna.Framework.Vector2;
using V3 = Microsoft.Xna.Framework.Vector3;
using MH = Microsoft.Xna.Framework.MathHelper;
using MX = Microsoft.Xna.Framework.Matrix;
using T2 = Microsoft.Xna.Framework.Graphics.Texture2D;
using F = System.Single;
using SE = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using REC = Microsoft.Xna.Framework.Rectangle;
using KS = Microsoft.Xna.Framework.Input.KeyboardState;
using MS = Microsoft.Xna.Framework.Input.MouseState;
using C = Microsoft.Xna.Framework.Color;
using BSM = Microsoft.Xna.Framework.Graphics.BlendState;
using SSM = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
#endregion

namespace online
{
    public class Character : Drawit
    {
        // this class is the basic class for all the characters in the program, the parent class of Controllablecharacter and the 
        // child class of DrawIt
        public Bullet bullet; // creates a Bullet-type pointer for when the user will press space
        public Keys space; // pointer for the Keys.Space value
        public MX mat;
        public Rectangle bar;
        public bool didShoot;
        public bool isFlipped;
        public float height = 40;
        public float width = 30;
        public int barHeight;
        public Random rnd;
        #region CTOR
        public Character(T2 tex, V2 pos, REC? rec, Color color,
                      F rot, V2 org, V2 scale, SE flip, F layer, bool isFlipped, Keys space = Keys.Space)
            : base(tex, pos, rec, color, rot, org, scale, flip, layer) // a building function for the spaceships
        {
            this.space = space;
            this.rnd = new Random();
            this.didShoot = false;
            this.isFlipped = isFlipped;
            this.barHeight = 25;
            this.bullet = new Bullet(G.cm.Load<T2>("laser"), pos, rec, color, rot, org, new V2(0.3f, 0.3f), flip, layer, isFlipped);
            this.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, barHeight);

        }

        #endregion

        #region update

        public virtual void update(List<Bullet> bullets) // updates the characters appearence 
        {
            mat = MX.CreateTranslation(-pos.X , -pos.Y , 0)*
                MX.CreateScale(G.zoom, G.zoom, 1)*
                MX.CreateTranslation(G.w/2, G.h/2, 0);

           
            this.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, barHeight);
            
        }

        public void draw()
        {
            if (isFlipped)
            {
                G.sb.Draw(G.cm.Load<T2>("red_lifebar"), this.bar, C.White);
            }
            else
            {
                G.sb.Draw(G.cm.Load<T2>("green_lifebar"), this.bar, C.White);
            }
        }

        public virtual void updateAI(List<Meteor> meteors)
        {
            mat = MX.CreateTranslation(-pos.X, -pos.Y, 0) *
               MX.CreateScale(G.zoom, G.zoom, 1) *
               MX.CreateTranslation(G.w / 2, G.h / 2, 0);


            this.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, barHeight);

            
        }
        

        #endregion

    }
}
