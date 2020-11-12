using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Player")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string jumpButton = "Jump";
    public PlayerStates playerStates;
    public bool useJoystick = true;
    private CharacterController characterController;
    private AudioSource _audioSource;

    [Header("Player Movement")]
    [Range(1f, 15f)]
    public float walkSpeed = 2.5f;
    [Range(1f, 15f)]
    public float runSpeed = 6f;
    [Range(1f, 15f)]
    public float JumpForce = 4f;
    private float hInput;
    private float vInput;

    [Header("Animator and Parameters")]
    public Animator CharacterAnimator;
    private float HorzAnimation;
    private float VertAnimation;
    private bool JumpAnimation;
    private bool LandAnimation;
    private float _footstepDelay;
    private float footstep_et;

    [Header("Sounds")]
    public List<AudioClip> FootstepSounds;
    public List<AudioClip> JumpSounds;
    public List<AudioClip> LandSounds;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        //handle controller
        HandlePlayerControls();

        //sync animations with controller
        SetCharacterAnimations();

        //sync footsteps with controller
        PlayFootstepSounds();

        if (useJoystick)
        {
            if (SimpleInput.GetButtonDown(jumpButton) && characterController.isGrounded)
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetButtonDown(jumpButton) && characterController.isGrounded)
            {
                Jump();
            }
        }
    }

    void HandlePlayerControls()
    {
        if (useJoystick)
        {
            hInput = SimpleInput.GetAxisRaw(horizontalAxis);
            vInput = SimpleInput.GetAxisRaw(verticalAxis);
        }
        else
        {
            hInput = Input.GetAxisRaw(horizontalAxis);
            vInput = Input.GetAxisRaw(verticalAxis);
        }

        Vector3 fwdMovement = characterController.isGrounded == true ? transform.forward * vInput : Vector3.zero;
        Vector3 rightMovement = characterController.isGrounded == true ? transform.right * hInput : Vector3.zero;

        float _speed = vInput > 0.8f ? runSpeed : walkSpeed;
        characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * _speed);

        //Managing Player States
        if (characterController.isGrounded)
        {
            if (hInput == 0 && vInput == 0)
            {
                playerStates = PlayerStates.Idle;
            }
            else
            {
                if (_speed == walkSpeed)
                    playerStates = PlayerStates.Walking;
                else
                    playerStates = PlayerStates.Running;

                _footstepDelay = (2 / _speed);
            }
        }
        else
        {
            playerStates = PlayerStates.Jumping;
        }
    }

    void Jump()
    {
        StartCoroutine(PerformJumpRoutine());
        JumpAnimation = true;
    }

    IEnumerator PerformJumpRoutine()
    {
        //play jump sound
        if (_audioSource)
            _audioSource.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Count)]);

        float _jump = JumpForce;

        if (JumpAnimation)
        {
            CharacterAnimator.SetTrigger("Jump");
            JumpAnimation = false;
        }

        do
        {
            characterController.Move(Vector3.up * _jump * Time.deltaTime);
            _jump -= Time.deltaTime;
            yield return null;
        }
        while (!characterController.isGrounded);

        //play land sound
        if (_audioSource)
        {
            _audioSource.PlayOneShot(LandSounds[Random.Range(0, LandSounds.Count)]);
        }
    }

    void SetCharacterAnimations()
    {
        if (!CharacterAnimator)
            return;

        switch (playerStates)
        {
            case PlayerStates.Idle:
                HorzAnimation = Mathf.Lerp(HorzAnimation, 0, 5 * Time.deltaTime);
                VertAnimation = Mathf.Lerp(VertAnimation, 0, 5 * Time.deltaTime);
                break;

            case PlayerStates.Walking:
                if (useJoystick)
                {
                    HorzAnimation = SimpleInput.GetAxis(horizontalAxis);
                    VertAnimation = SimpleInput.GetAxis(verticalAxis);
                }
                else
                {
                    HorzAnimation = Mathf.Lerp(HorzAnimation, 1 * Input.GetAxis(horizontalAxis), 5 * Time.deltaTime);
                    VertAnimation = Mathf.Lerp(VertAnimation, 1 * Input.GetAxis(verticalAxis), 5 * Time.deltaTime);
                }
                break;

            case PlayerStates.Running:
                if (useJoystick)
                {
                    HorzAnimation = 2 * SimpleInput.GetAxis(horizontalAxis);
                    VertAnimation = 2 * SimpleInput.GetAxis(verticalAxis);
                }
                else
                {
                    HorzAnimation = Mathf.Lerp(HorzAnimation, 2 * Input.GetAxis(horizontalAxis), 5 * Time.deltaTime);
                    VertAnimation = Mathf.Lerp(VertAnimation, 2 * Input.GetAxis(verticalAxis), 5 * Time.deltaTime);
                }                
                break;

            //case PlayerStates.Jumping:
            //    if (JumpAnimation)
            //    {
            //        CharacterAnimator.SetTrigger("Jump");
            //        JumpAnimation = false;
            //    }
            //    break;
        }

        LandAnimation = characterController.isGrounded;
        CharacterAnimator.SetFloat("Horizontal", HorzAnimation);
        CharacterAnimator.SetFloat("Vertical", VertAnimation);
        CharacterAnimator.SetBool("isGrounded", LandAnimation);
    }

    void PlayFootstepSounds()
    {
        if (playerStates == PlayerStates.Idle || playerStates == PlayerStates.Jumping)
            return;

        if (footstep_et < _footstepDelay)
        {
            footstep_et += Time.deltaTime;
        }
        else
        {
            footstep_et = 0;
            _audioSource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Count)]);
        }
    }
}