using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public enum CameraView
    {
        OVERVIEW,
        OVERHEAD, 
        FREE
    }

    public class CameraManager : Manager<CameraManager>
    {
        public CameraView cameraView;
        private CameraView _cameraView;

        public Camera mainCamera;
        public Transform sceneOverview;
        public Transform sceneOverhead;
        public bool flyCamEnabled
        {
            get
            {
                if (mainCamera == null) return false;
                if(flyComponent != null)
                {
                    return flyComponent.isActiveAndEnabled;
                }
                return false;
            }
            set
            {
                if (mainCamera == null) return;
                if(flyComponent != null)
                {
                    flyComponent.enabled = value;
                }
            }
        }
        private FreeFlyCamera flyComponent;

        private void Update()
        {
            if(_cameraView != cameraView)
            {
                SetCamera(cameraView);
            }
        }

        private void Start()
        {
            if(mainCamera != null)
            {
                flyComponent = mainCamera.GetComponent<FreeFlyCamera>();
                flyCamEnabled = false;
            }
        }

        public void SetCamera(CameraView view)
        {
            if(mainCamera != null)
            {
                cameraView = view;
                _cameraView = view;
                switch (view)
                {
                    case CameraView.FREE:
                        flyCamEnabled = true;
                        EnableFlyCamera(true);
                        break;

                    case CameraView.OVERVIEW:
                        mainCamera.transform.Copy(sceneOverview);
                        EnableFlyCamera(false);
                        break;

                    case CameraView.OVERHEAD:
                        mainCamera.transform.Copy(sceneOverhead);
                        EnableFlyCamera(false);
                        break;

                }
            }
        }

        private void EnableFlyCamera(bool active)
        {
            flyCamEnabled = active;
        }
    }
}
