using System;
using System.IO;

using FrogGame;
using FrogGame.Entities;
using FrogGame.World;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Toad
{
    public class Game1 : Game
    {
        private static void Log(string message)
        {
            File.AppendAllText("game_debug.txt", message + "\n");
            System.Console.WriteLine(message);
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TileMap _tileMap;
        private Frog _frog;
        private Camera _camera;
        private Color backgroundColor;
        private const int GRID_SIZE = 32; // This should match your tile size
        private Song _backgroundMusic;
        private Matrix _scaleMatrix;


        public Game1()
        {
            Log("Game1 constructor called");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 672;
            _graphics.PreferredBackBufferHeight = 672;
            _graphics.ApplyChanges();
            Log("Game1 constructor completed");
        }

        protected override void Initialize()
        {
            backgroundColor = new Color(99, 155, 255);

            _tileMap = new TileMap();
            _tileMap.LoadContent(Content, GraphicsDevice);

            // Find a non-colliding tile to spawn the frog
            Vector2 spawnPosition = FindNonCollidingTile();
            _frog = new Frog();
            _frog.Initialize((int)spawnPosition.X, (int)spawnPosition.Y); // Cast to int here
            _frog.RenderWidth = 32;
            _frog.RenderHeight = 32;

            base.Initialize();
        }

        private Vector2 FindNonCollidingTile()
        {
            for (int y = 0; y < _tileMap.CollisionMap.GetLength(0); y++)
            {
                for (int x = 0; x < _tileMap.CollisionMap.GetLength(1); x++)
                {
                    if (!_tileMap.CollisionMap[y, x])
                    {
                        return new Vector2(x * _tileMap.TileSize + _tileMap.TileSize / 2,
                                           y * _tileMap.TileSize + _tileMap.TileSize / 2);
                    }
                }
            }
            throw new Exception("No non-colliding tile found for frog spawn!");
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load other content
            _frog.LoadContent(Content);
            _tileMap.LoadContent(Content, GraphicsDevice);

            // Calculate the scaling matrix
            float scaleX = (float)_graphics.PreferredBackBufferWidth / (32 * _tileMap.CollisionMap.GetLength(1));
            float scaleY = (float)_graphics.PreferredBackBufferHeight / (32 * _tileMap.CollisionMap.GetLength(0));
            _scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            // Initialize the camera after the map is loaded
            int worldWidth = _tileMap.CollisionMap.GetLength(1) * _tileMap.TileSize;
            int worldHeight = _tileMap.CollisionMap.GetLength(0) * _tileMap.TileSize;
            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, worldWidth, worldHeight);

            _backgroundMusic = Content.Load<Song>(@"Music\SWAPPARTY");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
        }
        protected override void UnloadContent()
        {
            MediaPlayer.Stop();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            UpdateFrog(gameTime, kstate);
            _camera.Update(_frog.Position);
            _tileMap.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateFrog(GameTime gameTime, KeyboardState kstate)
        {
            if (!_frog.IsMoving)
            {
                Vector2 newTargetPosition = _frog.Position;
                bool wantsToMove = false;

                if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
                {
                    newTargetPosition.X -= GRID_SIZE;
                    _frog.MovementDirection = "left";
                    wantsToMove = true;
                    _frog.IsFacingLeft = true;
                }
                else if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
                {
                    newTargetPosition.X += GRID_SIZE;
                    _frog.MovementDirection = "right";
                    wantsToMove = true;
                    _frog.IsFacingLeft = false;
                }
                else if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
                {
                    newTargetPosition.Y -= GRID_SIZE;
                    _frog.MovementDirection = "up";
                    wantsToMove = true;
                }
                else if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
                {
                    newTargetPosition.Y += GRID_SIZE;
                    _frog.MovementDirection = "down";
                    wantsToMove = true;
                }

                if (wantsToMove && !IsCollision(newTargetPosition))
                {
                    _frog.TargetPosition = newTargetPosition;
                    _frog.IsMoving = true;
                }
            }

            if (_frog.IsMoving)
            {
                Vector2 movement = _frog.TargetPosition - _frog.Position;
                if (movement.Length() > _frog.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds)
                {
                    movement.Normalize();
                    _frog.Position += movement * _frog.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _frog.Position = _frog.TargetPosition;
                    _frog.IsMoving = false;
                    _frog.MovementDirection = "idle";
                }
            }

            _frog.UpdateAnimation(gameTime);
        }
        private bool IsCollision(Vector2 position)
        {
            if (_tileMap == null || _tileMap.CollisionMap == null)
            {
                Log("TileMap or CollisionMap is null in IsCollision method");
                return true; // Prevent movement if there's no collision map
            }

            int tileX = (int)Math.Floor(position.X / GRID_SIZE);
            int tileY = (int)Math.Floor(position.Y / GRID_SIZE);

            // Check if the position is within the map bounds
            if (tileX < 0 || tileX >= _tileMap.CollisionMap.GetLength(1) ||
                tileY < 0 || tileY >= _tileMap.CollisionMap.GetLength(0))
            {
                return true; // Collision with map boundaries
            }

            return _tileMap.CollisionMap[tileY, tileX];
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            Matrix transformMatrix = _scaleMatrix * _camera.Transform;

            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _tileMap.Draw(GraphicsDevice, transformMatrix);  // Pass the GraphicsDevice and transform matrix to the TileMap's Draw method
            _frog.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }



    }
}
