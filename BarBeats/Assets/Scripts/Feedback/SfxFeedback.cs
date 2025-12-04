using UnityEngine;

public class SfxFeedback : MonoBehaviour
{
    public AK.Wwise.Bank SFX;
    public AK.Wwise.Event SfxEvent;

    public void PlaySound ()
    {
        SfxEvent.Post(gameObject);
    }


}
