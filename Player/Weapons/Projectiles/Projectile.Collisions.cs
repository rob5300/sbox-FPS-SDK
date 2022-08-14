﻿using Sandbox;

namespace Amper.Source1;

partial class Projectile
{
	protected Vector3 Mins;
	protected Vector3 Maxs;

	public virtual void SetBBox( float size )
	{
		SetBBox( -size / 2, size / 2 );
	}

	public virtual void SetBBox( Vector3 mins, Vector3 maxs )
	{
		if ( Mins == mins && Maxs == maxs )
			return;

		Mins = mins;
		Maxs = maxs;
	}

	public virtual void SimulateCollisions()
	{
		// Don't simulate collisions if we don't move.
		if ( MoveType == ProjectileMoveType.None )
			return;

		var vecVelocityPerTick = Velocity * Time.Delta;

		var origin = Position - vecVelocityPerTick;
		var target = Position + vecVelocityPerTick;

		var tr = TraceBBox( origin, target );

		if ( tr.Hit )
			OnTraceTouch( tr.Entity, tr );
	}

	public virtual TraceResult TraceBBox( Vector3 start, Vector3 end )
	{
		return TraceBBox( start, end, Mins, Maxs );
	}

	public virtual TraceResult TraceBBox( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs )
	{
		if( sv_debug_projectile_collisions )
		{
			if ( mins != 0 || maxs != 0 )
				DebugOverlay.Box( start, mins, maxs, Color.Red, 0, false );
			else
				DebugOverlay.Line( start, start + Rotation.Forward * 5, Color.Red, 0, false );
		}	

		return SetupCollisionTrace( start, end, mins, maxs ).Run();
	}

	public virtual Trace SetupCollisionTrace( Vector3 start, Vector3 end )
	{
		return SetupCollisionTrace( start, end, Mins, Maxs );
	}

	public virtual Trace SetupCollisionTrace( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs )
	{
		var tr = Trace.Ray( start, end )

			// Collides with:
			.WithAnyTags( CollisionTags.Solid )
			.WithAnyTags( CollisionTags.Clip )
			.WithAnyTags( CollisionTags.ProjectileClip )

			// Except weapons and other projectiles.
			.WithoutTags( CollisionTags.Projectile )
			.WithoutTags( CollisionTags.Weapon )

			.Ignore( this )
			.Ignore( Owner );

		if ( !GameRules.mp_friendly_fire ) 
			tr = tr.WithoutTags( TeamManager.GetTag( TeamNumber ) );

		if ( mins != 0 || maxs != 0 ) tr = tr.Size( mins, maxs );
		return tr;
	}

	[ConVar.Replicated] public static bool sv_debug_projectile_collisions { get; set; }
}
