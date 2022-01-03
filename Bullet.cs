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
    public class Bullet : ICloneable
    {
        public T2 tex; // the texture of the object -> the image
        public V2 pos; // initial position
        public REC? rec;
        public Color color;
        public F rot;
        public V2 org;
        public V2 scale; // the size of the object
        public SE flip;
        public F layer;
        public int speed;
        public bool isRemoved; // for Bullet object - a boolean which tells if the lifespan of the bullet is over
        public bool isFlipped;
        public static float height = 16;
        public static float width = 4;
        
        public Bullet(T2 tex, V2 pos, REC? rec, Color color, F rot, V2 org, V2 scale, SE flip, F layer, bool isFlipped,
            int speed = 2) // a building function for the bullet class
        {
            this.tex = tex;
            this.pos = pos;
            this.rec = rec;
            this.color = color;
            this.rot = rot;
            this.org = org;
            this.scale = scale;
            this.flip = flip;
            this.layer = layer;
            this.isFlipped = isFlipped; // if the bullet texture is flipped, depending on the flip variable
        }

        public void Draw() // draw the object using its variables
        {
            G.sb.Draw(tex, pos, rec, color, rot, org, scale, flip, layer);

        }

        public void Update(Character c)
        {


            bool flag = pos.Y <= 0 || pos.Y >= G.h;

            if (flag)
            {
                isRemoved = true;

            }

            if (isFlipped)
            {
                pos.Y += 2;
            }
            else
            {
                pos.Y -= 2;
            }

            collision(c);
            
        }

        public void collision(Character c)
        { 

            if ((Math.Abs((pos.X + 5) - c.pos.X) <= c.width) && (Math.Abs((pos.Y + 5) - c.pos.Y) <= c.height))
            {
                if (c.isFlipped != isFlipped)
                {
                    isRemoved = true;
                  
                    c.barHeight -= 2;
                    c.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, c.barHeight);
                    
                    
                }

            }
            
        }

        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
