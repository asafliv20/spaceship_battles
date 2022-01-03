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
    public class AIGame 
    {
        // creates the character's pointers, in order to access them from Game1
        public ControllableCharacter character;
        public AICharacter aICharacter;

        // a list of meteors, keeps access to all the meteors 
        public List<Meteor> meteors;

        public AIGame () // a building function for the AIGame class
        {
            initChars();
        }

        public void initChars() 
         // initialize the variables
        {

            // fill randomize meteors to the list
            meteors = new List<Meteor>();

            Random rnd = new Random();

            for (int i = 0; i < 5; i++) // a for loop that runs 5 times -> create 5 different meteors
            {
                
                V2 position = new V2(0, rnd.Next(50, G.h - 50)); // randomize the position

                var m = new Meteor(G.cm.Load<T2>("meteor" + i), // loading the image using the load method in the ContentManager object
                    position * G.scale,
                    null,
                    C.White,
                    0,
                    new V2(15, 15),
                    new V2(1f, 1f),
                    SE.None,
                    0,
                    false);

                if (i % 2 == 0) // set the speed and its direction according to index in the for loop 
                {
                    m.speed = new V2(rnd.Next(1, 4), rnd.Next(2, 4));
                }
                else
                {
                    m.speed = new V2(rnd.Next(1, 4), -1 * rnd.Next(2, 4));

                }


                meteors.Add(m);
            }

            // create the new Characters and set their pointer to the pointers above

            character = new ControllableCharacter(G.cm.Load<T2>("p1"), // loading the image using the load method in the ContentManager object
                                        new V2(G.w / 2, G.h / 2 + 90) * G.scale, // create a ControllableCharacter object -> the currnet program's
                                                                                 // character
                                        null,
                                        C.White,
                                        0,
                                        new V2(15, 15),
                                        new V2(0.4f, 0.4f),
                                        SE.None,
                                        0,
                                        false);

            aICharacter = new AICharacter(G.cm.Load<T2>("p3"),
                                        new V2(G.w / 2, G.h / 2 - 90) * G.scale,
                                        null,
                                        C.White,
                                        0,
                                        new V2(15, 15),
                                        new V2(0.4f, 0.4f),
                                        SE.FlipVertically,
                                        0,
                                        true);
        }
    }
}
