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
    public class Drawit
    {
        // the class which contains the basic parameters for a drawable object in the program, also the parent class of the Character
        // class
        #region Names
        public T2 tex; // the texture of the object -> the image
        public V2 pos; // initial position
        public REC? rec;
        public Color color;
        public F rot;
        public V2 org;
        public V2 scale; // the size of the object
        public SE flip;
        public F layer;
        
        #endregion
        #region CTOR
        public Drawit(T2 tex, V2 pos, REC? rec, Color color, 
                      F rot, V2 org, V2 scale, SE flip, F layer)
        {
            // a building function for regular characters
            this.tex = tex;
            this.pos = pos;
            this.rec = rec;
            this.color = color;
            this.rot = rot;
            this.org = org;
            this.scale = scale;
            this.flip = flip;
            this.layer = layer;
        }
        /*
        public Drawit(T2 tex, V2 pos, REC? rec, Color color,
                      F rot, V2 org, V2 scale, SE flip, bool isRemoved = false, float lifeSpan = 3f) // a building function for the 
                                                                                                     // Bullet class
        {
            this.tex = tex;
            this.pos = pos;
            this.rec = rec;
            this.color = color;
            this.rot = rot;
            this.scale = scale;
            this.flip = flip;
            this.isRemoved = isRemoved;
            this.lifeSpan = lifeSpan;
        }
        */
        #endregion
        #region Draw
        public void Draw() // draw the object using its variables
        {
            G.sb.Draw( tex,  pos,  rec,  color, rot,  org,  scale,  flip,  layer);
                    
        }
       
        #endregion
        
    }
}
