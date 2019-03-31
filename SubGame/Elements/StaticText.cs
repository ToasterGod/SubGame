using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class StaticText : Element
    {
        private readonly int margin = 20;

        private Rectangle textArea;
        private Texture2D textTexture;
        private Rectangle frameArea;
        private Texture2D frameTexture;

        private SpriteFont font; // Font to be able to write text

        public StaticText(Vector2 position, Vector2 size, GraphicsDeviceManager manager)
            : base(position)
        {
            textArea = new Rectangle(position.ToPoint(), size.ToPoint());
            textTexture = new Texture2D(manager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            textTexture.SetData(new[] { Color.White });

            frameArea = new Rectangle(textArea.X - 2, textArea.Y - 2, textArea.Width + 4, textArea.Height + 4);
            frameTexture = new Texture2D(manager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            frameTexture.SetData(new[] { Color.Black });
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            font = aContentManager.Load<SpriteFont>(anAssetName);
        }

        public void Draw(SpriteBatch spriteBatch, string text)
        {
            spriteBatch.Draw(frameTexture, frameArea, Color.Black);
            spriteBatch.Draw(textTexture, textArea, Color.White);
            spriteBatch.DrawString(font, text, new Vector2(textArea.X + margin, textArea.Y + margin), Color.Blue);
        }
    }
}