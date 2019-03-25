using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Objects
{
    // Not used right now, only a conceptual example of how to animate a sprite
    internal class AnimatedSprite
    {
        public Texture2D AccessTexture { get; set; }
        public int AccessRows { get; set; }
        public int AccessColumns { get; set; }
        private int myCurrentFrame;
        private int myTotalFrames;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            AccessTexture = texture;
            AccessRows = rows;
            AccessColumns = columns;
            myCurrentFrame = 0;
            myTotalFrames = AccessRows * AccessColumns;
        }

        public void Update()
        {
            myCurrentFrame++;
            if (myCurrentFrame == myTotalFrames)
                myCurrentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int tempWidth = AccessTexture.Width / AccessColumns;
            int tempHeight = AccessTexture.Height / AccessRows;
            int tempRow = (int)((float)myCurrentFrame / (float)AccessColumns);
            int tempColumn = myCurrentFrame % AccessColumns;

            Rectangle tempSourceRectangle = new Rectangle(tempWidth * tempColumn, tempHeight * tempRow, tempWidth, tempHeight);
            Rectangle tempDestinationRectangle = new Rectangle((int)location.X, (int)location.Y, tempWidth, tempHeight);

            spriteBatch.Begin();
            spriteBatch.Draw(AccessTexture, tempDestinationRectangle, tempSourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}