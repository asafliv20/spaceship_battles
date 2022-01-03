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
using GM = Microsoft.Xna.Framework.GameTime;
#endregion
namespace online
{
    static class G
    // a static class which contains all the important constants that initialize once (in order to prevernt duplicate code)
    // and will be used from other classes
    {
        #region Names
        public static GDM gdm; // GraphicsDeviceManager
        public static GD gd; // GraphicsDevice
        public static CM cm; // ContentManager
        public static SB sb; // SpriteBatch
        public static KS ks; // KeyBoardState
        public static KS prevKs;
        public static int w, h;
        public static GM gm;
        public static float scale = 1f;
        public static float zoom = 0.5f;
        #endregion
        #region Initialize
        public static void Initialize(GDM gdm, GD gd, CM cm, SB sb, GM gm) // a builind function
        {
            G.gdm = gdm;
            G.gm = gm;
            G.gd = gd;
            G.cm = cm;
            G.sb = sb;
            w = gdm.PreferredBackBufferWidth = 512;
            h = gdm.PreferredBackBufferHeight = 512;
            gdm.ApplyChanges();

            
        }

        #endregion
        public static void update_input(GameTime gameTime) // updating the current keyboard's state
        {
            gm = gameTime;
            prevKs = ks;
            ks = Keyboard.GetState();
        }
        public static V2  sovev_vector(F zavit)
        {
            MX cli_sivoov = MX.CreateRotationZ(zavit);
            return V2.Transform(-V2.UnitY, cli_sivoov);
        }
    }
}
