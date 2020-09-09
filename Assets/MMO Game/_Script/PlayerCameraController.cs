using UnityEngine;

public enum CameraPerspective
{
    ThirdPerson,
    FirstPerson
}

public class PlayerCameraController : MonoBehaviour
{
    [Header("Common Camera Settings")]
    public bool lockCursor = true;
    public CameraPerspective cameraPerspective;
    public float mouseSensitivity = 25f;

    [Header("Character")]
    public Transform CharacterControllerTransform;
    public Animator CharacterAnimator;

    [Header("Camera Settings")]
    public Vector3 FPS_CameraOffset = new Vector3(0f, 0.1f, 0.25f);
    public Vector3 TPP_CameraOffset = new Vector3(0f, 1f, -1.25f);
    public float TPP_CameraHeightOffset = 0.5f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 10f;
    private Vector3 currentRotation;
    private float yaw;
    private float pitch;
    private Transform _fpsCameraHelper;
    private Transform _tppCameraHelper;

    [Header("Speeds")]
    public float moveSpeed = 5f;
    public float returnSpeed = 10f;
    public float wallPush = 0.5f;

    [Header("Distances")]
    public float closestDistanceToPlayer = 2f;
    public float evenCloserDistanceToPlayer = 1f;

    [Header("Mask")]
    public LayerMask collisionMask;

    private bool pitchLock = false;




    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (CharacterAnimator)
        {
            Add_FPSCamPositionHelper();
            Add_TPPCamPositionHelper();
        }
    }
    private void Add_FPSCamPositionHelper()
    {
        _fpsCameraHelper = new GameObject().transform;
        _fpsCameraHelper.name = "_fpsCameraHelper";
        _fpsCameraHelper.SetParent(CharacterAnimator.GetBoneTransform(HumanBodyBones.Head));
        _fpsCameraHelper.localPosition = Vector3.zero;
    }
    private void Add_TPPCamPositionHelper()
    {
        _tppCameraHelper = new GameObject().transform;
        _tppCameraHelper.name = "_tppCameraHelper";
        _tppCameraHelper.SetParent(CharacterControllerTransform);
        _tppCameraHelper.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SwitchCameraPerspective();
        }
    }
    public void SwitchCameraPerspective()
    {
        if (cameraPerspective == CameraPerspective.FirstPerson)
        {
            cameraPerspective = CameraPerspective.ThirdPerson;
            SetCameraHelperPosition_TPP();
        }
        else
        {
            cameraPerspective = CameraPerspective.FirstPerson;
            SetCameraHelperPosition_FPS();
        }
    }
    private void SetCameraHelperPosition_FPS()
    {
        if (CharacterAnimator)
        {
            _fpsCameraHelper.localPosition = FPS_CameraOffset;
            transform.position = _fpsCameraHelper.position;
        }
    }
    private void SetCameraHelperPosition_TPP()
    {
        if (CharacterAnimator)
        {
            _tppCameraHelper.localPosition = TPP_CameraOffset;
            transform.position = _tppCameraHelper.position;
        }
    }

    private void LateUpdate()
    {
        CameraControl();
    }

    private void CameraControl()
    {
        if (cameraPerspective == CameraPerspective.ThirdPerson)
        {
            CollisionCheck(CharacterControllerTransform.position - (Vector3.down * TPP_CameraHeightOffset) - (transform.forward * closestDistanceToPlayer));
            WallCheck();
        }
        else
        {
            pitchLock = false;
        }

        if (!pitchLock)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSmoothTime * Time.deltaTime);
        }
        else
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch = pitchMinMax.y;
            currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSmoothTime * Time.deltaTime);
        }

        transform.eulerAngles = currentRotation;
        Vector3 e = transform.eulerAngles;
        e.x = 0;
        CharacterControllerTransform.eulerAngles = e;
    }

    private void WallCheck()
    {
        Ray ray = new Ray(CharacterControllerTransform.position, -CharacterControllerTransform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.1f, out hit, 0.5f, collisionMask))
        {
            pitchLock = true;
        }
        else
        {
            pitchLock = false;
        }
    }

    private void CollisionCheck(Vector3 retPoint)
    {
        RaycastHit hit;

        if (Physics.Linecast(CharacterControllerTransform.position, retPoint, out hit, collisionMask))
        {
            Vector3 norm = hit.normal * wallPush;
            Vector3 p = hit.point + norm;
            if (!(Vector3.Distance(Vector3.Lerp(transform.position, p, moveSpeed * Time.deltaTime), CharacterControllerTransform.position) <= evenCloserDistanceToPlayer))
            {
                transform.position = Vector3.Lerp(transform.position, p, moveSpeed * Time.deltaTime);
            }
            return;
        }

        transform.position = Vector3.Lerp(transform.position, retPoint, returnSpeed * Time.deltaTime);
        pitchLock = false;
    }
}