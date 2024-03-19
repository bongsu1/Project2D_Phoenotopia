using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;

    public enum SFX { WakeUp, NormalAttack, AirAttack, ChargeAttack, Hit, Shot, Roll, Hurt }

    public void PlaySFX(SFX sfx)
    {
        Manager.Sound.PlaySFX(audioClips[(int)sfx]);
        Debug.Log(sfx);
    }
}
