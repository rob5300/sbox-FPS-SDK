﻿using Sandbox;

namespace Amper.Source1;

partial class Source1Weapon
{
	[Net, Predicted]
	public float NextAttackTime { get; set; }

	/// <summary>
	/// This simulates weapon's secondary attack.
	/// </summary>
	public virtual void SimulateSecondaryAttack()
	{
		if ( !WishSecondaryAttack() )
			return;

		if ( !CanSecondaryAttack() )
			return;

		SecondaryAttack();
	}

	public virtual bool CanAttack()
	{
		if ( !GameRules.Current.CanWeaponsAttack() )
			return false;

		if ( !Player.CanAttack() )
			return false;

		return true;
	}

	public virtual float CalculateNextAttackTime( float attackTime )
	{
		// Fixes:
		// https://www.youtube.com/watch?v=7puuYqq_rgw

		var curAttack = NextAttackTime;
		var deltaAttack = Time.Now - curAttack;

		if ( deltaAttack < 0 || deltaAttack > Global.TickInterval )
		{
			curAttack = Time.Now;
		}

		NextAttackTime = curAttack + attackTime;
		return curAttack;
	}

	/// <summary>
	/// When weapon fired while having no ammo.
	/// </summary>
	public virtual void OnDryFire()
	{
		if ( !PlayEmptySound() )
			return;

		NextAttackTime = Time.Now + 0.2f;
	}

	/// <summary>
	/// Procedure to play empty fire sound, if game needs it.
	/// If your weapon needs dry fire sounds, play it in this function and return true.
	/// Otherwise return false.
	/// </summary>
	public virtual bool PlayEmptySound() => false;

	public virtual void SecondaryAttack() { }
	public virtual bool WishSecondaryAttack() => Input.Down( InputButton.SecondaryAttack );

	public virtual bool CanSecondaryAttack()
	{
		return CanPrimaryAttack();
	}

	public virtual void DoPlayerModelAnimation()
	{
		SendPlayerAnimParameter( "b_fire" );
	}

	public virtual void DoViewModelAnimation()
	{
		SendViewModelAnimParameter( "b_fire" );
	}


	/// <summary>
	/// Return the position in the worldspace, from which the attack is made.
	/// </summary>
	/// <returns></returns>
	public virtual Vector3 GetAttackOrigin()
	{
		if ( Owner == null ) return Vector3.Zero;
		return Owner.EyePosition;
	}

	/// <summary>
	/// Return the diretion of the attack of this weapon.
	/// </summary>
	/// <returns></returns>
	public virtual Vector3 GetAttackDirection()
	{
		if ( Owner == null ) return Vector3.Zero;
		return Owner.EyeRotation.Forward;
	}

	/// <summary>
	/// Return the diretion of the attack of this weapon.
	/// </summary>
	/// <returns></returns>
	public virtual Vector3 GetAttackSpreadDirection()
	{
		var dir = GetAttackDirection();

		var spread = GetBulletSpread();
		dir += Vector3.Random * spread * .15f;

		return dir;
	}
}