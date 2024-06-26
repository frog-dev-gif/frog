using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrogGame.Entities
{
    public class Frog
    {
        // Textures for the frog
        public Texture2D IdleTexture { get; set; }
        public Texture2D IdleAnimation { get; set; }
        public Texture2D MovementSpriteSheet { get; set; }
        public Texture2D UpMovementSpriteSheet { get; set; }
        public Texture2D DownMovementSpriteSheet { get; set; }

        // Position and movement variables
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        // Animation variables
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public int FrameCount { get; private set; }
        public int CurrentFrame { get; private set; }
        public double TimePerFrame { get; private set; }
        public double TotalElapsed { get; private set; }

        // Movement state
        public bool IsMoving { get; set; }
        public bool IsFacingLeft { get; set; }
        public string MovementDirection { get; set; }

        // Render size
        public int RenderWidth { get; set; }
        public int RenderHeight { get; set; }

        public Frog()
        {
            Position = new Vector2();
            Speed = 100f;
            CurrentFrame = 0;
            TimePerFrame = 0.35; // Time per frame in seconds
            TotalElapsed = 0;
            IsFacingLeft = false;
            MovementDirection = "idle";

            RenderWidth = 64; // Default render width
            RenderHeight = 64; // Default render height
        }

        public void Initialize(int startX, int startY)
        {
            Position = new Vector2(startX, startY);
        }

        public void LoadContent(ContentManager content)
        {
            IdleAnimation = content.Load<Texture2D>("idle-animation");
            MovementSpriteSheet = content.Load<Texture2D>("movement-export");
            UpMovementSpriteSheet = content.Load<Texture2D>("up-sprite");
            DownMovementSpriteSheet = content.Load<Texture2D>("down-sprite");

            FrameWidth = MovementSpriteSheet.Width;
            FrameHeight = MovementSpriteSheet.Height / 2; // Adjust this based on your sprite sheet
            FrameCount = 2; // Number of frames in the sprite sheet
        }

        public void Update(GameTime gameTime, KeyboardState kstate, int v, int v1)
        {
            IsMoving = false;

            // Temporary new position for collision checking
            Vector2 newPosition = Position;

            // Handle left and right movement
            if (kstate.IsKeyDown(Keys.Left))
            {
                newPosition.X -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                IsMoving = true;
                IsFacingLeft = true;
                MovementDirection = "left";
                Debug.WriteLine("Moving Left");
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                newPosition.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                IsMoving = true;
                IsFacingLeft = false;
                MovementDirection = "right";
                Debug.WriteLine("Moving Right");
            }

            // Handle up and down movement
            if (kstate.IsKeyDown(Keys.Up))
            {
                newPosition.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                IsMoving = true;
                MovementDirection = "up";
                Debug.WriteLine("Moving Up");
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                newPosition.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                IsMoving = true;
                MovementDirection = "down";
                Debug.WriteLine("Moving Down");
            }

            // Animation frame update is handled separately
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            TotalElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (TotalElapsed > TimePerFrame)
            {
                CurrentFrame++;
                if (CurrentFrame >= FrameCount)
                {
                    CurrentFrame = 0;
                }
                TotalElapsed -= TimePerFrame;
                Debug.WriteLine($"Animation Frame: {CurrentFrame}");
            }
        }

        public void ResetAnimation()
        {
            MovementDirection = "idle";
            TotalElapsed += TimePerFrame;
            if (TotalElapsed > TimePerFrame)
            {
                CurrentFrame++;
                if (CurrentFrame >= FrameCount)
                {
                    CurrentFrame = 0;
                }
                TotalElapsed -= TimePerFrame;
                Debug.WriteLine($"Animation Frame: {CurrentFrame}");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Determine the sprite effects based on the facing direction
            var spriteEffects = IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Draw the frog
            Texture2D currentTexture = IdleAnimation;
            switch (MovementDirection)
            {
                case "left":
                case "right":
                    currentTexture = MovementSpriteSheet;
                    break;
                case "up":
                    currentTexture = UpMovementSpriteSheet;
                    break;
                case "down":
                    currentTexture = DownMovementSpriteSheet;
                    break;
                default:
                    currentTexture = IdleTexture;
                    break;
            }

            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, RenderWidth, RenderHeight);

            if (MovementDirection == "idle")
            {
                // Calculate source rectangle for current animation frame
                var sourceRectangle = new Rectangle(
                    0, // X position of the frame
                    CurrentFrame * FrameHeight, // Y position of the frame
                    FrameWidth,
                    FrameHeight
                );

                // Draw the current frame from sprite sheet
                spriteBatch.Draw(
                    IdleAnimation,
                    destinationRectangle,
                    sourceRectangle,
                    Color.White,
                    0f,
                    new Vector2(FrameWidth / 2, FrameHeight / 2),
                    spriteEffects,
                    0f
                );
            }
            else
            {
                // Calculate source rectangle for current animation frame
                var sourceRectangle = new Rectangle(
                    0, // X position of the frame
                    CurrentFrame * FrameHeight, // Y position of the frame
                    FrameWidth,
                    FrameHeight
                );

                // Draw the current frame from the sprite sheet
                spriteBatch.Draw(
                    currentTexture,
                    destinationRectangle,
                    sourceRectangle,
                    Color.White,
                    0f,
                    new Vector2(FrameWidth / 2, FrameHeight / 2),
                    spriteEffects,
                    0f
                );
            }
        }
    }
}
