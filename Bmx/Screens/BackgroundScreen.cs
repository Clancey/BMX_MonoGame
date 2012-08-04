#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
		 #region Constants

        // X location to start drawing the HUD from        
        const int hudLocX = 200;

        // Y location to start drawing the HUD from
        const int hudLocY = 30;

        // Min value for the distance sliders
        const float sliderMin = 0.0f;

        // Max value for the distance sliders        
        const float sliderMax = 100f;

        // Width of the slider button
        const int sliderButtonWidth = 10;

        // Default value for the AI parameters
        const float detectionDefault = 70.0f;
        const float separationDefault = 50.0f;
        const float moveInOldDirInfluenceDefault = 1.0f;
        const float moveInFlockDirInfluenceDefault = 1.0f;
        const float moveInRandomDirInfluenceDefault = 0.05f;
        const float maxTurnRadiansDefault = 6.0f;
        const float perMemberWeightDefault = 1.0f;
        const float perDangerWeightDefault = 50.0f;
        #endregion

        #region Fields

        ContentManager content;
		SpriteBatch spriteBatch;

		Texture2D backgroundTexture;

		Texture2D onePixelWhite;

        Texture2D birdTexture;

        Flock flock;

        AIParameters flockParams;

		// Do we need to update AI parameers this Update
        bool aiParameterUpdate = false;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

			flock = null;

            flockParams = new AIParameters();
            ResetAIParams();
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("background");

			birdTexture = content.Load<Texture2D>("mouse");

			onePixelWhite = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            onePixelWhite.SetData<Color>(new Color[] { Color.White });

			spriteBatch = ScreenManager.SpriteBatch;
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

		/// <summary>
        /// Create the bird flock
        /// </summary>
        /// <param name="theNum"></param>
        protected void SpawnFlock()
        {
            if (flock == null)
            {
                flock = new Flock(birdTexture, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Width, 
                                  ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Height, flockParams);
            }
        }


		/// <summary>
        /// Reset flock AI parameters
        /// </summary>
        private void ResetAIParams()
        {
            flockParams.DetectionDistance = detectionDefault;
            flockParams.SeparationDistance = separationDefault;
            flockParams.MoveInOldDirectionInfluence = moveInOldDirInfluenceDefault;
            flockParams.MoveInFlockDirectionInfluence = moveInFlockDirInfluenceDefault;
            flockParams.MoveInRandomDirectionInfluence = moveInRandomDirInfluenceDefault;
            flockParams.MaxTurnRadians = maxTurnRadiansDefault;
            flockParams.PerMemberWeight = perMemberWeightDefault;
            flockParams.PerDangerWeight = perDangerWeightDefault;
        }
        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
			if (flock != null)
            {
                flock.Update(gameTime);
            }
            else
            {
                SpawnFlock();
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

			if (flock != null)
            {
                flock.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }


        #endregion
    }
}
