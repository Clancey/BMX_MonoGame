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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace GameStateManagement
{
	public static class Constants
	{
		public const float Scale = 20f;
		public static float HalfScreenWidth { get; private set; }
		public static float HalfScreenHeight { get; private set; }
		public static float ScreenWidth { get; private set; }
		public static float ScreenHeight { get; private set; }
		public static float WorldWidth {get;private set;}
		public static Vector2 FloorPosition { get; private set;}
		public static Vector2 FloorSize {get;private set;}
		public static Vector2 ScreenCenter{get;private set;}
	
		public static void Initialize(ScreenManager screenManager)
		{
			var width = screenManager.GraphicsDevice.Viewport.Width;
			var height = screenManager.GraphicsDevice.Viewport.Height;
			HalfScreenWidth = ScreenWidth / 2f;
			HalfScreenHeight = ScreenHeight / 2f;
			ScreenCenter = new Vector2(HalfScreenWidth,HalfScreenHeight);
			FloorPosition = new Vector2(0, ScreenHeight * 0.875f); // = 7/8
			FloorSize = new Vector2(ScreenWidth,ScreenHeight - FloorPosition.Y);
			WorldWidth =  (ScreenWidth / Scale) * 2f;
		}
	}
}

