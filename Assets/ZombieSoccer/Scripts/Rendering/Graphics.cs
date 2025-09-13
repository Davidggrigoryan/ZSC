using UnityEngine;

namespace ZombieSoccer.Rendering
{
    public class Graphics
    {
        public void Initialize()
        {
            Debug.Log("Initialize graphics");

            //Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
            Application.targetFrameRate = 30;
        }
    }
}
