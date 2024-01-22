using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
   
    public enum CameraType
    {
        BaseCamera,
        CombatCamera,
        DeathCamera,
       
    }

    
    [System.Serializable]
    public struct CameraSetup
    {
        public CameraType type;
        public CinemachineFreeLook camera;
    }

    
    public CameraSetup[] cameraSetups;


    [SerializeField] private CameraType currentActiveCamera = CameraType.BaseCamera;

    private void Start()
    {
        ActivateCamera(currentActiveCamera);
    }

    private void ActivateCamera(CameraType cameraType)
    {
        foreach (var setup in cameraSetups)
        {
            setup.camera.Priority = (setup.type == cameraType) ? 10 : 1;
        }
       
        currentActiveCamera = cameraType;
    }

    
    public void SwitchCamera(CameraType cameraType)
    {
        ActivateCamera(cameraType);
    }

    
    public CameraType GetCurrentActiveCamera()
    {
        return currentActiveCamera;
    }
}