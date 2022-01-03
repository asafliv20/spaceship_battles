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

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        enum OnlineState // when we run, the system needs to know in which state in the process of the connection and the game we are,
                         // the enum helps us to understand better the current place we're in and function accordingly
        {
            AskingRole, //host or join
            Connecting,
            Playing,
            AIplaying,
            GameOver,
            AIGameOver
        }

        OnlineGame onlineGame; // create an onlineGame object which contains the server and the connection
        AIGame aiGame;
        OnlineState state = OnlineState.AskingRole; // waiting for the player to choose which us

        GraphicsDeviceManager graphics; // an object that handles the overall presentaion of the game on the screen
        SpriteBatch spriteBatch; // an object that's being used to draw texts and sprites
        Drawit bg; // a drawit object which will be the background of the game
        Drawit youWin;
        Drawit youLose;

        // Viewport leftVp, rightVp,screenVp;
         bool isH;

        public Game1() // the builidng function of the Game1 class
        {
            graphics = new GraphicsDeviceManager(this); // gives the graphics pointer a place and a value of a GraphicsDeviceManager object
            Content.RootDirectory = "Content"; // set the directory of the content application 
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true; // set the mouse to visible in the game
            base.Initialize(); // calls the intialize function from the Game XNA framework the class Game1 inherits from
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice); // constructs a spritebatch and passes the graphicsdevice as a parameter
            G.Initialize(graphics, GraphicsDevice, Content, spriteBatch, new GameTime()); // calls the initialize function from the G class, which 
                                                                          // sets important constants to our program in the G class 
     
            #region bg
            // load the background image to a variable, and passes in the required parameters
            bg = new Drawit(G.cm.Load<T2>("blue"),
                                        new V2(0, 0),
                                        null,
                                        C.White,
                                        0,
                                        new V2(0, 0),
                                        new V2(G.scale * 2),
                                        SE.None,
                                        0);
            youWin = new Drawit(G.cm.Load<T2>("you_win"),
                                       new V2(0, 0),
                                       null,
                                       C.White,
                                       0,
                                       new V2(0, 0),
                                       new V2(G.scale),
                                       SE.None,
                                       0);
            youLose = new Drawit(G.cm.Load<T2>("you_lose"),
                                       new V2(0, 0),
                                       null,
                                       C.White,
                                       0,
                                       new V2(0, 0),
                                       new V2(G.scale),
                                       SE.None,
                                       0);
            #endregion

            // leftVp = new Viewport(0, 0, G.w / 2, G.h);
            // rightVp = new Viewport(G.w / 2, 0, G.w / 2, G.h);
            // screenVp= new Viewport(0, 0, G.w, G.h);

        }
        protected override void UnloadContent()
        {
        
        }
        protected override void Update(GameTime gameTime)
        {


            G.update_input(gameTime); // updates the current input from the keyborad in order to kn

            #region zoom
            /*
            if (G.ks.IsKeyDown(Keys.P))
            {
                G.zoom  += 0.01f;
            }
            if (G.ks.IsKeyDown(Keys.O ))
            {
                G.zoom -= 0.01f;
            }
            */
            #endregion

            #region cases
            switch (state) // creates in a switch system 3 different situations for 3 different possibilities of the state variable
            {
                case OnlineState.AskingRole: // if the current online state is waiting for a role from the players
                    if (G.ks.IsKeyDown(Keys.H)) // if the H key is pressed -> if the current window is the host, the "server"
                    {
                        onlineGame = new HostOnlineGame(int.Parse(File.ReadAllText("port.txt"))); // create a hostOnlineGame class for the 
                                                                                                  // current game, and passes a byte-stream 
                                                                                                  // of the text in the port.txt file, which
                                                                                                  // contains the port of this game
                        onlineGame.OnConnection +=onlineGame_OnConnection; // adds the current running game to the event handeler onConnection
                        //onlineGame.StartCommunication();
                         isH = true;

                        state = OnlineState.Connecting; // changes the current state of the operating game to connecting
                    }
                    else if (G.ks.IsKeyDown(Keys.J)) // if the J key is pressed -> if the current window is the "client"
                    {
                        onlineGame = new JoinOnlineGame(File.ReadAllText("ip.txt"), int.Parse(File.ReadAllText("port.txt"))); // creates a 
                        // joinOnlineGame class and sends the buliding function the host's ip adress and it's port, which is the same port as 
                        // the host's port
                        onlineGame.OnConnection += onlineGame_OnConnection;
                       // onlineGame.StartCommunication();
                         isH = false;
                        state = OnlineState.Connecting;
                    }
                    else if (G.ks.IsKeyDown(Keys.K))
                    {
                        aiGame = new AIGame();
                        isH = true;
                        state = OnlineState.AIplaying;
                    }
                    break;

                case OnlineState.Connecting: // if the state of the operating game is connecting
                   

                    break;

                case OnlineState.Playing: // if the two games have connected to the main server and to eachover, we can play -> playing
                   
                    if (onlineGame.joinChar.barHeight <= 0 || onlineGame.hostChar.barHeight <= 0 || onlineGame.gameOver)
                    {
                        onlineGame.gameOver = true;
                        state = OnlineState.GameOver;
                    }

                    onlineGame.hostChar.update(onlineGame.bullets); // update the hostChar character on both screens
                    onlineGame.joinChar.update(onlineGame.bullets); // update the joinChar character on both screens

                    if (onlineGame.bullets.Count != 0) // if the list is not empty
                    {
                        foreach (var bullet in onlineGame.bullets.ToArray()) // goes through each bullet in the list
                        {
                            if (bullet.isRemoved) // if the boolean is true -> if the bullet doesn't need to be drawn or updated 
                                onlineGame.bullets.Remove(bullet); // remove the item from the list

                            if (bullet.isFlipped != onlineGame.joinChar.isFlipped) // if the sprites turn to different directions
                            {
                                bullet.Update(onlineGame.joinChar);
                            }
                            else
                            {
                                bullet.Update(onlineGame.hostChar);
                            }
                        }
                    }



                    break;

                case OnlineState.AIplaying: // if the user chose to play against AI

                    if (aiGame.aICharacter.barHeight <= 0 || aiGame.character.barHeight <= 0) // if one of the character's bars is zero
                    {
                        state = OnlineState.AIGameOver; // the game is over
                    }

                    aiGame.character.updateAI(aiGame.meteors); // update the regular character
                    aiGame.aICharacter.update(aiGame.meteors); // update the AI character

                    foreach (var meteor in aiGame.meteors.ToArray()) // goes through the meteors
                    {
                        
                        meteor.Update(aiGame.character, aiGame.aICharacter); // calls for the update function in the meteor class
                        
                    }

                    break;
                case OnlineState.GameOver:

                    break;

                case OnlineState.AIGameOver:
                    break;
            }
            #endregion

            this.Window.Title = state.ToString(); // show the users the current state the program is in

            #region zoom
            /*
            if (G.ks.IsKeyDown(Keys.A) )
            {
                G.zoom = MathHelper.Lerp(G.zoom, 0.1f, 0.01f);
            }
            if (G.ks.IsKeyDown(Keys.D) )
            {
                G.zoom = MathHelper.Lerp(G.zoom, 5, 0.001f);
            }
            base.Update(gameTime);
            */
            #endregion
        }

        void onlineGame_OnConnection()
        {
            state = OnlineState.Playing;
        }
        protected override void Draw(GameTime gameTime) // draw the objects and characters to the screen
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // start a new window and "filling" - drawing the screen in lightblue

            if (state == OnlineState.Playing) // if the programs are connected and ready to play
            {
               
                DrawViewport(); // draw the objects in the OnlineGame class

            }
            if (state == OnlineState.AIplaying)
            {
                DrawAIViewport(); // draw the objects in the AIGame class
            }

            if (state == OnlineState.GameOver) // if the online game is over
            {
                G.sb.Begin(); // begin the use of the spritebatch
                if (isH) // if the current program is the host's program
                {
                    if (onlineGame.hostChar.barHeight <= 0 || onlineGame.hostChar.barHeight < onlineGame.joinChar.barHeight)
                        // if the host's lifebar is zero / smaller than the join's lifebar
                    {
                        youLose.Draw();
                    }
                    else
                    {
                        youWin.Draw();
                    }
                }
                else
                {
                    if (onlineGame.joinChar.barHeight <= 0 || onlineGame.hostChar.barHeight > onlineGame.joinChar.barHeight)
                    {
                        youLose.Draw();
                    }
                    else
                    {
                        youWin.Draw();
                    }
                }
                G.sb.End(); // ending the use of the spritebatch 
            } 

            if (state == OnlineState.AIGameOver) // if the AI game is over
            {
                G.sb.Begin();
                if (aiGame.character.barHeight <= 0) // if the controllable character's bar height is zero
                {
                    youLose.Draw();
                }
                else
                {
                    youWin.Draw();
                }
                G.sb.End();
            }

            base.Draw(gameTime);
        }

        protected void DrawViewport() // draw the objects we want to see on the screen
        {

            G.sb.Begin(); // initializing the spritebatch for the objects

            bg.Draw(); // put on the screen (draw) the background we uploaded before

            onlineGame.joinChar.Draw(); // draw the join character on screen
            onlineGame.joinChar.draw(); // draw the lifebar on screen
            onlineGame.hostChar.Draw(); // draw the host character on screen
            onlineGame.hostChar.draw();

            if (onlineGame.bullets.Count != 0)
            {
                foreach (var bullet in onlineGame.bullets.ToArray()) // goes through the bullet list
                {
                    bullet.Draw(); // draws them
                }
            }

            G.sb.End(); // ending the use of the spritebatch 

        }

        protected void DrawAIViewport() // draw the objects we want to see on the screen // Viewport vp, Matrix camMatrix
        {
            // GraphicsDevice.Viewport = vp;

            G.sb.Begin(); // initializing the spritebatch for the objects // transformMatrix: camMatrix

            bg.Draw(); // put on the screen (draw) the background we uploaded before

            aiGame.character.Draw();
            aiGame.character.draw();
            aiGame.aICharacter.Draw();
            aiGame.aICharacter.draw();

            
            foreach (var meteor in aiGame.meteors.ToArray()) // goes through the meteors list and draws them
            {
                
                meteor.Draw();

            }

            G.sb.End(); // ending the use of the spritebatch 

        }
    }
}
