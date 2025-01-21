using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AssetAssignmentHelper : MonoBehaviour
{
    [SerializeField] LevelConfiguration levelConfiguration;
    [SerializeField] ComboManager comboManager;
    [SerializeField] GameManagerVR gameManagerVR;
    [SerializeField] BladeVR bladeVRLeft;
    [SerializeField] BladeVR bladeVRRight;
    [SerializeField] Spawner spawner;
    [SerializeField] GameLogger gameLogger;
    [SerializeField] ControllerMovementLogger controllerMovementLogger;

    void OnValidate()
    {
        if (levelConfiguration == null)
        {
            Debug.LogWarning("LevelConfiguration is not assigned.");
            return;
        }

        if (comboManager != null)
            comboManager.configuration = levelConfiguration;

        if (gameManagerVR != null)
            gameManagerVR.configuration = levelConfiguration;

        if (bladeVRLeft != null)
            bladeVRLeft.configuration = levelConfiguration;

        if (bladeVRRight != null)
            bladeVRRight.configuration = levelConfiguration;

        if (spawner != null)
            spawner.configuration = levelConfiguration;

        if (gameLogger != null)
            gameLogger.configuration = levelConfiguration;

        if (controllerMovementLogger != null)
            controllerMovementLogger.configuration = levelConfiguration;

        Debug.Log("Configurations updated to LevelConfiguration in OnValidate.");
    }
}
