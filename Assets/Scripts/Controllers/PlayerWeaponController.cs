using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponController : WeaponController
{
    [Space]
    public GameObject aimGuide;

    [Space]
    public Rig armRightRig;
    public Rig armLeftRig;

    [Space]
    public Transform armRightTarget;
    public Transform armLeftTarget;

    private Vector3 armRightBasePosition;
    private Vector3 armLeftBasePosition;

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

        armRightBasePosition = armRightTarget.transform.localPosition;
        armLeftBasePosition = armLeftTarget.transform.localPosition;

        player = GetComponentInParent<PlayerController>();
        if(!player)
            player = transform.parent.GetComponentInParent<PlayerController>();

        player.UpdateEquipment(profile, currentMagazine);

        onWeaponChange.AddListener( () => {
            muzzle.parent.gameObject.layer = LayerMask.NameToLayer("Player");
            aimGuide.SetActive(profile);
        } );

        onMagazineChange.AddListener( () => {
            Recoil();
            aimGuide.SetActive(profile && currentMagazine != 0);
            player.UpdateEquipment(profile, currentMagazine);
        } );
    }

    void Recoil()
    {
        Vector3 recoilOffset = ((player.transform.up/2.0f) - player.transform.forward) * profile.recoilOffset;
        displayTransform.localPosition += recoilOffset;
        armRightTarget.localPosition += recoilOffset;
        armLeftTarget.localPosition += recoilOffset;
    }

    void RecoilRecovery()
    {
        if(profile && currentMagazine == 0)
            return;

        displayTransform.localPosition = Vector3.Lerp(displayTransform.localPosition, Vector3.zero, 8.0f * Time.deltaTime);
        armRightTarget.localPosition = Vector3.Lerp(armRightTarget.localPosition, armRightBasePosition, 8.0f * Time.deltaTime);
        armLeftTarget.localPosition = Vector3.Lerp(armLeftTarget.localPosition, armLeftBasePosition, 8.0f * Time.deltaTime);
    }

    void Update()
    {
        RecoilRecovery();
        HandleShooting();
    }
}