using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    private InputSystem inputSystem;
    private PlayerMovement player;
    [SerializeField] GameObject controlsCanvas;
    public float inputVertical {  get; private set; }
    public float inputHorizontal { get; private set; }


    private void Awake()
    {
        inputSystem = new InputSystem();
        player = GetComponent<PlayerMovement>();


    }

    private void OnEnable()
    {
        GetRebindsAndOverrideBindingds();
        EnableAndSubscribeActions();
    }

    private void EnableAndSubscribeActions()
    {
        inputSystem.Player.Enable();
        inputSystem.Player.Jump.performed += player.Jump;
        inputSystem.Player.Dash.started += player.Dash_Started;
        inputSystem.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        if (player.playerHealth > 0)
        {
            if (controlsCanvas.activeSelf)
            {
                Debug.Log(controlsCanvas.activeSelf);
                controlsCanvas.SetActive(false);
                GameManager.isGamePaused = false;
            }
            else
            {
                Debug.Log(controlsCanvas.activeSelf);

                controlsCanvas.SetActive(true);
                GameManager.isGamePaused = true;
            }
        }
    }

    private void OnDisable()
    {
        DisableAndUnsubscribeActions();

    }

    public void ActionLoadAndSave()
    {
        DisableAndUnsubscribeActions();
        inputSystem = new InputSystem();
        Debug.Log("first made new input system");
        GetRebindsAndOverrideBindingds();
        EnableAndSubscribeActions();
    }

    private void GetRebindsAndOverrideBindingds()
    {
        Debug.Log("then added the line to get the rebidns and override the rebinding");
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            inputSystem.asset.LoadBindingOverridesFromJson(rebinds);
    }

    private void DisableAndUnsubscribeActions()
    {
        inputSystem.Player.Disable();
        inputSystem.Player.Jump.performed -= player.Jump;
        inputSystem.Player.Dash.performed -= player.Dash_Started;
    }



    // Update is called once per frame
    void Update()
    {
        inputHorizontal = inputSystem.Player.MoveHorizontal.ReadValue<float>();
        inputVertical = inputSystem.Player.MoveVertical.ReadValue<float>();
    }
}
