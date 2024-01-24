using System.Collections;
using FishNet.Object;
using UnityEngine;

namespace FantasyRobbery.Scripts
{
    public class NetworkPlayer : NetworkBehaviour, IInteractor
    {
        #region SerializedFields

        [Header("Technical")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform viewAnchor;
        [SerializeField] private Camera firstPersonCamera;
        [SerializeField] private AudioListener audioListener;

        [Header("Sprint options")]
        [SerializeField] private bool canSprint = true;
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        
        [Header("Jump options")]
        [SerializeField] private bool canJump = true;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;

        [Header("Interaction options")]
        [SerializeField] private float interactionDistance = 7f;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        
        [Header("Movement parameters")]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float sprintSpeed = 6f;
        [SerializeField] private float crouchSpeed = 1.5f;
        
        [Header("Look parameters")]
        [SerializeField] private float lookSpeedX = 2;
        [SerializeField] private float lookSpeedY = 2;
        [SerializeField] private float upperLookLimit = 80;
        [SerializeField] private float lowerLookLimit = 80;

        [Header("Jump parameters")]
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = 30f;

        [Header("Crouch parameters")]
        [SerializeField] private bool canCrouch = true;
        [SerializeField] private float crouchHeight = 0.5f;
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float timeToCrouch = 0.25f;
        [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
        [SerializeField] private Vector3 standingCenter = Vector3.zero;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

        #endregion

        #region Private

        private bool _isCrouching;
        private bool _duringCrouchAnimation;
        private Transform _cachedTransform;
        private Transform _cachedCameraTransform;
        private float _rotationX = 0;
        private Vector3 _moveDirection;
        private Vector2 _currentInput;
        private Vector3 _interactionViewPoint = new Vector3(0.5f, 0.5f, 0);
        private IInteractable _currentInteractable;

        #endregion

        #region Fields

        public bool CanMove { get; set; } = true;
        public bool CanInteract { get; set; } = true;
        public bool IsSprinting => canSprint && Input.GetKey(sprintKey);
        private bool ShouldJump => Input.GetKeyDown(jumpKey) && controller.isGrounded;
        private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !_duringCrouchAnimation && controller.isGrounded;

        #endregion

        public override void OnStartClient()
        {
            if (!IsOwner)
            {
                firstPersonCamera.enabled = false;
                audioListener.enabled = false;
                enabled = false;
            }
        }

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _cachedTransform = transform;
            _cachedCameraTransform = firstPersonCamera.transform;
        }

        private float GetSpeed()
        {
            return _isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : moveSpeed;
        }

        private void HandleMoveInput()
        {
            _currentInput = new Vector2(GetSpeed() * Input.GetAxis("Vertical"), GetSpeed() * Input.GetAxis("Horizontal"));
            float moveDirectionY = _moveDirection.y;
            _moveDirection = (_cachedTransform.TransformDirection(Vector3.forward) * _currentInput.x) + (_cachedTransform.TransformDirection(Vector3.right) * _currentInput.y);
            _moveDirection.y = moveDirectionY;
        }

        private void HandleLookInput()
        {
            _rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, -upperLookLimit, lowerLookLimit);
            _cachedCameraTransform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            _cachedTransform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
        }

        private void ApplyMovements()
        {
            if (!controller.isGrounded)
                _moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(_moveDirection * Time.deltaTime);
        }

        private void HandleJump()
        {
            if (ShouldJump)
                _moveDirection.y = jumpForce;
        }

        private IEnumerator Crouch()
        {
            if (_isCrouching && Physics.Raycast(_cachedCameraTransform.position, Vector3.up, 1f))
                yield break;

            _duringCrouchAnimation = true;

            float timeElapsed = 0f;
            float targetHeight = _isCrouching ? standingHeight : crouchHeight;
            float currentHeight = controller.height;
            Vector3 targetCenter = _isCrouching ? standingCenter : crouchingCenter;
            Vector3 currentCenter = controller.center;

            while (timeElapsed < timeToCrouch)
            {
                controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
                controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            controller.height = targetHeight;
            controller.center = targetCenter;

            _isCrouching = !_isCrouching;
            
            _duringCrouchAnimation = false;
        }

        private void HandleCrouch()
        {
            if (ShouldCrouch)
                StartCoroutine(nameof(Crouch));
        }

        private void HandleInteractCheck()
        {
            if (Physics.Raycast(firstPersonCamera.ViewportPointToRay(_interactionViewPoint), out var hit, interactionDistance))
            {
                if (_currentInteractable == null || hit.collider.gameObject.GetInstanceID() != _currentInteractable.InstanceId)
                {
                    _currentInteractable?.OnUnfocus();
                    hit.collider.TryGetComponent(out _currentInteractable);
                    _currentInteractable?.OnFocus();
                }
            }
            else
            {
                _currentInteractable?.OnUnfocus();
                _currentInteractable = null;
            }
        }

        private void HandleInteractInput()
        {
            if (Input.GetKeyDown(interactionKey))
                _currentInteractable?.OnInteractionStart(this);
            if (Input.GetKeyUp(interactionKey))
                _currentInteractable?.OnInteractionEnd(this);
        }
        
        private void Update()
        {
            if (!CanMove)
                return;
            
            HandleMoveInput();
            HandleLookInput();
            
            if (canJump)
                HandleJump();
            
            if (canCrouch)
                HandleCrouch();

            if (CanInteract)
            {
                HandleInteractCheck();
                HandleInteractInput();
            }
            
            ApplyMovements();
        }
    }
}