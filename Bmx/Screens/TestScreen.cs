#region File Description
//-----------------------------------------------------------------------------
// Human.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameStateManagement;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision;
using FarseerPhysics.Controllers;
using FarseerPhysics.DebugViews;

#endregion

namespace GameStateManagement
{
	public class TestScreen : GameScreen
	{
       
		Random random;
		private List<Body> _bodies = new List<Body> ();
		private World _world;
		private const float Scale = 100f;
		GraphicsDevice GraphicsDevice;
		SpriteBatch spriteBatch;
		DebugViewXNA DebugView;
		Body slingShot;
		Ragdoll ragdoll;
		Vector2 startPos;
		public void LoadAssets ()
		{
			

			// Load texture
			LoadTextures();

			// Create new simulation world
			_world = new World (new Vector2 (0, 6f));
			_world.ContactManager.BeginContact = BeginContact;
			
			// Define the ground
			Constants.Initialize (ScreenManager);
			float simulatedHeight = Constants.FloorPosition.Y / Constants.Scale;
			float simulatedWidth = Constants.WorldWidth;
			startPos = new Vector2 (1.5f, simulatedHeight - 1.5f);

			//floor
			BodyFactory.CreateEdge (_world, new Vector2 (0.0f, simulatedHeight), new Vector2 (simulatedWidth* 2, simulatedHeight));

			ragdoll = new Ragdoll(_world,ScreenManager,startPos);
			
			Camera.Current.CenterPointTarget = 620f;
			//Camera.Current.StartTracking (_projectile.Body);
			Camera.Current.ScreenScale = ScreenManager.ScreenScale;
			
		}
		
		private void LoadTextures()
		{

			/*
			_box1 = Load<Texture2D> ("box1");
			_box2 = Load<Texture2D> ("box2");
			_ball = Load<Texture2D> ("ball");
			_floor = Load<Texture2D> ("floor");
			_pig = Load<Texture2D> ("pig");
			_smoke = Load<Texture2D> ("smoke");
			_triangleL = Load<Texture2D> ("triangleL");
			_triangleR = Load<Texture2D> ("triangleR");
			_slingShotBack = Load<Texture2D> ("Textures/Slingshot/BackLeft.png");
			_slingShotFront = Load<Texture2D> ("Textures/Slingshot/FrontLeft.png");
			*/
		}

		public TestScreen ()
		{
			EnabledGestures = 
                GestureType.Hold |
                GestureType.Tap | 
                GestureType.DoubleTap |
                GestureType.FreeDrag |
                GestureType.Flick |
                GestureType.Pinch;

			random = new Random ();
		}

		public override void LoadContent ()
		{ 
			LoadAssets ();

			base.LoadContent ();
			spriteBatch = ScreenManager.SpriteBatch;
			GraphicsDevice = ScreenManager.GraphicsDevice;
			// Start the game
			Start ();
			
			
            DebugView = new DebugViewXNA(_world);
            DebugView.LoadContent(GraphicsDevice,ScreenManager.Game.Content);
		}

		void Start ()
		{
			
		}
		
		public bool BeginContact (Contact contact)
		{
			Body bodyA = contact.FixtureA.Body;
			Body bodyB = contact.FixtureB.Body;

			// get the speed of impact between the two bodies
			Manifold worldManifold;
			contact.GetManifold (out worldManifold);
			ManifoldPoint p = worldManifold.Points [0];
			Vector2 vA = bodyA.GetLinearVelocityFromLocalPoint (p.LocalPoint);
			Vector2 vB = bodyB.GetLinearVelocityFromLocalPoint (p.LocalPoint);
			float approachVelocity = Math.Abs (Vector2.Dot (vB - vA, worldManifold.LocalNormal));

			//deduct hitpoints from both bodies
			ProcessContact (contact, bodyA, approachVelocity);
			ProcessContact (contact, bodyB, approachVelocity);

			return true;
		}

		private void ProcessContact (Contact contact, Body body, float approachVelocity)
		{
			
		
		}

		public override void Update (GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
         
			Camera.Current.Update (gameTime);

			_world.Step (Math.Min ((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
			
        
				
			base.Update (gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput (InputState input)
		{
			var accel = Accelerometer.GetState().Acceleration;
			Console.WriteLine(accel.X + " , " + accel.Y + " , " + accel.Z);

			var gravX = accel.Y * 6f;
			var gravY = accel.X * 6f;
			//_world.Gravity = new Vector2(gravX,gravY);
			foreach(var gesture in input.Gestures )
            {
                // read the next gesture from the queue

                // we can use the type of gesture to determine our behavior
                switch (gesture.GestureType)
                {


                    // on flicks, we want to update the selected sprite's velocity with
                    // the flick velocity, which is in pixels per second.
                   
                    // on pinches, we want to scale the selected sprite
                    case GestureType.Pinch:
                        if (Camera.Current != null)
                        {
                            // get the current and previous locations of the two fingers
                            Vector2 a = gesture.Position;
                            Vector2 aOld = gesture.Position - gesture.Delta;
                            Vector2 b = gesture.Position2;
                            Vector2 bOld = gesture.Position2 - gesture.Delta2;

                            // figure out the distance between the current and previous locations
                            float d = Vector2.Distance(a, b);
                            float dOld = Vector2.Distance(aOld, bOld);

                            // calculate the difference between the two and use that to alter the scale
                            float scaleChange = (d - dOld) * .01f;
                            Camera.Current.AddScale (scaleChange);
						//Console.WriteLine(Camera.Current.Scale);
                        }
                        break;
                }
            }
		}

		private void FinishCurrentGame ()
		{
			ExitScreen ();
		}

		private void PauseCurrentGame ()
		{
			// TODO: Display pause screen
		}

		public override void Draw (GameTime gameTime)
		{
			
			GraphicsDevice.Clear (Color.CornflowerBlue);
		
			spriteBatch.Begin (0, null, null, null, null, null, Camera.Current.TransformationMatrix);
			/*
			spriteBatch.Draw (_slingShotBack,
					 slingShot.Position * Constants.Scale, null, Color.White, slingShot.Rotation, new Vector2(.19f,1.2f) * Constants.Scale, 1f,
											   SpriteEffects.None, 0f);
			*/

			//Draw slingshot
			/*
			spriteBatch.Draw (_slingShotFront,
					 slingShot.Position * Constants.Scale, null, Color.White, slingShot.Rotation, new Vector2(.47f,1.3f) * Constants.Scale, 1f,
											   SpriteEffects.None, 0f);
			*/
			

			spriteBatch.End ();
			  // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width / Scale,
                                                             GraphicsDevice.Viewport.Height / Scale, 0f, 0f,
                                                             1f);
            
			var view = Camera.Current.DebugView;
			//Console.WriteLine(Camera.Current.CameraCenter/ Scale);
           
			DebugView.RenderDebugData(ref projection, ref view);
			
			base.Draw (gameTime);
			return;
		}

		private void DrawBackground ()
		{
			/*
            // Clear the background
            ScreenManager.Game.GraphicsDevice.Clear(Color.White);

            // Draw the Sky
            ScreenManager.SpriteBatch.Draw(skyTexture, Vector2.Zero, Color.White);

            // Draw Cloud #1
            ScreenManager.SpriteBatch.Draw(cloud1Texture,
                cloud1Position, Color.White);

            // Draw the Mountain
            ScreenManager.SpriteBatch.Draw(mountainTexture,
                Vector2.Zero, Color.White);

            // Draw Cloud #2
            ScreenManager.SpriteBatch.Draw(cloud2Texture,
                cloud2Position, Color.White);

            // Draw the Castle, trees, and foreground 
            ScreenManager.SpriteBatch.Draw(foregroundTexture,
                Vector2.Zero, Color.White);
                */
		}

		void DrawHud ()
		{
			
		}

		void DrawPlayer (GameTime gameTime)
		{
			
		}

		void DrawComputer (GameTime gameTime)
		{
			
		}

		// A simple helper to draw shadowed text.
		void DrawString (SpriteFont font, string text, Vector2 position, Color color)
		{
			ScreenManager.SpriteBatch.DrawString (font, text,
                new Vector2 (position.X + 1, position.Y + 1), Color.Black);
			ScreenManager.SpriteBatch.DrawString (font, text, position, color);
		}

		// A simple helper to draw shadowed text.
		void DrawString (SpriteFont font, string text, Vector2 position, Color color, float fontScale)
		{
			ScreenManager.SpriteBatch.DrawString (font, text,
                new Vector2 (position.X + 1, position.Y + 1),
                Color.Black, 0, new Vector2 (0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
			ScreenManager.SpriteBatch.DrawString (font, text, position,
                color, 0, new Vector2 (0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
		}

	}
}
