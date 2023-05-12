using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-10)]
public class InputManager : MonoBehaviourSingleton<InputManager>
{
    #region Events
    public delegate void StartTouch(Vector3 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector3 position, float time);
    public event EndTouch OnEndTouch;
    public delegate void MoveJoystick(Vector2 direction);
    public event MoveJoystick OnMoveJoystick;

    #endregion

    private PlayerTouchController playerControls;
    private Camera mainCamera;

    private void Awake() {
        playerControls = new PlayerTouchController();
        mainCamera = Camera.main;
    }
    void Start()
    {
        playerControls.Touch.PrimaryContact.performed += context => StartTouchPrimary(context);
        playerControls.Touch.CamJoystick.performed += context => MoveCamera(context);
    }

    void MoveCamera(InputAction.CallbackContext context){
        if (OnMoveJoystick != null)
        {
            OnMoveJoystick(context.ReadValue<Vector2>());
        }
    }
    void StartTouchPrimary(InputAction.CallbackContext context){
        if(context.ReadValue<float>() == 1){
            if (OnStartTouch != null)
            {
                OnStartTouch(PrimaryPosition(), (float)context.startTime);
            }
        }
        else{
            if (OnEndTouch != null)
            {
                OnEndTouch(PrimaryPosition(), (float)context.time);
            }
        }
    }
    
    public Vector3 PrimaryPosition(){
        Vector3 potionInWorldSpace = Utils.ScreenToWorld(mainCamera, getTouchPosition());
        
        return potionInWorldSpace;
    }
    public Vector3 getTouchPosition(){
        return playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
    }

    #region Enable
    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    #endregion

}
