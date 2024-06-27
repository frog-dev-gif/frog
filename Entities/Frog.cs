using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        private const float ANIMATION_FPS = 4f;
        private float _animationTimer = 0f;
        public Vector2 TargetPosition { get; set; }

        public Frog()
        {
            Position = new Vector2();
            Speed = 128f; // Changed to 128f for grid-based movement
            CurrentFrame = 0;
            TimePerFrame = 0.35;
            TotalElapsed = 0;
            IsFacingLeft = false;
            MovementDirection = "idle";

            RenderWidth = 32;
            RenderHeight = 32;
            TargetPosition = Position;
        }

        public void Initialize(int startX, int startY)
        {
            Position = new Vector2(startX, startY);
        }

        public void LoadContent(ContentManager content)
        {
            try
            {
                IdleAnimation = content.Load<Texture2D>("idle-animation");
                MovementSpriteSheet = content.Load<Texture2D>("movement-export");
                UpMovementSpriteSheet = content.Load<Texture2D>("up-sprite");
                DownMovementSpriteSheet = content.Load<Texture2D>("down-sprite");
                IdleTexture = content.Load<Texture2D>("idle-animation"); // Add this line if it's missing
                Console.WriteLine("Successfully loaded textures:");
                Console.WriteLine($"IdleAnimation: {IdleAnimation != null}");
                Console.WriteLine($"MovementSpriteSheet: {MovementSpriteSheet != null}");
                Console.WriteLine($"UpMovementSpriteSheet: {UpMovementSpriteSheet != null}");
                Console.WriteLine($"DownMovementSpriteSheet: {DownMovementSpriteSheet != null}");
                Console.WriteLine($"IdleTexture: {IdleTexture != null}");

                if (IdleAnimation == null || MovementSpriteSheet == null ||
                    UpMovementSpriteSheet == null || DownMovementSpriteSheet == null ||
                    IdleTexture == null)
                {
                    throw new ContentLoadException("One or more textures failed to load.");
                }

                FrameWidth = MovementSpriteSheet.Width;
                FrameHeight = MovementSpriteSheet.Height / 2;
                FrameCount = 2;
            }
            catch (ContentLoadException e)
            {
                Console.WriteLine($"Error loading content: {e.Message}");
                // You might want to throw this exception again if you want to stop the game from running
            }
        }
        public void UpdateAnimation(GameTime gameTime)
        {
            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_animationTimer >= 1f / ANIMATION_FPS)
            {
                CurrentFrame = (CurrentFrame + 1) % FrameCount;
                _animationTimer = 0f;
            }
        }

        public void ResetAnimation()
        {
            MovementDirection = "idle";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture;
            Rectangle sourceRectangle;

            switch (MovementDirection)
            {
                case "up":
                    currentTexture = UpMovementSpriteSheet;
                    break;
                case "down":
                    currentTexture = DownMovementSpriteSheet;
                    break;
                case "left":
                case "right":
                    currentTexture = MovementSpriteSheet;
                    break;
                default: // "idle" or any other case
                    currentTexture = IdleAnimation;
                    break;
            }

            if (currentTexture == null)
            {
                Console.WriteLine($"Texture for {MovementDirection} movement is null.");
                return;
            }

            // Calculate the source rectangle based on the current frame
            sourceRectangle = new Rectangle(
                0, // X position of the frame
                CurrentFrame * FrameHeight, // Y position of the frame
                FrameWidth,
                FrameHeight
            );

            Rectangle destinationRectangle = new Rectangle(
                (int)Position.X - RenderWidth / 2,
                (int)Position.Y - RenderHeight / 2,
                RenderWidth,
                RenderHeight
            );

            SpriteEffects spriteEffects = IsFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(
                currentTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                spriteEffects,
                0f
            );
        }
    }
}

