using JetBrains.Annotations;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AK.Wwise.Bank SFX;
    public AK.Wwise.Event play_dash;
    public PlayerMovement isDashing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SFX.Load();
    }

    // Update is called once per frame
    void Update()
    {
   
        if (isDashing == true)
        {
            play_dash.Post(gameObject);
        }
    }
    private void OnDestroy()
    {
        SFX.Unload();
    }
}
