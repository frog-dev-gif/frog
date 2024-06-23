using Microsoft.Xna.Framework;

namespace FrogGame
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;

        public Camera(int viewportWidth, int viewportHeight, int worldWidth, int worldHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        public void Update(Vector2 playerPosition)
        {
            // Center the camera on the player
            Vector2 newPosition = playerPosition - new Vector2(_viewportWidth / 2, _viewportHeight / 2);

            // Clamp the camera position to the bounds of the world
            newPosition.X = MathHelper.Clamp(newPosition.X, 0, _worldWidth - _viewportWidth);
            newPosition.Y = MathHelper.Clamp(newPosition.Y, 0, _worldHeight - _viewportHeight);

            // Update the position and transform matrix
            Position = newPosition;
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
