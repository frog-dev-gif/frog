using System.Diagnostics;
using FrogGame.Entities;
using FrogGame.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrogGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Frog _frog;
        private TileMap _tileMap;
        private Camera _camera;
        private Color backgroundColor;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the desired resolution
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges(); // Apply the changes
        }

        protected override void Initialize()
        {
            // Initialize the custom background color
            backgroundColor = new Color(99, 155, 255); // #639bff in RGB

            // Initialize frog
            _frog = new Frog();
            _frog.Initialize(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _frog.RenderWidth = 32; // Set render width
            _frog.RenderHeight = 32; // Set render height

            // Initialize tile map
            _tileMap = new TileMap();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content for frog and tile map
            _frog.LoadContent(Content);
            _tileMap.LoadContent(Content);

            // Initialize the camera after the map is loaded
            int worldWidth = _tileMap.Map.GetLength(1) * _tileMap.TileSize;
            int worldHeight = _tileMap.Map.GetLength(0) * _tileMap.TileSize;
            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, worldWidth, worldHeight);

            Debug.WriteLine("Content loaded successfully");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            // Update frog position and animation
            UpdateFrog(gameTime, kstate);

            // Update the camera position based on the frog's position
            _camera.Update(_frog.Position);

            base.Update(gameTime);
        }

        private void UpdateFrog(GameTime gameTime, KeyboardState kstate)
        {
            // Handle movement logic here
            Vector2 newPosition = _frog.Position;
            bool isMoving = false;

            if (kstate.IsKeyDown(Keys.Left))
            {
                newPosition.X -= _frog.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                isMoving = true;
                _frog.IsFacingLeft = true;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                newPosition.X += _frog.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                isMoving = true;
                _frog.IsFacingLeft = false;
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                newPosition.Y -= _frog.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                isMoving = true;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                newPosition.Y += _frog.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                isMoving = true;
            }

            // Check for collisions
            if (!IsCollision(newPosition))
            {
                _frog.Position = newPosition;
            }

            // Update animation frame if moving
            if (isMoving)
            {
                _frog.UpdateAnimation(gameTime);
            }
            else
            {
                _frog.ResetAnimation();
            }
        }

        private bool IsCollision(Vector2 position)
        {
            // Calculate the bounding box of the frog
            Rectangle frogBounds = new Rectangle((int)position.X - _frog.RenderWidth / 2, (int)position.Y - _frog.RenderHeight / 2, _frog.RenderWidth, _frog.RenderHeight);

            // Check each corner of the frog's bounding box
            for (int x = frogBounds.Left; x <= frogBounds.Right; x += _tileMap.TileSize)
            {
                for (int y = frogBounds.Top; y <= frogBounds.Bottom; y += _tileMap.TileSize)
                {
                    int tileX = x / _tileMap.TileSize;
                    int tileY = y / _tileMap.TileSize;

                    if (tileX >= 0 && tileX < _tileMap.Map.GetLength(1) && tileY >= 0 && tileY < _tileMap.Map.GetLength(0))
                    {
                        if (_tileMap.CollisionMap[tileY, tileX])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor); // Use the custom background color

            _spriteBatch.Begin(transformMatrix: _camera.Transform);

            // Draw the tile map
            _tileMap.Draw(_spriteBatch);

            // Draw the frog
            _frog.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
