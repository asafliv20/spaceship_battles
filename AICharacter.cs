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
    public class AICharacter
    {
        #region variables
        public Bullet bullet;
        public T2 tex; // the texture of the object -> the image
        public V2 pos; // initial position
        public REC? rec;
        public Color color;
        public F rot;
        public V2 org;
        public V2 scale; // the size of the object
        public SE flip;
        public F layer;
        public bool isFlipped;
        public float speed;
        public bool isMoveLeft;
        public bool isMoveRight;
        public bool canShoot;
        public int barHeight;
        public Rectangle bar;
        public bool didMove;
       

        #endregion

        public AICharacter (T2 tex, V2 pos, REC? rec, Color color, F rot, V2 org, V2 scale, SE flip, F layer, bool isFlipped)
            
        {
            // a building function for the AICharacter
            this.tex = tex;
            this.pos = pos;
            this.rec = rec;
            this.color = color;
            this.rot = rot;
            this.org = org;
            this.scale = scale;
            this.flip = flip;
            this.layer = layer;
            this.isFlipped = isFlipped;
            this.speed = 3f;
            this.isMoveLeft = true;
            this.isMoveRight = true;
            this.didMove = false;
            this.barHeight = 25;
            this.bullet = new Bullet(G.cm.Load<T2>("laser"), pos, rec, color, rot, org, new V2(0.35f, 0.35f), flip, layer, isFlipped);
            this.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, barHeight);

          
        }

        public void Draw() // draw the object using its variables
        {
            G.sb.Draw(tex, pos, rec, color, rot, org, scale, flip, layer);

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

        public void update(List<Meteor> meteors) // updates the sprite given the meteor's position
        {

            this.bar = new Rectangle((int)pos.X - 15, (int)pos.Y - 5, 6, barHeight); // update the life bar

           
            foreach (var meteor in meteors) // go through the meteors
            {
                
                

                if (pos.X > meteor.pos.X && pos.X < meteor.pos.X + 30) // if there's a meteor to the character's left, and in 20px 
                                                                       // he'll be on his right, meaning if the meteor is close to the
                                                                       // sprite
                {

                    if (pos.Y <= meteor.pos.Y + 30 && pos.Y > meteor.pos.Y) // if the meteor is close to the character and the meteor 
                                                                            // is coming from below
                    {
                        if (!(pos.Y > 50)) // if the position allows the character to move up
                        {
                            moveDown();
                            moveRight();
                            moveRight();
                            break;
                        }
                        else
                        {
                            moveUp();
                            moveRight();
                            break;
                        }
                    }

                    if (pos.Y >= meteor.pos.Y - 30 && pos.Y < meteor.pos.Y ) // if the meteor is above the sprite and near him
                    {
                        if (pos.Y < G.h / 2) // if the position allows the character to go down
                        {
                            moveDown();
                            moveLeft();
                            break;
                        }
                        else
                        {
                            moveLeft();
                            moveLeft();
                            break;
                        }
                    }

                    if (MathHelper.Distance(pos.Y, meteor.pos.Y) <= 65) // if the distance between the two Y position of the character
                                                                        // and the meteor
                    {
                        if (pos.Y > meteor.pos.Y) // if the character is above the meteor
                        {
                            if (meteor.speed.Y > 0 && isMoveLeft) // if the meteor is advancing upwards
                            {
                                if (meteor.pos.X + 30 >= pos.X) // if the meteor is near the character's X position
                                {
                                    moveLeft();
                                    moveLeft();
                                }
                            }
                        }
                        else
                        {
                            if (meteor.pos.X < pos.X && meteor.speed.Y < 0 && isMoveRight) // if the meteor is on the character's left
                                                                                           // and its advancing downwards
                            {
                                moveRight();
                                moveUp();
                                break;
                            }
                           
                        }
                    }

                }
                else if (pos.X == meteor.pos.X && pos.Y > meteor.pos.Y) // if the X position of the two sprites is identical and the 
                                                                        // character is above the meteor
                {
                    moveLeft();
                }
                
                else if (pos.X < meteor.pos.X && pos.Y < meteor.pos.Y + 20 && pos.Y > meteor.pos.Y) // if the character is below the meteor
                                                                                                    // and near his Y position
                {
                    moveDown();
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

        private void moveLeft()
        {
           
            if (pos.X > 30)
            {
                isMoveLeft = true;
            }
            for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
                {
                if (!(pos.X >= 0 && pos.X <= G.w))
                {
                        moveRight();
                        isMoveLeft = false;

                        break;
                }

                pos.X--;
            }
            
            
        }

        private void moveRight()
        {
            
                if (pos.X < G.w - 30)
                {
                    isMoveRight = true;
                }
                else
                {
                  
                }
                for (int i = 0; i < 2; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                            // gives the moving a more sliding effect
                {
                    if (!(pos.X >= 0 && pos.X <= G.w))
                    {

                        moveLeft();
                        isMoveRight = false;

                        break;
                    }

                    pos.X++;

                }
            
            
        }

        private void moveUp()
        {
            
             for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
             {
                bool limitUp = pos.Y >= 0; // the first condition to check if the player is in its borders
                bool limitDown = pos.Y <= G.h / 2; // the second condition to check if the player is in its borders
                if (limitDown && limitUp) pos.Y--; // if both exist, move

                else
                {
                    if (!limitUp)
                    {
                        bounceDown();
                        moveDown();
                    }
                }
             }
            
        }
        
        private void moveDown()
        {
           
            for (int i = 0; i < speed; i++) // instead of moving the object x pixles at once, move the object 1 pixles x times
                                                // gives the moving a more sliding effect
            {
                bool limitUp = pos.Y >= 0; // the first condition to check if the player is in its borders
                bool limitDown = pos.Y <= G.h / 2; // the second condition to check if the player is in its borders
                if (limitDown && limitUp) pos.Y++; // if both exist, move

                else
                {
                    if (!limitDown) // if the object has passed some border line
                    {
                        bounceUp();
                        moveUp();
                    }
                }
            }
        }


    }
}
