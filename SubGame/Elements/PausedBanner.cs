﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    public class PausedBanner : Element
    {
        private readonly int myMargin = 20;

        private Rectangle myTextArea;
        private readonly Texture2D myTextTexture;
        private Rectangle myFrameArea;
        private readonly Texture2D myFrameTexture;

        private SpriteFont myFont; // Font to be able to write text

        public PausedBanner(Vector2 aPosition, Vector2 aSize, GraphicsDeviceManager aManager)
            : base(aPosition)
        {
            myTextArea = new Rectangle(aPosition.ToPoint(), aSize.ToPoint());
            myTextTexture = new Texture2D(aManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            myTextTexture.SetData(new[] { Color.Red });

            myFrameArea = new Rectangle(myTextArea.X - 2, myTextArea.Y - 2, myTextArea.Width + 4, myTextArea.Height + 4);
            myFrameTexture = new Texture2D(aManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            myFrameTexture.SetData(new[] { Color.Black });
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAssetName)
            => myFont = aContentManager.Load<SpriteFont>(anAssetName);

        public void Draw(SpriteBatch aSpriteBatch, string aText)
        {
            aSpriteBatch.Draw(myFrameTexture, myFrameArea, Color.Black);
            aSpriteBatch.Draw(myTextTexture, myTextArea, Color.Red);
            aSpriteBatch.DrawString(myFont, aText, new Vector2(myTextArea.X + myMargin, myTextArea.Y + myMargin), Color.Yellow);
        }
    }
}
