using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Objects
{
    public class StatusPanel
    {
        private readonly Texture2D myFrameTexture;
        private readonly Texture2D myTextTexture;
        private readonly int margin = 20;

        private Rectangle myFrameArea;
        private Rectangle myTextArea;
        private SpriteFont myFont; // Font to be able to write text

        public int AccessSubsHit { get; set; }
        public int AccessBoatsHit { get; set; }

        public StatusPanel(Rectangle aTextArea, GraphicsDevice aGraphicsDevice)
        {
            this.myTextArea = aTextArea;
            myTextTexture = new Texture2D(aGraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            myTextTexture.SetData(new[] { Color.White });

            myFrameArea = new Rectangle(aTextArea.X - 2, aTextArea.Y - 2, aTextArea.Width + 4, aTextArea.Height + 4);
            myFrameTexture = new Texture2D(aGraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            myFrameTexture.SetData(new[] { Color.Black });
        }

        public void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            // Load the selected font
            myFont = aContentManager.Load<SpriteFont>(anAssetName);
        }

        public void Draw(SpriteBatch mySpriteBatch)
        {
            mySpriteBatch.Draw(myFrameTexture, myFrameArea, Color.Black);
            mySpriteBatch.Draw(myTextTexture, myTextArea, Color.White);

            string tempLabels = $"Subs sinked:\nBoat hits:";
            string tempValues = $"{AccessSubsHit,3}\n{AccessBoatsHit,3}";

            mySpriteBatch.DrawString(myFont, tempLabels, new Vector2(myTextArea.X + margin, myTextArea.Y + margin), Color.Blue);
            mySpriteBatch.DrawString(myFont, tempValues, new Vector2(myTextArea.X + margin + 300, myTextArea.Y + margin), Color.Blue);
        }
    }
}