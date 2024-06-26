using System.IO;

using FrogGame;
using FrogGame.Entities;
using FrogGame.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Toad
{
    public class Game1 : Game
    {
        private static void Log(string message)
        {
            File.AppendAllText("game_log.txt", message + "\n");
            System.Console.WriteLine(message);
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TileMap _tileMap;
        private Frog _frog;
        private Camera _camera;
        private Color backgroundColor;

        public Game1()
        {
            Log("Game1 constructor called");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();

            Log("Game1 constructor done");
        }

        protected override void Initialize()
        {
            Log("Initialize called");
            backgroundColor = new Color(99, 155, 255);

            _frog = new Frog();
            _frog.Initialize(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _frog.RenderWidth = 32;
            _frog.RenderHeight = 32;

            _tileMap = new TileMap();

            base.Initialize();
            Log("Initialize done");
        }

protected override void LoadContent()
{
    Log("LoadContent called");
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    _frog.LoadContent(Content);
    
    _tileMap = new TileMap();
    _tileMap.LoadContent(Content, GraphicsDevice);

    // Initialize the camera after the map is loaded
    int worldWidth = _tileMap.CollisionMap.GetLength(1) * _tileMap.TileSize;
    int worldHeight = _tileMap.CollisionMap.GetLength(0) * _tileMap.TileSize;
    _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, worldWidth, worldHeight);

    Log("LoadContent done");
}

        protected override void Update(GameTime gameTime)
        {
            Log("Update called");
                Log("Update called");
    if (_tileMap == null)
    {
        Log("_tileMap is null in Update method");
    }
    if (_tileMap != null && _tileMap.CollisionMap == null)
    {
        Log("_tileMap.CollisionMap is null in Update method");
    }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            UpdateFrog(gameTime, kstate);
            _camera.Update(_frog.Position);
            _tileMap.Update(gameTime);

            base.Update(gameTime);
            Log("Update done");
        }

        private void UpdateFrog(GameTime gameTime, KeyboardState kstate)
        {
            Log("UpdateFrog called");
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

            if (!IsCollision(newPosition))
            {
                _frog.Position = newPosition;
            }

            if (isMoving)
            {
                _frog.UpdateAnimation(gameTime);
            }
            else
            {
                _frog.ResetAnimation();
            }
            Log("UpdateFrog done");
        }

        private bool IsCollision(Vector2 position)
{
    if (_tileMap == null || _tileMap.CollisionMap == null)
    {
        Log("TileMap or CollisionMap is null in IsCollision method");
        return false; // Or handle this case as appropriate for your game
    }

    Rectangle frogBounds = new Rectangle((int)position.X - _frog.RenderWidth / 2, (int)position.Y - _frog.RenderHeight / 2, _frog.RenderWidth, _frog.RenderHeight);

    for (int x = frogBounds.Left; x <= frogBounds.Right; x += _tileMap.TileSize)
    {
        for (int y = frogBounds.Top; y <= frogBounds.Bottom; y += _tileMap.TileSize)
        {
            int tileX = x / _tileMap.TileSize;
            int tileY = y / _tileMap.TileSize;

            if (tileX >= 0 && tileX < _tileMap.CollisionMap.GetLength(1) && 
                tileY >= 0 && tileY < _tileMap.CollisionMap.GetLength(0))
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
            Log("Draw called");
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);

            _tileMap.Draw(_camera.Transform);
            _frog.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
            Log("Draw done");
        }
    }
}