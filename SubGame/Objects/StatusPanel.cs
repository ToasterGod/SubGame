using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Objects
{
    public class StatusPanel
    {
        private readonly Texture2D frameTexture;
        private readonly Texture2D textTexture;
        private readonly int margin = 20;

        private Rectangle frameArea;
        private Rectangle textArea;
        private SpriteFont font; // Font to be able to write text

        public int SubsHit { get; set; }
        public int BoatsHit { get; set; }

        public StatusPanel(Rectangle textArea, GraphicsDevice myDevice)
        {
            this.textArea = textArea;
            textTexture = new Texture2D(myDevice, 1, 1, false, SurfaceFormat.Color);
            textTexture.SetData(new[] { Color.White });

            frameArea = new Rectangle(textArea.X - 2, textArea.Y - 2, textArea.Width + 4, textArea.Height + 4);
            frameTexture = new Texture2D(myDevice, 1, 1, false, SurfaceFormat.Color);
            frameTexture.SetData(new[] { Color.Black });
        }

        public void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            // Load the selected font
            font = aContentManager.Load<SpriteFont>(anAssetName);
        }

        public void Draw(SpriteBatch mySpriteBatch)
        {
            mySpriteBatch.Draw(frameTexture, frameArea, Color.Black);
            mySpriteBatch.Draw(textTexture, textArea, Color.White);

            string labels = $"Subs sinked:\nBoat hits:";
            string values = $"{SubsHit,3}\n{BoatsHit,3}";

            mySpriteBatch.DrawString(font, labels, new Vector2(textArea.X + margin, textArea.Y + margin), Color.Blue);
            mySpriteBatch.DrawString(font, values, new Vector2(textArea.X + margin + 300, textArea.Y + margin), Color.Blue);
        }
    }
}