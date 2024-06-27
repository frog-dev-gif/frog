using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace FrogGame.World
{
    public class TileMap
    {
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        public bool[,] CollisionMap { get; private set; }
        public int TileSize { get; private set; }

        public TileMap()
        {
            TileSize = 32;
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _tiledMap = content.Load<TiledMap>("test-map/test-map-0");
            _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);

            int mapWidth = _tiledMap.Width;
            int mapHeight = _tiledMap.Height;
            CollisionMap = new bool[mapHeight, mapWidth];

            var layer = _tiledMap.TileLayers[0];  // Assuming the first layer is your collision layer

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Assume tile ID 3 is collidable (adjust this based on your tileset)
                    CollisionMap[y, x] = layer.GetTile((ushort)x, (ushort)y).GlobalIdentifier == 3;
                }
            }

            // Print collision map for debugging
            Console.WriteLine("Collision Map:");
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Console.Write(CollisionMap[y, x] ? "X" : ".");
                }
                Console.WriteLine();
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix)
        {
            var previousSamplerState = graphicsDevice.SamplerStates[0];
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            _tiledMapRenderer.Draw(viewMatrix);

            graphicsDevice.SamplerStates[0] = previousSamplerState;
        }



        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }
    }
}
