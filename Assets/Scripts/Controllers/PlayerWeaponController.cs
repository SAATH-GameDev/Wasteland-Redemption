using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    private PlayerController player;

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