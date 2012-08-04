
#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace GameStateManagement
{
	public class FollowingBehavior : Behavior
	{
		#region Initialization
        public FollowingBehavior(Animal animal)
            : base(animal)
        {
        }
        #endregion

		#region Update
        public override void Update(Animal otherAnimal, AIParameters aiParams)
        {
            base.ResetReaction();

            Vector2 dangerDirection = Vector2.Zero;

            //Vector2.Dot will return a positive result in this case if the 
            //otherAnimal is in front of the animal, in that case we don’t have to 
            //worry about it because we’re already following it.
            if (Vector2.Dot(
                Animal.Location, Animal.ReactionLocation) >= (Math.PI / 2))
            {
                //set the animal to fleeing so that it flashes blue
                Animal.AnimalState = AnimalState.Following;
                reacted = true;

                dangerDirection = Animal.Location + Animal.ReactionLocation;
                Vector2.Normalize(ref dangerDirection, out dangerDirection);
                
                reaction = (aiParams.PerDangerWeight * dangerDirection);
            }
        }
        #endregion
	}
}

