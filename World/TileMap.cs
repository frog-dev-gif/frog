using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FrogGame.World
{
    public class TileMap
    {
        public int[,] Map { get; private set; }
        public bool[,] CollisionMap { get; private set; }
        public Texture2D Tile0Texture { get; set; }
        public Texture2D Tile1Texture { get; set; }
        public Texture2D Tile2Texture { get; set; }
        public int TileSize { get; private set; }
        public int RenderSize { get; set; }

        public TileMap()
        {
            TileSize = 32;
            RenderSize = 32; // Default render size for tiles

            // Initialize the tile map and collision map
            GenerateMaps();
        }

        private void GenerateMaps()
        {
            Map = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 0, 1, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
                { 0, 1, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
                { 0, 1, 2, 2, 2, 1, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
                { 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
                { 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
                { 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            CollisionMap = new bool[,]
            {
                { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
                { true, true, false, false, false, true, false, false, true, false, false, false, false, false, false, false, false, false, true, true },
                { true, true, false, false, false, true, false, false, true, false, false, false, false, false, false, false, false, false, true, true },
                { true, true, false, false, false, true, true, false, true, false, false, false, false, false, false, false, false, false, true, true },
                { true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true },
                { true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true },
                { true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true },
                { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }
            };
        }

        public void LoadContent(ContentManager content)
        {
            Tile0Texture = content.Load<Texture2D>("tile0");
            Tile1Texture = content.Load<Texture2D>("tile1");
            Tile2Texture = content.Load<Texture2D>("tile2");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Map.GetLength(0); y++)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                {
                    Texture2D tileTexture = null;
                    switch (Map[y, x])
                    {
                        case 0:
                            tileTexture = Tile0Texture;
                            break;
                        case 1:
                            tileTexture = Tile1Texture;
                            break;
                        case 2:
                            tileTexture = Tile2Texture;
                            break;
                    }

                    if (tileTexture != null)
                    {
                        Rectangle destinationRectangle = new Rectangle(x * RenderSize, y * RenderSize, RenderSize, RenderSize);
                        spriteBatch.Draw(
                            tileTexture,
                            destinationRectangle,
                            Color.White
                        );
                    }
                }
            }
        }
    }
}
