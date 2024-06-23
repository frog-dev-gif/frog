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

            // Initialize frog and tile map
            _frog = new Frog();
            _frog.Initialize(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _frog.RenderWidth = 32; // Set render width
            _frog.RenderHeight = 32; // Set render height

            _tileMap = new TileMap();

            // Initialize the camera
            int worldWidth = _tileMap.Map.GetLength(1) * _tileMap.TileSize;
            int worldHeight = _tileMap.Map.GetLength(0) * _tileMap.TileSize;
            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, worldWidth, worldHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content for frog and tile map
            _frog.LoadContent(Content);
            _tileMap.LoadContent(Content);

            Debug.WriteLine("Content loaded successfully");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            // Update frog position and animation
            _frog.Update(gameTime, kstate, _tileMap.Map.GetLength(1), _tileMap.Map.GetLength(0), _tileMap.CollisionMap, _tileMap.TileSize);

            // Update the camera position based on the frog's position
            _camera.Update(_frog.Position);

            base.Update(gameTime);
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
