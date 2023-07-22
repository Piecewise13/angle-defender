using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParentUltimateAbility : MonoBehaviour
{


    [SerializeField] protected float chargeAmount;

    protected float currentCharge;

    protected float chargeRate = 2f;



    protected PlayerScript player;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    //charges the ultimate meter by a specific amount each time this is called
    public virtual bool ChargeUp(float multiplier)
    {
        currentCharge += chargeRate * multiplier;
        player.hudScript.UpdateUltimateAbilityCharge(currentCharge / chargeAmount);
        print(currentCharge / chargeAmount);
        return currentCharge >= chargeAmount;
    }

    public abstract void ActivateAbility();

    public abstract void EndAbility();

    public virtual void DepleteCharge()
    {
        player.hudScript.UpdateUltimateAbilityCharge(0f);
        currentCharge = 0f;
    }

    public void SetChargeAmount(float amount)
    {
        chargeAmount = amount;
    }

    public virtual void DestroyAbility()
    {
        Destroy(this);
    }
}
