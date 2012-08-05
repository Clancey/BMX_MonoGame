//
//  Copyright 2012  James Clancey, Xamarin Inc  (http://www.xamarin.com)
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;

namespace BMX
{
	
	public class PhysicsGameScreen : GameScreen
	{
		public Camera2D Camera;
		protected DebugViewXNA DebugView;
		protected World World;
		
		private float _agentForce;
		private float _agentTorque;
		private FixedMouseJoint _fixedMouseJoint;
		private Body _userAgent;
		
		protected PhysicsGameScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(0.75);
			TransitionOffTime = TimeSpan.FromSeconds(0.75);
			//HasCursor = true;
			EnableCameraControl = true;
			_userAgent = null;
			World = null;
			Camera = null;
			DebugView = null;
		}
		
		public bool EnableCameraControl { get; set; }
		
		protected void SetUserAgent(Body agent, float force, float torque)
		{
			_userAgent = agent;
			_agentForce = force;
			_agentTorque = torque;
		}
		
		public override void LoadContent()
		{
			base.LoadContent();
			
			//We enable diagnostics to show get values for our performance counters.
			//Settings.EnableDiagnostics = true;
			
			if (World == null)
			{
				World = new World(Vector2.Zero);
			}
			else
			{
				World.Clear();
			}

			if (DebugView == null)
			{
				DebugView = new DebugViewXNA(World);
			//	DebugView.RemoveFlags(DebugViewFlags.Shape);
			//	DebugView.RemoveFlags(DebugViewFlags.Joint);
				DebugView.DefaultShapeColor = Color.White;
				DebugView.SleepingShapeColor = Color.LightGray;
				
				DebugView.LoadContent(ScreenManager.GraphicsDevice,ScreenManager.Game.Content);
			}
			
			if (Camera == null)
			{
				Camera = new Camera2D(ScreenManager.GraphicsDevice);
			}
			else
			{
				Camera.ResetCamera();
			}
			
			// Loading may take a while... so prevent the game from "catching up" once we finished loading
			ScreenManager.Game.ResetElapsedTime();
		}
		
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			if (!coveredByOtherScreen && !otherScreenHasFocus)
			{
				// variable time step but never less then 30 Hz
				World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
			}
			else
			{
				World.Step(0f);
			}
			Camera.Update(gameTime);
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}
		

		
		private void HandleCursor(InputHelper input)
		{
			Vector2 position = Camera.ConvertScreenToWorld(input.Cursor);
			
			if ((input.IsNewButtonPress(Buttons.A) ||
			     input.IsNewMouseButtonPress(MouseButtons.LeftButton)) &&
			    _fixedMouseJoint == null)
			{
				Fixture savedFixture = World.TestPoint(position);
				if (savedFixture != null)
				{
					Body body = savedFixture.Body;
					_fixedMouseJoint = new FixedMouseJoint(body, position);
					_fixedMouseJoint.MaxForce = 1000.0f * body.Mass;
					World.AddJoint(_fixedMouseJoint);
					body.Awake = true;
				}
			}
			
			if ((input.IsNewButtonRelease(Buttons.A) ||
			     input.IsNewMouseButtonRelease(MouseButtons.LeftButton)) &&
			    _fixedMouseJoint != null)
			{
				World.RemoveJoint(_fixedMouseJoint);
				_fixedMouseJoint = null;
			}
			
			if (_fixedMouseJoint != null)
			{
				_fixedMouseJoint.WorldAnchorB = position;
			}
		}
		
		private void HandleCamera(InputHelper input, GameTime gameTime)
		{
			Vector2 camMove = Vector2.Zero;
			
			if (input.KeyboardState.IsKeyDown(Keys.Up))
			{
				camMove.Y -= 10f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (input.KeyboardState.IsKeyDown(Keys.Down))
			{
				camMove.Y += 10f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (input.KeyboardState.IsKeyDown(Keys.Left))
			{
				camMove.X -= 10f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (input.KeyboardState.IsKeyDown(Keys.Right))
			{
				camMove.X += 10f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			if (input.KeyboardState.IsKeyDown(Keys.PageUp))
			{
				Camera.Zoom += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds * Camera.Zoom / 20f;
			}
			if (input.KeyboardState.IsKeyDown(Keys.PageDown))
			{
				Camera.Zoom -= 5f * (float)gameTime.ElapsedGameTime.TotalSeconds * Camera.Zoom / 20f;
			}
			if (camMove != Vector2.Zero)
			{
				Camera.MoveCamera(camMove);
			}
			if (input.IsNewKeyPress(Keys.Home))
			{
				Camera.ResetCamera();
			}
		}
		
		private void HandleUserAgent(InputHelper input)
		{
			Vector2 force = _agentForce * new Vector2(input.GamePadState.ThumbSticks.Right.X,
			                                          -input.GamePadState.ThumbSticks.Right.Y);
			float torque = _agentTorque * (input.GamePadState.Triggers.Right - input.GamePadState.Triggers.Left);
			
			_userAgent.ApplyForce(force);
			_userAgent.ApplyTorque(torque);
			
			float forceAmount = _agentForce * 0.6f;
			
			force = Vector2.Zero;
			torque = 0;
			
			if (input.KeyboardState.IsKeyDown(Keys.A))
			{
				force += new Vector2(-forceAmount, 0);
			}
			if (input.KeyboardState.IsKeyDown(Keys.S))
			{
				force += new Vector2(0, forceAmount);
			}
			if (input.KeyboardState.IsKeyDown(Keys.D))
			{
				force += new Vector2(forceAmount, 0);
			}
			if (input.KeyboardState.IsKeyDown(Keys.W))
			{
				force += new Vector2(0, -forceAmount);
			}
			if (input.KeyboardState.IsKeyDown(Keys.Q))
			{
				torque -= _agentTorque;
			}
			if (input.KeyboardState.IsKeyDown(Keys.E))
			{
				torque += _agentTorque;
			}
			
			_userAgent.ApplyForce(force);
			_userAgent.ApplyTorque(torque);
		}

		public override void Draw(GameTime gameTime)
		{
			Matrix projection = Camera.SimProjection;
			Matrix view = Camera.SimView;
			
			DebugView.RenderDebugData(ref projection, ref view);
			base.Draw(gameTime);
		}
	}
}