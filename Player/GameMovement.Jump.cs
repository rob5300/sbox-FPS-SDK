﻿using Sandbox;
using System;

namespace Source1
{
	public partial class S1GameMovement
	{
		public float JumpTime { get; set; }

		public virtual bool WishJump()
		{
			return Input.Pressed( InputButton.Jump );
		}

		public virtual bool CanJump()
		{
			// Yeah why not.
			return true;
		}

		/// <summary>
		/// Returns true if we succesfully made a jump.
		/// </summary>
		/// <returns></returns>
		public virtual bool Jump()
		{
			if ( !CanJump() )
				return false;

			/*
            if ( player->m_flWaterJumpTime )
            {
                player->m_flWaterJumpTime -= gpGlobals->frametime();
                if ( player->m_flWaterJumpTime < 0 )
                    player->m_flWaterJumpTime = 0;

                return false;
            }*/


			// If we are in the water most of the way...
			/*if ( IsSwimming )
			{
				// swimming, not jumping
				ClearGroundEntity();

				// Move upwards
				Velocity = Velocity.WithZ( 100 );

				// play swimming sound
				//  if ( player->m_flSwimSoundTime <= 0 )
				{
					// Don't play sound again for 1 second
					//   player->m_flSwimSoundTime = 1000;
					//   PlaySwimSound();
				}

				return false;
			}*/

			// Can't just if we're not grounded
			if ( GroundEntity == null )
				return false;

			/*
            if ( player->m_Local.m_bDucking && (player->GetFlags() & FL_DUCKING) )
                return false;
            */

			/*
            // Still updating the eye position.
            if ( player->m_Local.m_nDuckJumpTimeMsecs > 0u )
                return false;
            */

			ClearGroundEntity();

			// player->PlayStepSound( (Vector &)mv->GetAbsOrigin(), player->m_pSurfaceData, 1.0, true );

			AddEvent( "jump" );

			float flGroundFactor = 1.0f;
			//if ( player->m_pSurfaceData )
			{
				//   flGroundFactor = g_pPhysicsQuery->GetGameSurfaceproperties( player->m_pSurfaceData )->m_flJumpFactor;
			}

			float flMul = 268.3281572999747f * 1.2f;

			float startz = Velocity.z;

			if ( IsDucked )
				flMul *= 0.8f;

			Velocity = Velocity.WithZ( startz + flMul * flGroundFactor );
			Velocity -= new Vector3( 0, 0, GetCurrentGravity() * 0.5f ) * Time.Delta;

			return true;
		}

		public virtual void OnJump( float velocity )
		{

		}
	}
}
