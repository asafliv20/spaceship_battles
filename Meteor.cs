#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public class Meteor : ICloneable
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
        public V2 speed;
        public bool isRemoved;
        public Random rnd; // random class, for randomizing the direction and the velocitiy of the meteor
        public int direction;

        public Meteor(T2 tex, V2 pos, REC? rec, Color color,
                      F rot, V2 org, V2 scale, SE flip, F layer, bool isRemoved)
            // a building function for Meteor 
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
            this.direction = 0; // set the direction pointer from null to an integer, set his value
            this.isRemoved = isRemoved;

            rnd = new Random(); // creates a new random object
            
            
        }

        public void Draw() // draw the object using its variables
        {
            G.sb.Draw(tex, pos, rec, color, rot, org, scale, flip, layer);

        }

        public void Update(Character c, AICharacter aiChar) // update the meteor and check for collisions from the two characters
        {
            // check collision
            if (Math.Abs(pos.Y - c.pos.Y) <= 20 && (Math.Abs(pos.X - c.pos.X) <= 20)) // if the absolute distance between the sprites's
                                                                                      // components is less/equals to 20
            {
                c.barHeight -= 1; // subtract from the life bar
                c.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, c.barHeight); // update the bar

            }
            if (Math.Abs(pos.Y - aiChar.pos.Y) <= 20 && (Math.Abs(pos.X - aiChar.pos.X) <= 20)) // if the absolute distance between the sprites's
                                                                                                // components is less/equals to 20
            {
                //isRemoved = true;
                aiChar.barHeight -= 1;
                aiChar.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, aiChar.barHeight);

            }

            if (pos.X > G.w + 20) // if the meteor reached the right side limit
            {
                direction = rnd.Next(0, 1);
                // change his direction, and randomize again his direction and velocity, and respawn him at the left side
                if (direction == 1)
                {
                    this.speed = new V2(rnd.Next(2, 4), 1 * rnd.Next(1, 3));
                }
                else
                {
                    this.speed = new V2(rnd.Next(2, 4), -1 * rnd.Next(1, 3));
                }
                pos.X = -10;
            }
            if (pos.Y < -20) // if the meteor reached the bottom of the screen
            {
                pos.Y = G.h + 10; // respawn him at the top of the screen
            }
            if (pos.Y > G.h + 20) // if the meteor reached the top of the screen
            {
                pos.Y = -10; // respawn him at the bottom of the screen
            }

            pos += speed; // move the meteor according to his speed
        }

        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
