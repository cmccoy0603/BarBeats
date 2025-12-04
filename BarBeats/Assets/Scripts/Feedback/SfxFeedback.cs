using UnityEngine;

public class SfxFeedback : MonoBehaviour
{
    public AK.Wwise.Bank MainBank;
    public AK.Wwise.Event SfxEvent;

    public void PlaySound ()
    {
        SfxEvent.Post(gameObject);
    }


}
