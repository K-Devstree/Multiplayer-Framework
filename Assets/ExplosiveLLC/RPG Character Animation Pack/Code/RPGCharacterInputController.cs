using UnityEngine;

namespace RPGCharacterAnims
{
    public class RPGCharacterInputController : MonoBehaviour
    {
        //LegacyInputs.
        [HideInInspector] public Vector2 inputAiming;
        [HideInInspector] public Vector2 inputMovement;
        [HideInInspector] public bool inputJump;
        [HideInInspector] public bool inputLightHit;
        [HideInInspector] public bool inputDeath;
        [HideInInspector] public bool inputAttackL;
        [HideInInspector] public bool inputAttackR;
        [HideInInspector] public bool inputCastL;
        [HideInInspector] public bool inputCastR;
        [HideInInspector] public float inputSwitchUpDown;
        [HideInInspector] public float inputSwitchLeftRight;
        [HideInInspector] public float inputBlock = 0;
        [HideInInspector] public bool inputTarget;
        [HideInInspector] public bool inputRoll;
        [HideInInspector] public bool inputShield;
        [HideInInspector] public bool inputRelax;

        //Variables.
        [HideInInspector] public bool allowedInput = true;
        [HideInInspector] public Vector3 moveInput;

        private void Awake()
        {
            allowedInput = true;
        }

        private void Update()
        {
            Inputs();
            HasJoystickConnected();
            moveInput = CameraRelativeInput(inputMovement.x, inputMovement.y);
        }

        /// <summary>
        /// Input abstraction for easier asset updates using outside control schemes.
        /// </summary>
        private void Inputs()
        {
            try
            {
                //inputMouseFacing = Mouse.current.position.ReadValue();
                //inputFacing = rpgInputs.RPGCharacter.Facing.ReadValue<Vector2>();
                //inputFace = rpgInputs.RPGCharacter.Face.IsPressed();
                inputAiming = new Vector2(Input.GetAxisRaw("AimHorizontal"), Input.GetAxisRaw("AimVertical"));
                inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                inputBlock = Input.GetAxisRaw("Block");
                inputJump = Input.GetButtonDown("Jump");
                inputLightHit = Input.GetButtonDown("LightHit");
                inputDeath = Input.GetButtonDown("Death");
                inputAttackL = Input.GetButtonDown("AttackL");
                inputAttackR = Input.GetButtonDown("AttackR");
                inputCastL = Input.GetButtonDown("CastL");
                inputCastR = Input.GetButtonDown("CastR");
                inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
                inputSwitchLeftRight = Input.GetAxisRaw("SwitchLeftRight");
                inputTarget = Input.GetButton("Target");
                inputRoll = Input.GetButtonDown("L3");
                inputShield = Input.GetButtonDown("Shield");
                inputRelax = Input.GetButtonDown("Relax");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("INPUTS NOT FOUND!");
            }
        }

        /// <summary>
        /// Movement based off camera facing.
        /// </summary>
        private Vector3 CameraRelativeInput(float inputX, float inputZ)
        {
            //Forward vector relative to the camera along the x-z plane.
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            //Right vector relative to the camera always orthogonal to the forward vector.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 relativeVelocity = inputMovement.x * right + inputMovement.y * forward;
            //Reduce input for diagonal movement.
            if (relativeVelocity.magnitude > 1)
            {
                relativeVelocity.Normalize();
            }
            return relativeVelocity;
        }

        public bool HasAnyInput()
        {
            //if (allowedInput && (HasMoveInput() || HasFacingInput() && inputJump != false))
            if (allowedInput && (HasMoveInput() || HasAimInput() && inputJump != false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasMoveInput()
        {
            if (allowedInput && (inputMovement != Vector2.zero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool HasFacingInput()
        //{
        //    if (allowedInput && (inputFacing != Vector2.zero))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public bool HasAimInput()
        {
            if (allowedInput && (inputAiming != Vector2.zero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasJoystickConnected()
        {
            //No joysticks.
            if (Input.GetJoystickNames().Length == 0)
            {
                //Debug.Log("No Joystick Connected");
                return false;
            }
            else
            {
                //Debug.Log("Joystick Connected");
                //If joystick is plugged in.
                for (int i = 0; i < Input.GetJoystickNames().Length; i++)
                {
                    //Debug.Log(Input.GetJoystickNames()[i].ToString());
                }
                return true;
            }
        }

        public bool HasBlockInput()
        {
            if (inputBlock != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}