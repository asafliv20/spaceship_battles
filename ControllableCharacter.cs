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
    public class ControllableCharacter : Character
    {
        // the class that represents the controlled character objects in the program, inherits from Character
        public Keys upKey, downKey ,rightKey, leftKey;
        public V2 initialPos;
        public float speed;
        public float lastTime;
        public float timer = 0f;
        #region CTOR
        public ControllableCharacter(T2 tex, V2 pos, REC? rec, Color color,
                     F rot, V2 org, V2 scale, SE flip, F layer, bool isFlipped,
                     Keys upKey = Keys.Up, Keys downKey = Keys.Down, Keys rightKey = Keys.Right, Keys leftKey = Keys.Left,
                     Keys shoot = Keys.Space, float speed = 2f)
           : base(tex, pos, rec, color, rot, org, scale, flip, layer, isFlipped)
        {
            // a builidng function for all the controlled by keyboard objects

            this.initialPos = pos;
            //sets the keys variables to their intended values
            this.upKey = upKey;
            this.downKey = downKey;
            this.rightKey = rightKey;
            this.leftKey = leftKey;
            this.speed = speed;
            this.didShoot = false;
            this.lastTime = 0f;
        } 
        #endregion

        public override void update(List<Bullet> bullets) // listens to the keyboard state, and updates the position of the character accordingly
        {
            this.didShoot = false;
            base.update(bullets);

            if (G.ks.IsKeyDown(upKey)) // if the up key has been pressed
            {
                if (initialPos.Y > G.h / 2) // if the initial position of the object is bigger than half of the screen height
                {
                    for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                    // gives the moving a more sliding effect
                    {
                        bool limitUp = pos.Y >= G.h / 2; // the first condition to check if the player is in its borders
                        bool limitDown = pos.Y <= G.h; // the second condition to check if the player is in its borders
                        if (limitDown && limitUp) pos.Y--; // if both exist, move

                        else
                        {
                            if (!limitUp)
                                bounceDown();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < speed; i++)
                    {
                        bool limitDown = pos.Y <= G.h / 2; // the first condition to check if the player is in its borders
                        bool limitUp = pos.Y >= 0; // the second condition to check if the player is in its borders
                        if (limitDown && limitUp) pos.Y--; // if both exist, move

                        else
                        {
                            if (!limitUp)
                            {
                                bounceDown();
                            }
                        }
                    }
                    
                }
            }
            
            if (G.ks.IsKeyDown(downKey)) // if the down key has been pressed
            {
                if (initialPos.Y > G.h / 2) // if the initial position of the object is bigger than half of the screen height
                {
                    for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                    // gives the moving a more sliding effect
                    {
                        bool limitUp = pos.Y >= G.h / 2; // the first condition to check if the player is in its borders
                        bool limitDown = pos.Y <= G.h - 10; // the second condition to check if the player is in its borders
                        if (limitDown && limitUp) pos.Y++; // if both exist, move

                        else
                        {
                            if (!limitDown) // if the object has passed some border line
                            {
                                bounceUp();
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < speed; i++)
                    {
                        bool limitDown = pos.Y <= G.h / 2; // the first condition to check if the player is in its borders
                        bool limitUp = pos.Y >= 0; // the second condition to check if the player is in its borders
                        if (limitDown && limitUp) pos.Y++; // if both exist, move

                        else
                        {
                            if (!limitDown) // if the object has passed some border line
                            {
                                bounceUp();
                            }
                        }
                    }

                }
            }

            if (G.ks.IsKeyDown(leftKey))
            {
                for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
                {
                    if (!(pos.X >= -30 && pos.X <= G.w + 30))
                    {
                        pos.X = G.w + 20; // if the object reached the end of the screen, 
                                          // respawn the object on the other end
                        didShoot = false;
                    }

                    pos.X--;
                }
            }

            if (G.ks.IsKeyDown(rightKey))
            {
                for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
                {
                    if (!(pos.X >= -30 && pos.X <= G.w + 30)) {

                        pos.X = -20; // if the object reached the end of the screen,
                                     // respawn the object on the other end
                        didShoot = false;
                    }

                    pos.X++;
                   
                }
            }

            timer = (float)G.gm.TotalGameTime.Seconds;

            if (G.prevKs.IsKeyUp(space) && G.ks.IsKeyDown(space) && timer - lastTime > 1f) // if the last pressed key wasnt space and
                                                                                       // the current key is, and if the span between the current time
                                                                                      
            {
                lastTime = timer;
                this.didShoot = true;
                addBullet(bullets);

            }

        }

        private void addBullet(List<Bullet> bullets) // creates a bullet clone and adds it to the overall bullet list, 
        {
            var b = bullet.Clone() as Bullet; // calling the clone function in the bullet class
            b.pos = this.pos;
            b.org = this.org;
            b.rec = this.rec;
            b.flip = this.flip;
            b.color = this.color;
            b.layer = this.layer;

            bullets.Add(b); // adds the item to the list
        }

        
        public override void updateAI(List<Meteor> meteors) 
        {
            base.updateAI(meteors);
            if (G.ks.IsKeyDown(upKey)) // if the up key has been pressed
            {
               
                    for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                    // gives the moving a more sliding effect
                    {
                        bool limitUp = pos.Y >= G.h / 2; // the first condition to check if the player is in its borders
                        bool limitDown = pos.Y <= G.h; // the second condition to check if the player is in its borders
                        if (limitDown && limitUp) pos.Y--; // if both exist, move

                        else
                        {
                            if (!limitUp)
                                bounceDown();
                        }
                    }
                
                
            }
            
            if (G.ks.IsKeyDown(downKey)) // if the down key has been pressed
            {
                
                    for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                    // gives the moving a more sliding effect
                    {
                        bool limitUp = pos.Y >= G.h / 2; // the first condition to check if the player is in its borders
                        bool limitDown = pos.Y <= G.h - 10; // the second condition to check if the player is in its borders
                        if (limitDown && limitUp) pos.Y++; // if both exist, move

                        else
                        {
                            if (!limitDown) // if the object has passed some border line
                            {
                                bounceUp();
                            }
                        }
                    }
               
            }

            if (G.ks.IsKeyDown(leftKey))
            {
                for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
                {
                    if (!(pos.X >= -30 && pos.X <= G.w + 30))
                    {
                        pos.X = G.w + 20; // if the object reached the end of the screen, 
                                          // respawn the object on the other end
                        didShoot = false;
                    }

                    pos.X--;
                }
            }

            if (G.ks.IsKeyDown(rightKey))
            {
                for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
                {
                    if (!(pos.X >= -30 && pos.X <= G.w + 30)) {

                        pos.X = -20; // if the object reached the end of the screen,
                                     // respawn the object on the other end
                        didShoot = false;
                    }

                    pos.X++;
                   
                }
            }
        }
       
        private void bounceDown()
        {
            for (int j = 0; j < 4 * speed; j++)
            {
                pos.Y++;
            }

        }

        private void bounceUp()
        {
            for (int j = 0; j < 4 * speed; j++)
            {
                pos.Y--;
            }

        }
    }

}
