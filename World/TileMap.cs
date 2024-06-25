using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using System.Collections.Generic;

namespace FrogGame.World
{
    public class TileMap
    {
        public int[,] Map { get; private set; }
        public bool[,] CollisionMap { get; private set; }
        public Dictionary<int, Texture2D> TileTextures { get; private set; }
        public int TileSize { get; private set; }

        public TileMap()
        {
            TileSize = 32;
            TileTextures = new Dictionary<int, Texture2D>();
        }

        public void LoadContent(ContentManager content)
        {
            // Load and parse the TMX map
            var map = new TmxMap("Content/test-map/Level_0.tmx");

            // Load tilesets
            foreach (var tileset in map.Tilesets)
            {
                string tilesetPath = Path.Combine("Content/test-map", Path.GetFileNameWithoutExtension(tileset.Image.Source));
                TileTextures[tileset.FirstGid] = content.Load<Texture2D>(tilesetPath);
            }

            int width = map.Width;
            int height = map.Height;
            Map = new int[height, width];
            CollisionMap = new bool[height, width];

            var layer = map.Layers.First(l => l.Name == "IntGrid");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = layer.Tiles[y * width + x].Gid;
                    Map[y, x] = value;
                    CollisionMap[y, x] = (value == 1); // Assuming 1 is the collidable tile
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Map.GetLength(0); y++)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                {
                    int tileGid = Map[y, x];
                    if (tileGid == 0) continue;

                    // Find the correct texture for the tile Gid
                    int textureGid = TileTextures.Keys.Where(gid => gid <= tileGid).Max();
                    Texture2D tileTexture = TileTextures[textureGid];
                    
                    int localTileId = tileGid - textureGid;
                    int tilesPerRow = tileTexture.Width / TileSize;
                    int tileX = (localTileId % tilesPerRow) * TileSize;
                    int tileY = (localTileId / tilesPerRow) * TileSize;

                    spriteBatch.Draw(
                        tileTexture,
                        new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize),
                        new Rectangle(tileX, tileY, TileSize, TileSize),
                        Color.White
                    );
                }
            }
        }
    }
}
