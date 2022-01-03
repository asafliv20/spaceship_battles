#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
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
    public delegate void OnConnectionHandler();

    abstract class OnlineGame
    {
        // the parent class for JoinOnlineGame and HostOnlineGame objects, containing 
        protected BinaryReader reader; // creates a BinaryReader object pointer
        protected BinaryWriter writer; // creates a BinaryReader object pointer

        protected Thread thread; // creates Thread-type pointer

        protected TcpClient client; // creates TcpClient pointer

        protected int port;

        public Character hostChar, joinChar;

        public  event OnConnectionHandler OnConnection;

        public bool gameOver;

        public List<Bullet> bullets; // list of all the bullets being shot from the characters

        #region funcs to overide
        protected abstract void InitChars(); // creates an abstract function, which the classes who inherit OnlineGame class can override


        protected abstract void SocketThread(); // creates an abstract function, which the classes who inherit this class will override
        #endregion
        
        protected void RaiseOnConnectionEvent()
        {

            OnConnection?.Invoke();
        }

        public void StartCommunication() // creates a thread, which will handle the SocketThread function, and starts it
        {
         
            thread = new Thread(new ThreadStart(SocketThread));
            thread.IsBackground = true;
            thread.Start();
        }

        protected void ReadAndUpdateCharacter(Character c) // updating the receiving character's position from the data being send to us
        {
            c.pos.X = reader.ReadSingle (); // read the 4-bytes point from the stream and move to current position of the 
                                           // stream by 4 bytes
            c.pos.Y = reader.ReadSingle();
           
            
            if (reader.ReadBoolean())
            {
                if (c.isFlipped) // if the character's flip variable is not none -> if the isFlipped boolean is true
                {
                    bullets.Add(new Bullet(G.cm.Load<T2>("laser"), // add to the bullets list a new bullet
                    new V2(c.pos.X , c.pos.Y) * G.scale,
                    null,
                    Color.White,
                    0,// MH.ToRadians(180),
                    new V2(15, 15), // 515, 548
                    new V2(0.35f, 0.35f),
                    c.flip,
                    0,
                    true));
                }
                else
                { // if the character's flip variable is not none -> if the isFlipped boolean is true
                    bullets.Add(new Bullet(G.cm.Load<T2>("laser"),
                    new V2(c.pos.X , c.pos.Y ) * G.scale,
                    null,
                    Color.White,
                    0,// MH.ToRadians(180),
                    new V2(15, 15), // 515, 548
                    new V2(0.35f, 0.35f),
                    c.flip,
                    0,
                    false));
                }
            }
            

        }

        protected void WriteCharacterData(Character c) // converts to bytes the position of the passed character to the stream
        {
            writer.Write(c.pos.X); // write a 4-bytes floating point value to the stream and move to current position of the 
                                   // stream by 4 bytes
            writer.Write(c.pos.Y);
            writer.Write(c.didShoot);
        }



    }
}
