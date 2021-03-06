﻿using System.Collections.Generic;
using FarseerPhysics.Common;
//using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public class Ragdoll
    {
        private const float ArmDensity = 10;
        private const float LegDensity = 15;
        private const float LimbAngularDamping = 7;

        private Body _body;
        private Body _head;

        private Body _lowerLeftArm;
        private Body _lowerLeftLeg;
        private Body _lowerRightArm;
        private Body _lowerRightLeg;

        private Body _upperLeftArm;
        private Body _upperLeftLeg;
        private Body _upperRightArm;
        private Body _upperRightLeg;

        private Sprite _face;
        private Sprite _torso;
        private Sprite _upperArm;
        private Sprite _lowerArm;
        private Sprite _upperLeg;
        private Sprite _lowerLeg;


		private ScreenManager ScreenManager;

		public Ragdoll(World world,  ScreenManager manager, Vector2 position)
        {
            CreateBody(world, position);
            CreateJoints(world);

			ScreenManager = manager;
            CreateGFX();
        }

        public Body Body
        {
            get { return _body; }
        }

        //Torso
        private void CreateBody(World world, Vector2 position)
        {
            //Head
            _head = BodyFactory.CreateCircle(world, 0.9f, 10f);
            _head.BodyType = BodyType.Dynamic;
            _head.AngularDamping = LimbAngularDamping;
            _head.Mass = 2f;
            _head.Position = position;

            //Body
            _body = BodyFactory.CreateRoundedRectangle(world, 2f, 4f, 0.5f, 0.7f, 2, 10f);
            _body.BodyType = BodyType.Dynamic;
            _body.Mass = 2f;
            _body.Position = position + new Vector2(0f, 3f);

            //Left Arm
            _lowerLeftArm = BodyFactory.CreateCapsule(world, 1f, 0.45f, ArmDensity);
            _lowerLeftArm.BodyType = BodyType.Dynamic;
            _lowerLeftArm.AngularDamping = LimbAngularDamping;
            _lowerLeftArm.Mass = 2f;
            _lowerLeftArm.Rotation = 1.4f;
            _lowerLeftArm.Position = position + new Vector2(-4f, 2.2f);

            _upperLeftArm = BodyFactory.CreateCapsule(world, 1f, 0.45f, ArmDensity);
            _upperLeftArm.BodyType = BodyType.Dynamic;
            _upperLeftArm.AngularDamping = LimbAngularDamping;
            _upperLeftArm.Mass = 2f;
            _upperLeftArm.Rotation = 1.4f;
            _upperLeftArm.Position = position + new Vector2(-2f, 1.8f);

            //Right Arm
            _lowerRightArm = BodyFactory.CreateCapsule(world, 1f, 0.45f, ArmDensity);
            _lowerRightArm.BodyType = BodyType.Dynamic;
            _lowerRightArm.AngularDamping = LimbAngularDamping;
            _lowerRightArm.Mass = 2f;
            _lowerRightArm.Rotation = -1.4f;
            _lowerRightArm.Position = position + new Vector2(4f, 2.2f);

            _upperRightArm = BodyFactory.CreateCapsule(world, 1f, 0.45f, ArmDensity);
            _upperRightArm.BodyType = BodyType.Dynamic;
            _upperRightArm.AngularDamping = LimbAngularDamping;
            _upperRightArm.Mass = 2f;
            _upperRightArm.Rotation = -1.4f;
            _upperRightArm.Position = position + new Vector2(2f, 1.8f);

            //Left Leg
            _lowerLeftLeg = BodyFactory.CreateCapsule(world, 1f, 0.5f, LegDensity);
            _lowerLeftLeg.BodyType = BodyType.Dynamic;
            _lowerLeftLeg.AngularDamping = LimbAngularDamping;
            _lowerLeftLeg.Mass = 2f;
            _lowerLeftLeg.Position = position + new Vector2(-0.6f, 8f);

            _upperLeftLeg = BodyFactory.CreateCapsule(world, 1f, 0.5f, LegDensity);
            _upperLeftLeg.BodyType = BodyType.Dynamic;
            _upperLeftLeg.AngularDamping = LimbAngularDamping;
            _upperLeftLeg.Mass = 2f;
            _upperLeftLeg.Position = position + new Vector2(-0.6f, 6f);

            //Right Leg
            _lowerRightLeg = BodyFactory.CreateCapsule(world, 1f, 0.5f, LegDensity);
            _lowerRightLeg.BodyType = BodyType.Dynamic;
            _lowerRightLeg.AngularDamping = LimbAngularDamping;
            _lowerRightLeg.Mass = 2f;
            _lowerRightLeg.Position = position + new Vector2(0.6f, 8f);

            _upperRightLeg = BodyFactory.CreateCapsule(world, 1f, 0.5f, LegDensity);
            _upperRightLeg.BodyType = BodyType.Dynamic;
            _upperRightLeg.AngularDamping = LimbAngularDamping;
            _upperRightLeg.Mass = 2f;
            _upperRightLeg.Position = position + new Vector2(0.6f, 6f);
        }

        private void CreateJoints(World world)
        {
            const float dampingRatio = 1f;
            const float frequency = 25f;

            //head -> body
            DistanceJoint jHeadBody = new DistanceJoint(_head, _body,
                                                        new Vector2(0f, 1f),
                                                        new Vector2(0f, -2f));
            jHeadBody.CollideConnected = true;
            jHeadBody.DampingRatio = dampingRatio;
            jHeadBody.Frequency = frequency;
            jHeadBody.Length = 0.025f;
            world.AddJoint(jHeadBody);

            //lowerLeftArm -> upperLeftArm
            DistanceJoint jLeftArm = new DistanceJoint(_lowerLeftArm, _upperLeftArm,
                                                       new Vector2(0f, -1f),
                                                       new Vector2(0f, 1f));
            jLeftArm.CollideConnected = true;
            jLeftArm.DampingRatio = dampingRatio;
            jLeftArm.Frequency = frequency;
            jLeftArm.Length = 0.02f;
            world.AddJoint(jLeftArm);

            //upperLeftArm -> body
            DistanceJoint jLeftArmBody = new DistanceJoint(_upperLeftArm, _body,
                                                           new Vector2(0f, -1f),
                                                           new Vector2(-1f, -1.5f));
            jLeftArmBody.CollideConnected = true;
            jLeftArmBody.DampingRatio = dampingRatio;
            jLeftArmBody.Frequency = frequency;
            jLeftArmBody.Length = 0.02f;
            world.AddJoint(jLeftArmBody);

            //lowerRightArm -> upperRightArm
            DistanceJoint jRightArm = new DistanceJoint(_lowerRightArm, _upperRightArm,
                                                        new Vector2(0f, -1f),
                                                        new Vector2(0f, 1f));
            jRightArm.CollideConnected = true;
            jRightArm.DampingRatio = dampingRatio;
            jRightArm.Frequency = frequency;
            jRightArm.Length = 0.02f;
            world.AddJoint(jRightArm);

            //upperRightArm -> body
            DistanceJoint jRightArmBody = new DistanceJoint(_upperRightArm, _body,
                                                            new Vector2(0f, -1f),
                                                            new Vector2(1f, -1.5f));

            jRightArmBody.CollideConnected = true;
            jRightArmBody.DampingRatio = dampingRatio;
            jRightArmBody.Frequency = 25;
            jRightArmBody.Length = 0.02f;
            world.AddJoint(jRightArmBody);

            //lowerLeftLeg -> upperLeftLeg
            DistanceJoint jLeftLeg = new DistanceJoint(_lowerLeftLeg, _upperLeftLeg,
                                                       new Vector2(0f, -1.1f),
                                                       new Vector2(0f, 1f));
            jLeftLeg.CollideConnected = true;
            jLeftLeg.DampingRatio = dampingRatio;
            jLeftLeg.Frequency = frequency;
            jLeftLeg.Length = 0.05f;
            world.AddJoint(jLeftLeg);

            //upperLeftLeg -> body
            DistanceJoint jLeftLegBody = new DistanceJoint(_upperLeftLeg, _body,
                                                           new Vector2(0f, -1.1f),
                                                           new Vector2(-0.8f, 1.9f));
            jLeftLegBody.CollideConnected = true;
            jLeftLegBody.DampingRatio = dampingRatio;
            jLeftLegBody.Frequency = frequency;
            jLeftLegBody.Length = 0.02f;
            world.AddJoint(jLeftLegBody);

            //lowerRightleg -> upperRightleg
            DistanceJoint jRightLeg = new DistanceJoint(_lowerRightLeg, _upperRightLeg,
                                                        new Vector2(0f, -1.1f),
                                                        new Vector2(0f, 1f));
            jRightLeg.CollideConnected = true;
            jRightLeg.DampingRatio = dampingRatio;
            jRightLeg.Frequency = frequency;
            jRightLeg.Length = 0.05f;
            world.AddJoint(jRightLeg);

            //upperRightleg -> body
            DistanceJoint jRightLegBody = new DistanceJoint(_upperRightLeg, _body,
                                                            new Vector2(0f, -1.1f),
                                                            new Vector2(0.8f, 1.9f));
            jRightLegBody.CollideConnected = true;
            jRightLegBody.DampingRatio = dampingRatio;
            jRightLegBody.Frequency = frequency;
            jRightLegBody.Length = 0.02f;
            world.AddJoint(jRightLegBody);
        }

        private void CreateGFX()
        {

            //_face = new Sprite(_creator.CircleTexture(0.9f, MaterialType.Squares, Color.Gray, 1f));
			_torso = new Sprite(ScreenManager.Game.Content.Load<Texture2D>("Biker/body"));

			_upperArm = new Sprite(ScreenManager.Game.Content.Load<Texture2D>("Biker/arm-1"));
			_lowerArm =  new Sprite(ScreenManager.Game.Content.Load<Texture2D>("Biker/arm-2"));

			_upperLeg = new Sprite(ScreenManager.Game.Content.Load<Texture2D>("Biker/leg-1"));
			_lowerLeg = new Sprite(ScreenManager.Game.Content.Load<Texture2D>("Biker/leg-2"));
        }

		public void Draw()
        {
			float scale = .5f;
			SpriteBatch batch = ScreenManager.SpriteBatch;
			batch.Draw(_lowerLeg.Texture, ConvertUnits.ToDisplayUnits(_lowerLeftLeg.Position), null,
			           Color.White, _lowerLeftLeg.Rotation, _lowerLeg.Origin, scale, SpriteEffects.None, 0f);
			batch.Draw(_lowerLeg.Texture, ConvertUnits.ToDisplayUnits(_lowerRightLeg.Position), null,
			           Color.White, _lowerRightLeg.Rotation, _lowerLeg.Origin, scale, SpriteEffects.None, 0f);
			
			batch.Draw(_upperLeg.Texture, ConvertUnits.ToDisplayUnits(_upperLeftLeg.Position), null,
			           Color.White, _upperLeftLeg.Rotation, _upperLeg.Origin, scale, SpriteEffects.None, 0f);
			batch.Draw(_upperLeg.Texture, ConvertUnits.ToDisplayUnits(_upperRightLeg.Position), null,
			           Color.White, _upperRightLeg.Rotation, _upperLeg.Origin, scale, SpriteEffects.None, 0f);
			
			batch.Draw(_lowerArm.Texture, ConvertUnits.ToDisplayUnits(_lowerLeftArm.Position), null,
			           Color.White, _lowerLeftArm.Rotation, _lowerArm.Origin, scale, SpriteEffects.None, 0f);
			batch.Draw(_lowerArm.Texture, ConvertUnits.ToDisplayUnits(_lowerRightArm.Position), null,
			           Color.White, _lowerRightArm.Rotation, _lowerArm.Origin, scale, SpriteEffects.None, 0f);
			
			batch.Draw(_upperArm.Texture, ConvertUnits.ToDisplayUnits(_upperLeftArm.Position), null,
			           Color.White, _upperLeftArm.Rotation, _upperArm.Origin, scale, SpriteEffects.None, 0f);
			batch.Draw(_upperArm.Texture, ConvertUnits.ToDisplayUnits(_upperRightArm.Position), null,
			           Color.White, _upperRightArm.Rotation, _upperArm.Origin, scale, SpriteEffects.None, 0f);
			
			batch.Draw(_torso.Texture, ConvertUnits.ToDisplayUnits(_body.Position), null,
			           Color.White, _body.Rotation, _torso.Origin, scale, SpriteEffects.None, 0f);
			
//			batch.Draw(_face.Texture, ConvertUnits.ToDisplayUnits(_head.Position), null,
//			           Color.White, _head.Rotation, _face.Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}