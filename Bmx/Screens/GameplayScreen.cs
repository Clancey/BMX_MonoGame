#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using FarseerPhysics.Dynamics;
using FarseerPhysics.DebugViews;
using BMX;
using System.Collections.Generic;
using FarseerPhysics.Dynamics.Joints;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;


#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GameStateManagement
{
	internal class GameplayScreen : PhysicsGameScreen
	{  

		private Sprite _obstacle;
		private Ragdoll _ragdoll;
		Border _border;
		
		#region IDemoScreen Members
		
		public string GetTitle()
		{
			return "Ragdoll";
		}
	
		
#endregion
		
		public override void LoadContent()
		{
			base.LoadContent();
			
			World.Gravity = new Vector2(0f, 20f);
			
			_border = new Border(World, this, ScreenManager.GraphicsDevice.Viewport);
			
			_ragdoll = new Ragdoll(World, ScreenManager, Vector2.Zero);
			LoadObstacles();
			
			SetUserAgent(_ragdoll.Body, 1000f, 400f);
		}
		
		private void LoadObstacles()
		{

		}
		
		public override void Draw(GameTime gameTime)
		{
			ScreenManager.GraphicsDevice.Clear (Color.CornflowerBlue);
			ScreenManager.SpriteBatch.Begin(0, null, null, null, null, null, Camera.View);
			_ragdoll.Draw();

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}