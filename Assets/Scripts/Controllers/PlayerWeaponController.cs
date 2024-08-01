using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponController : WeaponController
{
    public Rig armRightRig;
    public Rig armLeftRig;

    private PlayerController player;

    public void SetRightArm(float value)
    {
        armRightRig.weight = value;
    }
    
    public void SetLeftArm(float value)
    {
        armLeftRig.weight = value;
    }

    protected override void Start()
    {
        base.Start();

        player = GetComponentInParent<PlayerController>();
        if(!player)
            player = transform.parent.GetComponentInParent<PlayerController>();

        player.UpdateEquipment(profile, currentMagazine);
        onMagazineChange.AddListener( () => { player.UpdateEquipment(profile, currentMagazine); } );
    }

    void Update()
    {
        HandleShooting();
    }
}