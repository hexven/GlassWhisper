using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float footstepInterval = 0.7f;
    [SerializeField] private bool preventSameSound = true;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private MapManager mapManager;

    // ���������Ѻ�Ѵ������§���
    private float footstepTimer;
    private bool isMoving;
    private Vector2 lastPosition;
    private int lastClipIndex = -1;

    // ��ͧ�ѹ���§��蹷Ѻ�ѹ
    private bool isPlayingFootstep = false;
    private float minTimeBetweenSteps = 0.5f; // ���Ң�鹵�������ҧ���§
    private float lastFootstepTime = -1f;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        // ��Ǩ�ͺ�������͹����ԧ
        isMoving = Vector2.Distance(transform.position, lastPosition) > 0.01f;
        lastPosition = transform.position;

        // �Ѵ������§��ҵ���������͹���
        HandleFootsteps();

        // ��Ǩ�ͺʶҹ����§
        UpdateAudioStatus();
    }

    private void HandleFootsteps()
    {
        if (isMoving && moveInput.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f && CanPlayFootstep())
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private bool CanPlayFootstep()
    {
        // ��ͧ�ѹ���§��蹷Ѻ�ѹ
        float timeSinceLastStep = Time.time - lastFootstepTime;
        return !isPlayingFootstep && timeSinceLastStep >= minTimeBetweenSteps;
    }

    private void UpdateAudioStatus()
    {
        // �ѾഷʶҹС��������§
        if (audioSource != null && !audioSource.isPlaying)
        {
            isPlayingFootstep = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", !context.canceled);

            if (context.canceled)
            {
                animator.SetFloat("LastInputX", moveInput.x);
                animator.SetFloat("LastInputY", moveInput.y);
            }
        }

        moveInput = context.ReadValue<Vector2>();

        if (animator != null)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
        }
    }

    private void PlayFootstepSound()
    {
        if (mapManager != null && audioSource != null && CanPlayFootstep())
        {
            AudioClip[] currentFloorClips = mapManager.GetCurrentFloorClips(transform.position);

            if (currentFloorClips != null && currentFloorClips.Length > 0)
            {
                // ��ش���§��ҡ�͹ (��Ҩ���)
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                int clipIndex = GetRandomClipIndex(currentFloorClips.Length);
                AudioClip selectedClip = currentFloorClips[clipIndex];

                // ��駤�����§


                // ������§
                audioSource.clip = selectedClip;
                audioSource.Play();

                // �Ѿഷʶҹ�
                isPlayingFootstep = true;
                lastFootstepTime = Time.time;
                lastClipIndex = clipIndex;


            }
        }
    }

    private int GetRandomClipIndex(int clipCount)
    {
        if (clipCount <= 1 || !preventSameSound)
        {
            return Random.Range(0, clipCount);
        }

        int newIndex;
        int attempts = 0;

        do
        {
            newIndex = Random.Range(0, clipCount);
            attempts++;
        }
        while (newIndex == lastClipIndex && attempts < 10);

        return newIndex;
    }

    // ���ʹ����Ѻ Animation Events - ��ͧ�ѹ���§�Ѻ�ѹ
    public void Step()
    {
        // ੾�����¡�ҡ Animation Event ��ҹ��
        if (CanPlayFootstep())
        {
            PlayFootstepSound();
        }
    }

    // ���ʹ����Ѻ��ش���§������
    public void StopFootstepSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlayingFootstep = false;
        }
    }

    // ���ʹ����Ѻ��Ѻ�觡�õ�駤��
    public void SetFootstepInterval(float interval)
    {
        footstepInterval = Mathf.Max(0.1f, interval);
    }

    public void SetMinTimeBetweenSteps(float minTime)
    {
        minTimeBetweenSteps = Mathf.Max(0.05f, minTime);
    }
}