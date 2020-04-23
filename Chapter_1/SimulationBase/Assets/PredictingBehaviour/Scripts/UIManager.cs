using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public class UIManager : Manager<UIManager>
    {
        public void PauseSim()
        {
            Time.timeScale = 0;            
        }
        public void ContinueSim()
        {
            Time.timeScale = 1;            
        }

        private void LimitTimeScale()
        {
            if (Time.timeScale < .1f) Time.timeScale = .1f;
            if (Time.timeScale > 16) Time.timeScale = 16;
        }

        public void SpeedUpSim()
        {
            LimitTimeScale();
            Time.timeScale = Time.timeScale * 2f;
        }

        public void SlowSim()
        {
            LimitTimeScale();
            Time.timeScale = Time.timeScale * .5f;
        }

    }
}
