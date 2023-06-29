using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    [SerializeField]private int tier;
    public Sprite weaponIcon;
    protected PlayerScript player;
    protected static HUDScript hud;
    protected Animator playerAnimator;
    protected ModeManager inventory;

    protected static bool canShoot = false;

    public int GetTier()
    {
        return tier;
    }
}
