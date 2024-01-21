using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // Создайте enum с понятными именами для ваших камер
    public enum CameraType
    {
        BaseCamera,
        CombatCamera,
        DeathCamera,
        // Добавьте другие имена камер
    }

    // Сопоставьте каждый тип камеры с виртуальной камерой Cinemachine
    [System.Serializable]
    public struct CameraSetup
    {
        public CameraType type;
        public CinemachineFreeLook camera;
    }

    // Массив с информацией о ваших камерах
    public CameraSetup[] cameraSetups;

    // Текущий активированный тип камеры
    private CameraType currentActiveCamera;

    private void Start()
    {
        // Установить начальную активную камеру, если нужно
        currentActiveCamera = CameraType.BaseCamera; // Пример начальной камеры
        ActivateCamera(currentActiveCamera);
    }

    private void ActivateCamera(CameraType cameraType)
    {
        foreach (var setup in cameraSetups)
        {
            setup.camera.Priority = (setup.type == cameraType) ? 10 : 5;
        }
        // Обновить текущий активный тип камеры
        currentActiveCamera = cameraType;
    }

    // Вызывайте этот метод для переключения камеры, используя понятные имена
    public void SwitchCamera(CameraType cameraType)
    {
        ActivateCamera(cameraType);
    }

    // Метод для получения типа текущей активной камеры
    public CameraType GetCurrentActiveCamera()
    {
        return currentActiveCamera;
    }
}