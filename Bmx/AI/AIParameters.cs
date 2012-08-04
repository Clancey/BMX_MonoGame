using System;

namespace GameStateManagement
{
	#region FlockingAIParameters
	public struct AIParameters
	{
		/// <summary>
        /// how far away the animals see each other
        /// </summary>
        public float DetectionDistance;
        /// <summary>
        /// seperate from animals inside this distance
        /// </summary>
        public float SeparationDistance;
        /// <summary>
        /// how much the animal tends to move in it's previous direction
        /// </summary>
        public float MoveInOldDirectionInfluence;
        /// <summary>
        /// how much the animal tends to move with animals in it's detection distance
        /// </summary>
        public float MoveInFlockDirectionInfluence;
        /// <summary>
        /// how much the animal tends to move randomly
        /// </summary>
        public float MoveInRandomDirectionInfluence;
        /// <summary>
        /// how quickly the animal can turn
        /// </summary>
        public float MaxTurnRadians;
        /// <summary>
        /// how much each nearby animal influences it's behavior
        /// </summary>
        public float PerMemberWeight;
        /// <summary>
        /// how much dangerous animals influence it's behavior
        /// </summary>
        public float PerDangerWeight;
	}
	#endregion
}

