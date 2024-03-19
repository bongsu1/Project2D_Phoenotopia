using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;

    public enum SFX { WakeUp, Jump, Run, NormalAttack,AirAttack, ChargeAttack, Hit,
        Shot, Roll, Grab, Pull, Push, Hurt, Death = 14 }

    public void PlaySFX(int index)
    {
        Manager.Sound.PlaySFX(audioClips[index]);
    }
}
