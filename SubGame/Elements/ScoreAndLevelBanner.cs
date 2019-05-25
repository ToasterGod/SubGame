using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubGame.Elements
{
    public class ScoreAndLevelBanner: Element
    {
        private readonly int myMargin = 60;

        private Rectangle myTextArea;
        private readonly Texture2D myTextTexture;
        //private Rectangle myFrameArea;
        private readonly Texture2D myFrameTexture;
        private Texture2D myPanel;
        private SpriteFont myFont; // Font to be able to write text

        public ScoreAndLevelBanner(Vector2 aPosition, Vector2 aSize, GraphicsDeviceManager aManager)
            : base(aPosition)
        {
            myTextArea = new Rectangle(aPosition.ToPoint(), aSize.ToPoint());
            myTextTexture = new Texture2D(aManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            myTextTexture.SetData(new[] { Color.Red });

            //myFrameArea = new Rectangle(myTextArea.X - 2, myTextArea.Y - 2, myTextArea.Width + 4, myTextArea.Height + 4);
            myFrameTexture = new Texture2D(aManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            myFrameTexture.SetData(new[] { Color.Black });
        }

        public virtual void LoadContent(ContentManager aContentManager, string aFontName, string anAssetName)
        {
            myPanel = aContentManager.Load<Texture2D>(anAssetName);
            myFont = aContentManager.Load<SpriteFont>(aFontName);
        }

        public void Draw(SpriteBatch aSpriteBatch, string aText)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, myPanel.Width, myPanel.Height);
            Vector2 myOrigin = new Vector2(0, 0);
            aSpriteBatch.Draw(myPanel, AccessPosition, mySourceRectangle, Color.White, 0.0f, myOrigin, 1.0f, SpriteEffects.None, 1);
            //aSpriteBatch.Draw(myFrameTexture, myFrameArea, Color.Black);
            //aSpriteBatch.Draw(myTextTexture, myTextArea, Color.Red);
            aSpriteBatch.DrawString(myFont, aText, new Vector2(myTextArea.X + myMargin, myTextArea.Y + myMargin), Color.Yellow);
        }
    }
}
