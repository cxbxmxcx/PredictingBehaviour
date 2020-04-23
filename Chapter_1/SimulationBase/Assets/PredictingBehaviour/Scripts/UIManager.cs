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

        public void InfectNextCustomer()
        {
            InfectionManager.Instance.InfectNextCustomer();
        }

        public void ShowHideMap()
        {
            InfectionManager.Instance.ShowHideMap();
        }

        public void SetCameraOverview()
        {
            CameraManager.Instance.SetCamera(CameraView.OVERVIEW);
        }

        public void SetCameraOverhead()
        {
            CameraManager.Instance.SetCamera(CameraView.OVERHEAD);
        }

        public void SetFreeCamera()
        {
            CameraManager.Instance.SetCamera(CameraView.FREE);
        }

    }
}
