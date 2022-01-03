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
    class JoinOnlineGame : OnlineGame
    {
        string hostip; // creates a pointer for the ip adress of the host

        public JoinOnlineGame(string hostip, int port) // building function
        {
            this.port = port; // gives the port pointer it's received value
            this.hostip = hostip; // gives the hostip point it's received value
            InitChars(); // initialize the characters of the players
            StartCommunication(); // starts a background thread which will be handling the connection of the two programs, and the
                                  // information being received and sent to and from each program
            
        }

        protected override void InitChars() // creates the character
        {

            bullets = new List<Bullet>();

            hostChar = new Character(G.cm.Load<T2>("p1"), // create a character object -> the other program's character
                            new V2(G.w / 2, G.h / 2 + 75) * G.scale,
                            null,
                            C.White,
                            0,
                            new V2(15, 15), // 515, 548
                            new V2(0.4f, 0.4f),
                            SE.None,
                            0,
                            false);

            joinChar = new ControllableCharacter(G.cm.Load<T2>("p2"), // create a ControllableCharacter object -> the currnet program's
                                                                      // character
                            new V2(G.w / 2, G.h / 2 - 75) * G.scale,
                            null,
                            C.White,
                            0,
                            new V2(15, 15), // 515, 548
                            new V2(0.4f, 0.4f),
                            SE.FlipVertically,
                            0,
                            true);
        }

        protected override void SocketThread() // constructs the program's socket, connects to the other program, and start sending 
                                               // and reciveing information
        {
            client = new TcpClient(); // creates a TcpClient
            client.Connect(hostip, port); // send a request to connect to the network with the same ip adress and same port

            reader = new BinaryReader(client.GetStream()); // creates a BinaryReader object, with the NetworkStream used to receive and
                                                           // send data 
            writer = new BinaryWriter(client.GetStream());

            RaiseOnConnectionEvent();

            while (true) // an endless loop, operating in a background thread, that receives and sends the characters location, and 
                         // pausing the current thread
            {
                ReadAndUpdateCharacter(hostChar);
                WriteCharacterData(joinChar);
                

                Thread.Sleep(10); // pausing the current thread by 10 milliseconds
            }
         }
    }
}
