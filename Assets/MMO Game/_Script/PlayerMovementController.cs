using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    Walking,
    Running,
    Jumping
}

public class PlayerMovementController : MonoBehaviour
{
    public PlayerStates playerStates;
    private CharacterController characterController;
    private AudioSource _audioSource;

    [Header("Player Motor")]
    [Range(1f, 15f)]
    public float walkSpeed = 2.5f;
    [Range(1f, 15f)]
    public float runSpeed = 6f;
    [Range(1f, 15f)]
    public float JumpForce = 6f;

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
    }

    void HandlePlayerControls()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        Vector3 fwdMovement = characterController.isGrounded == true ? transform.forward * vInput : Vector3.zero;
        Vector3 rightMovement = characterController.isGrounded == true ? transform.right * hInput : Vector3.zero;

        float _speed = vInput > 0.8f ? runSpeed : walkSpeed;
        characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * _speed);

        if (characterController.isGrounded)
        {
            Jump();
        }

        //Managing Player States
        if (characterController.isGrounded)
        {
            if (hInput == 0 && vInput == 0)
            {
                Debug.Log("Idle Player");
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PerformJumpRoutine());
            JumpAnimation = true;
        }
    }

    IEnumerator PerformJumpRoutine()
    {
        //play jump sound
        if (_audioSource)
            _audioSource.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Count)]);

        float _jump = JumpForce;

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
                HorzAnimation = Mathf.Lerp(HorzAnimation, 1 * Input.GetAxis("Horizontal"), 5 * Time.deltaTime);
                VertAnimation = Mathf.Lerp(VertAnimation, 1 * Input.GetAxis("Vertical"), 5 * Time.deltaTime);
                break;

            case PlayerStates.Running:
                HorzAnimation = Mathf.Lerp(HorzAnimation, 2 * Input.GetAxis("Horizontal"), 5 * Time.deltaTime);
                VertAnimation = Mathf.Lerp(VertAnimation, 2 * Input.GetAxis("Vertical"), 5 * Time.deltaTime);
                break;

            case PlayerStates.Jumping:
                if (JumpAnimation)
                {
                    CharacterAnimator.SetTrigger("Jump");
                    JumpAnimation = false;
                }
                break;
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