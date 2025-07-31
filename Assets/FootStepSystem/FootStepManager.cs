using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FootstepManager : MonoBehaviour
{
    [System.Serializable]
    public class FootstepSounds
    {
        public FloorType floorType;
        public AudioClip[] footstepClips;
    }

    [System.Serializable]
    public class BackgroundSounds
    {
        public FloorType floorType;
        public AudioClip[] backgroundClips;
        [Range(0f, 1f)]
        public float volume = 0.5f;
        public bool loop = true;
    }

    [Header("Footstep Settings")]
    [SerializeField] private List<FootstepSounds> footstepSoundsList;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private bool preventSameClipTwice = true;
    [SerializeField] private float minTimeBetweenFootsteps = 0.1f;

    [Header("Background Audio Settings")]
    [SerializeField] private List<BackgroundSounds> backgroundSoundsList;
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private float backgroundFadeTime = 1f;
    [SerializeField] private bool enableBackgroundAudio = true;

    [Header("Player Detection")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private float tileCheckInterval = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = true;
    [SerializeField] private bool forcePlayOnStart = false;
    [SerializeField] private FloorType testFloorType = FloorType.Grass;
    [SerializeField] private bool playBackgroundOnStart = true; // ������ǹ��

    // Footstep variables
    private Dictionary<FloorType, FootstepSounds> footstepSoundsDict;
    private Dictionary<FloorType, int> lastClipIndex;
    private float lastPlayTime = -1f;
    private bool isPlayingFootstep = false;

    // Background audio variables
    private Dictionary<FloorType, BackgroundSounds> backgroundSoundsDict;
    private Dictionary<FloorType, int> lastBackgroundClipIndex;
    private FloorType currentFloorType = FloorType.Grass;
    private FloorType previousFloorType = FloorType.Grass;
    private float nextTileCheckTime = 0f;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        LogDebug("=== FootstepManager Initializing ===");
        InitializeFootstepSystem();
        InitializeBackgroundSystem();
    }

    private void Start()
    {
        LogDebug($"=== FootstepManager Started ===");
        LogDebug($"Player Transform: {(playerTransform != null ? playerTransform.name : "NULL")}");
        LogDebug($"Map Manager: {(mapManager != null ? "Found" : "NULL")}");
        LogDebug($"Background Audio Enabled: {enableBackgroundAudio}");
        LogDebug($"Background Sounds Count: {backgroundSoundsList.Count}");

        // ��Ǩ�ͺ���������§ background �ѹ�շ���������
        if (enableBackgroundAudio && playerTransform != null && mapManager != null && playBackgroundOnStart)
        {
            // �� 1 frame ���Ǥ��µ�Ǩ�ͺ tile
            StartCoroutine(InitialTileCheck());
        }

        // ���ͺ������§�ѹ�������������� (����Դ Force Play On Start)
        if (forcePlayOnStart && enableBackgroundAudio)
        {
            LogDebug($"Force playing background audio for: {testFloorType}");
            PlayBackgroundAudio(testFloorType);
        }
    }

    private System.Collections.IEnumerator InitialTileCheck()
    {
        // �� 1 frame �������ء���ҧ���Ѿ����
        yield return null;

        LogDebug("=== Initial Tile Check ===");

        if (playerTransform != null && mapManager != null)
        {
            Vector2 playerPosition = playerTransform.position;
            FloorType detectedFloorType = mapManager.GetCurrentFloorType(playerPosition);

            LogDebug($"Initial player position: {playerPosition}");
            LogDebug($"Initial detected floor: {detectedFloorType}");

            currentFloorType = detectedFloorType;

            // ������§ background �ѹ��
            PlayBackgroundAudio(currentFloorType);
        }
        else
        {
            LogDebug("Cannot perform initial tile check - missing player or map manager");
        }
    }

    private void Update()
    {
        // �ѾഷʶҹС��������§���
        if (footstepAudioSource != null && !footstepAudioSource.isPlaying)
        {
            isPlayingFootstep = false;
        }

        // ��Ǩ�ͺ tile �Ѩ�غѹ�ͧ������
        if (enableBackgroundAudio && Time.time >= nextTileCheckTime)
        {
            CheckCurrentTile();
            nextTileCheckTime = Time.time + tileCheckInterval;
        }
    }

    private void InitializeFootstepSystem()
    {
        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
            footstepAudioSource.playOnAwake = false;
            footstepAudioSource.spatialBlend = 0f;
            LogDebug("Created footstep AudioSource");
        }

        footstepSoundsDict = new Dictionary<FloorType, FootstepSounds>();
        lastClipIndex = new Dictionary<FloorType, int>();

        foreach (var footstepSound in footstepSoundsList)
        {
            footstepSoundsDict[footstepSound.floorType] = footstepSound;
            lastClipIndex[footstepSound.floorType] = -1;
            LogDebug($"Registered footstep sounds for: {footstepSound.floorType}");
        }

        if (footstepSoundsList.Count == 0)
        {
            LogDebug("WARNING: ��������§��ҷ���駤�����!");
        }
    }

    private void InitializeBackgroundSystem()
    {
        if (backgroundAudioSource == null)
        {
            backgroundAudioSource = gameObject.AddComponent<AudioSource>();
            backgroundAudioSource.playOnAwake = false;
            backgroundAudioSource.spatialBlend = 0f;
            backgroundAudioSource.loop = true;
            LogDebug("Created background AudioSource");
        }

        LogDebug($"Background AudioSource settings:");
        LogDebug($"- Volume: {backgroundAudioSource.volume}");
        LogDebug($"- Mute: {backgroundAudioSource.mute}");
        LogDebug($"- Enabled: {backgroundAudioSource.enabled}");

        backgroundSoundsDict = new Dictionary<FloorType, BackgroundSounds>();
        lastBackgroundClipIndex = new Dictionary<FloorType, int>();

        foreach (var backgroundSound in backgroundSoundsList)
        {
            backgroundSoundsDict[backgroundSound.floorType] = backgroundSound;
            lastBackgroundClipIndex[backgroundSound.floorType] = -1;
            LogDebug($"Registered background sounds for: {backgroundSound.floorType} (Clips: {backgroundSound.backgroundClips.Length}, Volume: {backgroundSound.volume})");
        }

        // �Ҽ������ѵ��ѵԶ��������˹�
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                LogDebug($"Auto-found player: {player.name}");
            }
            else
            {
                LogDebug("WARNING: ��辺 Player Transform!");
            }
        }

        // �� MapManager �ѵ��ѵԶ��������˹�
        if (mapManager == null)
        {
            mapManager = FindObjectOfType<MapManager>();
            if (mapManager == null)
            {
                LogDebug("WARNING: ��辺 MapManager!");
            }
            else
            {
                LogDebug("Auto-found MapManager");
            }
        }

        if (backgroundSoundsList.Count == 0)
        {
            LogDebug("WARNING: ��������§ background ����駤�����!");
        }
    }

    private void CheckCurrentTile()
    {
        if (playerTransform == null || mapManager == null)
        {
            LogDebug("Cannot check tile: Player or MapManager is null");
            return;
        }

        Vector2 playerPosition = playerTransform.position;
        FloorType detectedFloorType = mapManager.GetCurrentFloorType(playerPosition);

        LogDebug($"Player position: {playerPosition}, Detected floor: {detectedFloorType}");

        if (detectedFloorType != currentFloorType)
        {
            previousFloorType = currentFloorType;
            currentFloorType = detectedFloorType;
            LogDebug($"Floor changed from {previousFloorType} to {currentFloorType}");
            OnFloorTypeChanged();
        }
    }

    private void OnFloorTypeChanged()
    {
        LogDebug($"FootstepManager: ����¹�ҡ {previousFloorType} �� {currentFloorType}");

        if (enableBackgroundAudio)
        {
            PlayBackgroundAudio(currentFloorType);
        }
        else
        {
            LogDebug("Background audio is disabled");
        }
    }

    private void PlayBackgroundAudio(FloorType floorType)
    {
        LogDebug($"=== Attempting to play background audio for: {floorType} ===");

        if (!backgroundSoundsDict.ContainsKey(floorType))
        {
            LogDebug($"ERROR: ��辺���§ background ����Ѻ��� {floorType}");
            StopBackgroundAudio();
            return;
        }

        BackgroundSounds backgroundSound = backgroundSoundsDict[floorType];
        LogDebug($"Found background sound settings - Clips: {backgroundSound.backgroundClips.Length}, Volume: {backgroundSound.volume}");

        if (backgroundSound.backgroundClips.Length == 0)
        {
            LogDebug($"ERROR: �����������§ background ����Ѻ��� {floorType}");
            StopBackgroundAudio();
            return;
        }

        // ��Ǩ�ͺ AudioClips
        for (int i = 0; i < backgroundSound.backgroundClips.Length; i++)
        {
            if (backgroundSound.backgroundClips[i] == null)
            {
                LogDebug($"WARNING: Background clip {i} is null for {floorType}");
            }
            else
            {
                LogDebug($"Background clip {i}: {backgroundSound.backgroundClips[i].name}");
            }
        }

        // ��ش coroutine ���
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // ���͡���§Ẻ����
        int clipIndex = GetRandomBackgroundClipIndex(floorType, backgroundSound.backgroundClips.Length);
        AudioClip selectedClip = backgroundSound.backgroundClips[clipIndex];

        if (selectedClip == null)
        {
            LogDebug($"ERROR: Selected clip is null (index: {clipIndex})");
            return;
        }

        LogDebug($"Selected clip: {selectedClip.name}, Target volume: {backgroundSound.volume}");

        // ����� fade
        fadeCoroutine = StartCoroutine(FadeToNewBackgroundAudio(selectedClip, backgroundSound.volume, backgroundSound.loop));

        lastBackgroundClipIndex[floorType] = clipIndex;
        LogDebug($"Started fade coroutine for: {selectedClip.name}");
    }

    private System.Collections.IEnumerator FadeToNewBackgroundAudio(AudioClip newClip, float targetVolume, bool loop)
    {
        LogDebug($"=== Starting fade to: {newClip.name} ===");
        float startVolume = backgroundAudioSource.volume;

        // Fade out ���§���
        if (backgroundAudioSource.isPlaying)
        {
            LogDebug("Fading out old audio...");
            for (float t = 0; t < backgroundFadeTime / 2; t += Time.deltaTime)
            {
                backgroundAudioSource.volume = Mathf.Lerp(startVolume, 0, t / (backgroundFadeTime / 2));
                yield return null;
            }
        }

        // ����¹���§����
        backgroundAudioSource.clip = newClip;
        backgroundAudioSource.loop = loop;
        backgroundAudioSource.volume = 0;
        backgroundAudioSource.Play();

        LogDebug($"Started playing new audio: {newClip.name}");
        LogDebug($"AudioSource status - Playing: {backgroundAudioSource.isPlaying}, Clip: {backgroundAudioSource.clip?.name}");

        // Fade in ���§����
        LogDebug("Fading in new audio...");
        for (float t = 0; t < backgroundFadeTime / 2; t += Time.deltaTime)
        {
            backgroundAudioSource.volume = Mathf.Lerp(0, targetVolume, t / (backgroundFadeTime / 2));
            yield return null;
        }

        backgroundAudioSource.volume = targetVolume;
        LogDebug($"Fade complete! Final volume: {backgroundAudioSource.volume}");
        fadeCoroutine = null;
    }

    private int GetRandomBackgroundClipIndex(FloorType floorType, int clipCount)
    {
        if (clipCount <= 1 || !preventSameClipTwice)
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
        while (newIndex == lastBackgroundClipIndex[floorType] && attempts < 10);

        return newIndex;
    }

    // ���ʹ�������Ѻ���§���
    public void PlayFootstep(FloorType floorType)
    {
        float timeSinceLastPlay = Time.time - lastPlayTime;
        if (isPlayingFootstep || timeSinceLastPlay < minTimeBetweenFootsteps)
        {
            return;
        }

        if (!footstepSoundsDict.ContainsKey(floorType))
        {
            LogDebug($"WARNING: ��辺���§����Ѻ��� {floorType}");
            return;
        }

        FootstepSounds footstepSound = footstepSoundsDict[floorType];
        if (footstepSound.footstepClips.Length == 0)
        {
            LogDebug($"WARNING: �����������§����Ѻ��� {floorType}");
            return;
        }

        if (footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }

        int clipIndex = GetRandomClipIndex(floorType, footstepSound.footstepClips.Length);
        AudioClip selectedClip = footstepSound.footstepClips[clipIndex];

        footstepAudioSource.clip = selectedClip;
        footstepAudioSource.Play();

        isPlayingFootstep = true;
        lastPlayTime = Time.time;
        lastClipIndex[floorType] = clipIndex;

        LogDebug($"Playing footstep: {selectedClip.name} for {floorType}");
    }

    private int GetRandomClipIndex(FloorType floorType, int clipCount)
    {
        if (clipCount <= 1 || !preventSameClipTwice)
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
        while (newIndex == lastClipIndex[floorType] && attempts < 10);

        return newIndex;
    }

    // ���ʹ�Ǻ������§
    public void SetFootstepVolume(float volume)
    {
        if (footstepAudioSource != null)
        {
            footstepAudioSource.volume = Mathf.Clamp01(volume);
        }
    }

    public void SetBackgroundVolume(float volume)
    {
        if (backgroundAudioSource != null)
        {
            backgroundAudioSource.volume = Mathf.Clamp01(volume);
            LogDebug($"Set background volume to: {volume}");
        }
    }

    public void StopAllFootsteps()
    {
        if (footstepAudioSource != null && footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
            isPlayingFootstep = false;
        }
    }

    public void StopBackgroundAudio()
    {
        LogDebug("Stopping background audio");
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (backgroundAudioSource != null && backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
    }

    public void EnableBackgroundAudio(bool enable)
    {
        enableBackgroundAudio = enable;
        LogDebug($"Background audio enabled: {enable}");
        if (!enable)
        {
            StopBackgroundAudio();
        }
        else
        {
            CheckCurrentTile();
        }
    }

    public void ForceCheckCurrentTile()
    {
        LogDebug("Force checking current tile...");
        CheckCurrentTile();
    }

    public FloorType GetCurrentFloorType()
    {
        return currentFloorType;
    }

    // ���ʹ���ͺ
    [ContextMenu("Test Play Grass Background")]
    public void TestPlayGrassBackground()
    {
        LogDebug("=== MANUAL TEST: Playing Grass Background ===");
        PlayBackgroundAudio(FloorType.Grass);
    }

    [ContextMenu("Test Play Wood Background")]
    public void TestPlayWoodBackground()
    {
        LogDebug("=== MANUAL TEST: Playing Wood Background ===");
        PlayBackgroundAudio(FloorType.Wood);
    }

    [ContextMenu("Start Background Audio Now")]
    public void StartBackgroundAudioNow()
    {
        LogDebug("=== MANUAL START: Starting background audio now ===");
        if (playerTransform != null && mapManager != null)
        {
            Vector2 playerPosition = playerTransform.position;
            FloorType detectedFloorType = mapManager.GetCurrentFloorType(playerPosition);
            currentFloorType = detectedFloorType;
            PlayBackgroundAudio(currentFloorType);
        }
        else
        {
            LogDebug("Cannot start - missing player or map manager");
        }
    }

    [ContextMenu("Show Audio Status")]
    public void ShowAudioStatus()
    {
        LogDebug("=== AUDIO STATUS ===");
        LogDebug($"Background AudioSource: {(backgroundAudioSource != null ? "EXISTS" : "NULL")}");
        if (backgroundAudioSource != null)
        {
            LogDebug($"- Playing: {backgroundAudioSource.isPlaying}");
            LogDebug($"- Volume: {backgroundAudioSource.volume}");
            LogDebug($"- Mute: {backgroundAudioSource.mute}");
            LogDebug($"- Clip: {(backgroundAudioSource.clip != null ? backgroundAudioSource.clip.name : "NULL")}");
        }
        LogDebug($"Enable Background Audio: {enableBackgroundAudio}");
        LogDebug($"Current Floor Type: {currentFloorType}");
    }

    // ���ʹ����Ѻ���¡�ҡ��¹͡
    public void StartBackgroundMusic()
    {
        if (enableBackgroundAudio && playerTransform != null && mapManager != null)
        {
            StartCoroutine(InitialTileCheck());
        }
    }

    private void LogDebug(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[FootstepManager] {message}");
        }
    }
}