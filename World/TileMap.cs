using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using System;
using System.Collections.Generic;
using System.IO;

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
        _tiledMap = content.Load<TiledMap>("test-map/test-map");
        _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);

        // Initialize CollisionMap
        int mapWidth = _tiledMap.Width;
        int mapHeight = _tiledMap.Height;
        CollisionMap = new bool[mapHeight, mapWidth];

        // Assuming the first tile layer is your collision layer
        var collisionLayer = _tiledMap.TileLayers[0];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Assume any non-zero tile is collidable
                CollisionMap[y, x] = collisionLayer.GetTile((ushort)x, (ushort)y).GlobalIdentifier != 0;
            }
        }
    }
        public void Draw(Matrix viewMatrix)
        {
            _tiledMapRenderer.Draw(viewMatrix);
        }

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }
    }
}