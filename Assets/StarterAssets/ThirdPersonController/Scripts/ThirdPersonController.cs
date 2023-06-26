using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;
using UnityEngine.UI;
using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviourPunCallbacks, IPunObservable
    {

        public float xvalue;
        public float yvalue;

        public FixedJoystick movementJoystick;
        public FixedJoystick cameraMovementJoystic;

        [Space(30)]
        public GameObject courtYardObj;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("Door Animation")]
        public AnimationClip doorOpenAnimation;
        public AnimationClip doorCloseAnimation;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDWaving;
        private int _animIDLaughing;
        private int _animIDDancing;
        private int _animIDBackfliping;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        public Animator _animator;
        private CharacterController _controller;
        [HideInInspector] public StarterAssetsInputs _input;
        private Camera _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        public bool istomove = true;


        [DllImport("__Internal")]
        private static extern bool IsMobile();
        [DllImport("__Internal")]
        private static extern bool IsMobileModified();

        public bool isMobile()
        {

#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
#endif
            return false;
        }

        public bool IsMobileCheck()
        {

#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobileModified();
#endif
            return false;
        }

        bool isUsingReadyPlayer = false;
        public void setAnimator(Animator animator)
        {
            isUsingReadyPlayer = true;
            _animator = animator;
            _hasAnimator = false;
        }

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }

            isUsingReadyPlayer = true;
            _hasAnimator = false;
        }
        GameObject gmm;
        private void OnEnable()
        {
            if (!photonView.IsMine)
            {
                transform.name = photonView.Owner.NickName;
                transform.GetChild(2).GetComponent<Canvas>().worldCamera = Camera.main;

                gmm = transform.GetChild(2).GetChild(0).gameObject;
                gmm.GetComponent<TextMeshProUGUI>().text = transform.name;

                ConstraintSource cn = new ConstraintSource();
                cn.sourceTransform = Camera.main.transform;
                cn.weight = 1;
                gmm.GetComponent<LookAtConstraint>().AddSource(cn);
                gmm.GetComponent<LookAtConstraint>().constraintActive = true;
            }
        }

        void playAnimation(int animID)
        {
            if(this.photonView.IsMine)
                StartCoroutine(PlayAndWaitForAnim(_animator, animID));
        }

        bool isOtherAnimPlaying = false;

        public IEnumerator PlayAndWaitForAnim(Animator targetAnim, int animIDSate)
        {
            //if (targetAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            //    yield break;

            isOtherAnimPlaying = true;
            targetAnim.SetBool(animIDSate, true);
            while ((targetAnim.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.9f)
            {
                yield return null;
            }
            //yield return new WaitForSeconds(0.2f);
            _animator.SetBool(animIDSate, false);
            if (animIDSate == _animIDDancing)
                isPlayerDancing = false;
            else if (animIDSate == _animIDLaughing)
                isPlayerLaughing = false;
            else if (animIDSate == _animIDWaving)
                isPlayerWaving = false;
            else if (animIDSate == _animIDBackfliping)
                isPlayerBackfliping = false;
            _input.jump = false;
            isOtherAnimPlaying = false;

        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            AssignAnimationIDs();
            if (!photonView.IsMine)
            {   
                return;
            }
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

          
            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            UIhandler.instance.danceBtn.onClick.AddListener(delegate
            {
                playAnimation(_animIDDancing);
                isPlayerDancing = true;
            });
            UIhandler.instance.laughBtn.onClick.AddListener(delegate
            {
                playAnimation(_animIDLaughing);
                isPlayerLaughing = true;
            });
            UIhandler.instance.waveBtn.onClick.AddListener(delegate
            {
                playAnimation(_animIDWaving);
                isPlayerWaving = true;
            });
            UIhandler.instance.backflipBtn.onClick.AddListener(delegate
            {
                playAnimation(_animIDBackfliping);
                isPlayerBackfliping = true;
            });
        }

        private void Update()
        {
            if (isSceneLoding)
            {
                return;
            }
            if (!isOtherAnimPlaying)
            {

                // _hasAnimator = TryGetComponent(out _animator);
                if (!photonView.IsMine)
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                    _animator.SetBool(_animIDGrounded, Grounded);
                    _animator.SetBool(_animIDJump, isPlayerJumped);
                    _animator.SetBool(_animIDFreeFall, isPlayerFreeFall);
                    _animator.SetBool(_animIDWaving, isPlayerWaving);
                    _animator.SetBool(_animIDLaughing, isPlayerLaughing);
                    _animator.SetBool(_animIDDancing, isPlayerDancing);
                    _animator.SetBool(_animIDBackfliping, isPlayerBackfliping);
                    //_controller.Move(targetDirectionForPlayer.normalized * _speed * Time.deltaTime);
                    return;
                }
                else if(transform.name != api.nickName)
                {
                    return;
                }
                JumpAndGravity();
                GroundedCheck();
                Move();
            }
        }
        Ray ray;
        RaycastHit hit;
        GameObject otherChar;
        private void checkForPrivateChat()
        {
            ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    if (!hit.transform.GetComponent<PhotonView>().IsMine)
                    {
                        Debug.Log("________" + hit.transform.gameObject.name);
                        PhotonChatManager.PhotonChatManagerInst.playerNameTxtOnPrvtChat.text = "Do you want to chat with " + hit.transform.name;
                        PhotonChatManager.PhotonChatManagerInst.currentPlayerName = hit.transform.name;
                        PhotonChatManager.PhotonChatManagerInst.oneToOneChatDialog.SetActive(true);
                        otherChar = hit.transform.gameObject;
                    }
                }
            }
        }


        private void LateUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            else if(transform.name != api.nickName)
            {
                return;
            }
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDWaving = Animator.StringToHash("isWave");
            _animIDDancing = Animator.StringToHash("isDance");
            _animIDLaughing = Animator.StringToHash("isLaughing");
            _animIDBackfliping = Animator.StringToHash("isBackflip");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        Vector2 inputLook = new Vector2();
        private void CameraRotation()
        {
            inputLook = Vector2.zero;
            if (IsMobileCheck())
            {
                if (Input.GetMouseButton(0))
                {
                    xvalue = cameraMovementJoystic.Horizontal * Time.deltaTime * 25f;//Input.GetAxis("Mouse X");
                    yvalue = cameraMovementJoystic.Vertical;//Input.GetAxis("Mouse Y");
                    inputLook = new Vector2(xvalue, 0f);
                    //transform.eulerAngles += 3 * new Vector3(0, xvalue, 0);
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    xvalue = Input.GetAxis("Mouse X") * Time.deltaTime * 25F;
                    yvalue = Input.GetAxis("Mouse Y");
                    inputLook = new Vector2(xvalue, 0f);
                    //transform.eulerAngles += 2 * new Vector3(0, xvalue, 0);
                }
            }


            //#if UNITY_ANDROID || UNITY_IOS
            //            //if (Input.GetMouseButton(0))
            //            //{
            //            xvalue = cameraMovementJoystic.Horizontal / 40;//Input.GetAxis("Mouse X");
            //            yvalue = cameraMovementJoystic.Vertical;//Input.GetAxis("Mouse Y");
            //                transform.eulerAngles += 3 * new Vector3(0, xvalue, 0);
            //            //}
            //#else
            //            if (Input.GetMouseButton(0))
            //            {
            //                xvalue = Input.GetAxis("Mouse X");
            //                yvalue = Input.GetAxis("Mouse Y");
            //                transform.eulerAngles += 2 * new Vector3(0, xvalue, 0);
            //            }
            //#endif


            // if there is an input and camera position is not fixed
            //if (inputLook.sqrMagnitude >= _threshold && !LockCameraPosition)
            //{
            //    //Don't multiply mouse input by Time.deltaTime;
            //    float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            //    _cinemachineTargetYaw += inputLook.x * deltaTimeMultiplier;
            //    _cinemachineTargetPitch += inputLook.y * deltaTimeMultiplier;
            //}
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _cinemachineTargetYaw += inputLook.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += inputLook.y * deltaTimeMultiplier;
            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            float currentHorizontalSpeed = 0;
            Vector3 inputDirection = Vector3.zero;

            //#if UNITY_ANDROID || UNITY_IOS
            //if (movementJoystick != null && movementJoystick.Horizontal == 0 && movementJoystick.Vertical == 0) targetSpeed = 0.0f;
            //currentHorizontalSpeed = new Vector3(movementJoystick.Horizontal, 0.0f, movementJoystick.Vertical).magnitude;
            //inputDirection = new Vector3(movementJoystick.Horizontal, 0.0f, movementJoystick.Vertical).normalized;
            //#else
            //if (_input.move == Vector2.zero) targetSpeed = 0.0f;
            //currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            //inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            //#endif

            if (istomove)
            {
                if (IsMobileCheck())
                {
                    if (movementJoystick != null && movementJoystick.Horizontal == 0 && movementJoystick.Vertical == 0) targetSpeed = 0.0f;
                    currentHorizontalSpeed = new Vector3(movementJoystick.Horizontal, 0.0f, movementJoystick.Vertical).magnitude;
                    inputDirection = new Vector3(movementJoystick.Horizontal, 0.0f, movementJoystick.Vertical).normalized;
                }
                else
                {
                    if (_input.move == Vector2.zero) targetSpeed = 0.0f;
                    currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
                    inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
                }
            }


            // a reference to the players current horizontal velocity
            //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    //Debug.Log("vertical - " + movementJoystick.Vertical + "Horizontal - " + movementJoystick.Horizontal);
            //    currentHorizontalSpeed = new Vector3(movementJoystick.Horizontal, 0.0f, movementJoystick.Vertical).magnitude;
            //}
            //else
            //{
            //currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            //}

            //Debug.Log("vertical - " + movementJoystick.Vertical + "Horizontal - " + movementJoystick.Horizontal);
            //currentHorizontalSpeed = new Vector3(movementJoystick.Horizontal, 0.0f, movementJoystick.Vertical).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            //            Debug.Log("input megnitude - " + inputMagnitude);
            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed

                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            if (istomove)
            {
                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;
            }

            // normalise input direction
            //inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (istomove)
            {
                if (IsMobileCheck())
                {
                    if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
                    {
                        _targetRotation = Mathf.Atan2(movementJoystick.Horizontal, movementJoystick.Vertical) * Mathf.Rad2Deg +
                                          _mainCamera.transform.eulerAngles.y;
                        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                            RotationSmoothTime);

                        // rotate to face input direction relative to camera position
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }
                }
                else
                {
                    if (_input.move != Vector2.zero)
                    {
                        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                          _mainCamera.transform.eulerAngles.y;
                        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                            RotationSmoothTime);

                        // rotate to face input direction relative to camera position
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }
                }
            }



            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            targetDirectionForPlayer = targetDirection; ;
            // move the player
            if (istomove)
            {
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

            if (isUsingReadyPlayer)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
            }
            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    isPlayerJumped = false;
                    isPlayerFreeFall = false;
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        isPlayerJumped = true;
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        isPlayerFreeFall = false;
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (photonView.IsMine)
            {
                //Debug.Log("Foot ");
                if (animationEvent.animatorClipInfo.weight > 0.5f)
                {
                    if (FootstepAudioClips.Length > 0)
                    {
                        var index = Random.Range(0, FootstepAudioClips.Length);
                        //AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                    }
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                //AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        bool isScene_CurrentlyLoaded(string sceneName_no_extention)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i) {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName_no_extention) {
                    //the scene is already loaded
                    return true;
                }
            }
            return false;
        }

        bool isinitiated = false;
        void OnTriggerEnter(Collider other)
        {
            if (photonView.IsMine)
            {
                switch (other.name)
                {
                    case "Tutorial1":
                        if (isinitiated)
                            return;
                        isinitiated = true;
                        if (!isScene_CurrentlyLoaded("OnBoarding")) {
                            SceneManager.LoadSceneAsync("OnBoarding", LoadSceneMode.Additive);
                        }
                        break;
                    //case "Tutorial2":
                    //    if (!isScene_CurrentlyLoaded("Dome")) {
                    //        //SceneManager.LoadSceneAsync("Dome", LoadSceneMode.Additive);
                    //    }
                    //    break;
                    //case "Tutorial3":
                    //    if (!isScene_CurrentlyLoaded("Courtyard")) {
                    //        //SceneManager.LoadSceneAsync("Courtyard", LoadSceneMode.Additive);
                    //    }
                    //    break;
                    case "DoorOpenColliderObj":
                        DoorEvent(other.transform.parent.name);
                        //doorAnimation(other.gameObject, true);
                        //npcWaveAnimation(other.transform.parent);
                        break;
                }
            }
        }

        //void OnTriggerExit(Collider other)
        //{
        //    if (photonView.IsMine)
        //    {
        //        switch (other.name)
        //        {
        //            case "Tutorial1":
        //                //SceneManager.UnloadSceneAsync("OnBoarding");
        //                break;

        //            case "Tutorial2":
        //                //SceneManager.UnloadSceneAsync("Dome");
        //                break;

        //            case "Tutorial3":
        //                //SceneManager.UnloadSceneAsync("Courtyard");
        //                break;
        //            case "DoorOpenColliderObj":
        //                //doorAnimation(other.gameObject, false);
        //                break;
        //        }
        //    }
        //}

        bool isSceneLoding = false;
        public void DoorEvent(string doorName)
        {
            if(isSceneLoding)
            {
                return;
            }
            isSceneLoding = true;

            UIhandler.instance.characterDialogPanelObj.SetActive(false);
            UIhandler.instance.npcMainPanelObj.SetActive(false);
            
            UIhandler.instance.loadingTextLoadingScreenObj.text = "Please wait while loading . . .";
            if(doorName == "DoorOnboarding")
            {
                UIhandler.instance.loadingPanelScreenObj.GetComponent<Image>().color = new Color(0.101960f, 0.388235f, 0.282352f);
                UIhandler.instance.loadingPanelScreenObj.SetActive(true);
                SceneManager.UnloadSceneAsync("OnBoarding");
                //SceneManager.LoadSceneAsync("Dome", LoadSceneMode.Additive);
                StartCoroutine(SceneLoading("Dome", "OnBoarding", new Vector3(-8.671f, 0f, 7.86f)));
            }
            else if(doorName == "DoorDome1")
            {
                UIhandler.instance.loadingPanelScreenObj.GetComponent<Image>().color = new Color(0.101960f, 0.388235f, 0.282352f);
                UIhandler.instance.loadingPanelScreenObj.SetActive(true);
                SceneManager.UnloadSceneAsync("Dome");
                //SceneManager.LoadSceneAsync("OnBoarding", LoadSceneMode.Additive);
                StartCoroutine(SceneLoading("OnBoarding", "Dome", new Vector3(-8.233f, 0f, 20.46f)));
            }
            else if(doorName == "DoorDome2")
            {
                UIhandler.instance.loadingPanelScreenObj.GetComponent<Image>().color = new Color(0.101960f, 0.388235f, 0.282352f);
                UIhandler.instance.loadingPanelScreenObj.SetActive(true);
                SceneManager.UnloadSceneAsync("Dome");
                //SceneManager.LoadSceneAsync("Courtyard", LoadSceneMode.Additive);
                StartCoroutine(SceneLoading("Courtyard", "Dome", new Vector3(17.22f, 0f, -13.02f)));
            }
            else if(doorName == "DoorCourtyard")
            {
                UIhandler.instance.loadingPanelScreenObj.GetComponent<Image>().color = new Color(0.101960f, 0.388235f, 0.282352f);
                UIhandler.instance.loadingPanelScreenObj.SetActive(true);
                SceneManager.UnloadSceneAsync("Courtyard");
                //SceneManager.LoadSceneAsync("Dome", LoadSceneMode.Additive);
                StartCoroutine(SceneLoading("Dome", "Courtyard", new Vector3(-2.5f, 0f, -13.29f)));
            }
            resetPlayerStates(false);
        }

        public IEnumerator SceneLoading(string sceneName, string unloadScene, Vector3 position)
        {
            while (isScene_CurrentlyLoaded(unloadScene))
            {
                yield return null;
            }
            isSceneLoded = false;
            sceneToCheck = sceneName;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!isSceneLoded)
            {
                yield return null;
            }
            UIhandler.instance.playerArmatureObj.transform.position = position;
            yield return new WaitForSeconds(2f);
            resetPlayerStates(false);
            UIhandler.instance.loadingPanelScreenObj.SetActive(false);
            UIhandler.instance.loadingPanelScreenObj.GetComponent<Image>().color = new Color(0.101960f, 0.388235f, 0.282352f, 0.7843f);
            isSceneLoding = false;
        }

        string sceneToCheck = "";
        bool isSceneLoded = false;
        void OnSceneLoaded(Scene sceneName, LoadSceneMode mode)
        {
            if(sceneToCheck == sceneName.name)
            {
                isSceneLoded = true;
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void resetPlayerStates(bool isMoveToGround)
        {
            isPlayerJumped = false;
            isPlayerFreeFall = false;
            _speed = 0;
            Grounded = true;
            _input.jump = false;
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _verticalVelocity = -2f;
            _animationBlend = 0;
            _animator.SetFloat(_animIDSpeed, 0);
            _animator.SetBool(_animIDGrounded, true);
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);
            _controller.Move(Vector3.zero);
            if (isMoveToGround)
            {
                StartCoroutine(movePlayerToGround());
            }
        }

        public IEnumerator movePlayerToGround()
        {
            Vector3 velocity = new Vector3(0f, -2f, 0f);
            yield return null;
            while (true)
            {
                GroundedCheck();   
                if (Grounded)
                {
                    break;
                }
                yield return null;
                _controller.Move(velocity);
            }
        }

        //
        void npcWaveAnimation(Transform obj)
        {
            Debug.Log("doorname " + obj.name);
            if (obj.name == "DoorOnboarding")
            {
                GameObject gm = GameObject.Find("Npc1");
                if (gm != null)
                {
                    Debug.Log("npc1 not null found");
                    Animator anim = gm.GetComponent<Animator>();
                    anim.SetInteger("wave", 1);
                    StartCoroutine(setValueInAnimator(anim, "wave", 0));
                }
            }
        }

        IEnumerator setValueInAnimator(Animator animatorObj, string animPatameter, int value)
        {
            yield return new WaitForSeconds(0.5f);
            animatorObj.SetInteger(animPatameter, value);
        }
        //

        #region DoorAnimation
        public void doorAnimation(GameObject doorCollider, bool toOpen)
        {
            Animator doorAnimator = doorCollider.transform.parent.GetComponent<Animator>();
            if (toOpen)
            {
                doorAnimator.SetBool("isClose", false);
                doorAnimator.SetBool("isOpen", true);
            }
            else
            {
                doorAnimator.SetBool("isClose", true);
                doorAnimator.SetBool("isOpen", false);
            }
        }
        #endregion

        bool isPlayerJumped = false;
        bool isPlayerFreeFall = false;
        bool isPlayerWaving = false;
        bool isPlayerLaughing = false;
        bool isPlayerDancing = false;
        bool isPlayerBackfliping = false;
        Vector3 targetDirectionForPlayer = Vector3.zero;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //  throw new System.NotImplementedException();
            if (stream.IsWriting)
            {
                stream.SendNext(_animationBlend);
                stream.SendNext(Grounded);
                stream.SendNext(isPlayerFreeFall);
                stream.SendNext(isPlayerJumped);
                stream.SendNext(isPlayerWaving);
                stream.SendNext(isPlayerLaughing);
                stream.SendNext(isPlayerDancing);
                stream.SendNext(isPlayerBackfliping);
            }
            else
            {
                _animationBlend = (float)stream.ReceiveNext();
                Grounded = (bool)stream.ReceiveNext();
                isPlayerFreeFall = (bool)stream.ReceiveNext();
                isPlayerJumped = (bool)stream.ReceiveNext();
                isPlayerWaving = (bool)stream.ReceiveNext();
                isPlayerLaughing = (bool)stream.ReceiveNext();
                isPlayerDancing = (bool)stream.ReceiveNext();
                isPlayerBackfliping = (bool)stream.ReceiveNext();
            }
        }
    
        #region
        /*
        GameObject otherChar;
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (this.photonView.IsMine)
            {
                if (hit.gameObject.CompareTag("Player") && transform.name != hit.gameObject.transform.name)
                {
                        Debug.Log("________" + hit.gameObject.name);
                        PhotonChatManager.PhotonChatManagerInst.playerNameTxtOnPrvtChat.text = "Do you want to chat with " + hit.transform.name;
                        PhotonChatManager.PhotonChatManagerInst.currentPlayerName = hit.transform.name;
                        PhotonChatManager.PhotonChatManagerInst.oneToOneChatDialog.SetActive(true);
                        otherChar = hit.gameObject;
                        InvokeRepeating("checkDist", 0.4f, 0.4f);
                    
                }
            }
        }
        void checkDist()
        {
            if (otherChar)
            {
                float dist = Vector3.Distance(transform.position, otherChar.transform.position);
                if (dist >= 1)
                {
                    CancelInvoke("checkDist");
                    PhotonChatManager.PhotonChatManagerInst.oneToOneChatDialog.SetActive(false);
                }
            }
        }
        */
    }
    #endregion
}