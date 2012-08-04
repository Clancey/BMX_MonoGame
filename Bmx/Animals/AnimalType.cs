#region File Description
//-----------------------------------------------------------------------------
// Animal.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GameStateManagement
{
    public enum AnimalType
    {
        // no type
        Generic,
        // flies around and reacts
        Follower,
        // controled by waypoints, followers flee from it, attracted to followers
        Enemy,
		// Leader Followers follow the leader
		Leader,
    } 
}