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

    }

    private void Start()
    {


        // ��Ǩ�ͺ���������§ background �ѹ�շ���������
        if (enableBackgroundAudio && playerTransform != null && mapManager != null && playBackgroundOnStart)
        {
            // �� 1 frame ���Ǥ��µ�Ǩ�ͺ tile
            StartCoroutine(InitialTileCheck());
        }

        // ���ͺ������§�ѹ�������������� (����Դ Force Play On Start)
        if (forcePlayOnStart && enableBackgroundAudio)
        {

            PlayBackgroundAudio(testFloorType);
        }
    }

    private System.Collections.IEnumerator InitialTileCheck()
    {
        // �� 1 frame �������ء���ҧ���Ѿ����
        yield return null;



        if (playerTransform != null && mapManager != null)
        {
            Vector2 playerPosition = playerTransform.position;
            FloorType detectedFloorType = mapManager.GetCurrentFloorType(playerPosition);
            currentFloorType = detectedFloorType;

            // ������§ background �ѹ��
            PlayBackgroundAudio(currentFloorType);
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

        }

        footstepSoundsDict = new Dictionary<FloorType, FootstepSounds>();
        lastClipIndex = new Dictionary<FloorType, int>();

        foreach (var footstepSound in footstepSoundsList)
        {
            footstepSoundsDict[footstepSound.floorType] = footstepSound;
            lastClipIndex[footstepSound.floorType] = -1;

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

        }



        backgroundSoundsDict = new Dictionary<FloorType, BackgroundSounds>();
        lastBackgroundClipIndex = new Dictionary<FloorType, int>();

        foreach (var backgroundSound in backgroundSoundsList)
        {
            backgroundSoundsDict[backgroundSound.floorType] = backgroundSound;
            lastBackgroundClipIndex[backgroundSound.floorType] = -1;

        }

        // �Ҽ������ѵ��ѵԶ��������˹�
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;

            }
            else
            {

            }
        }

        // �� MapManager �ѵ��ѵԶ��������˹�
        if (mapManager == null)
        {
            mapManager = FindObjectOfType<MapManager>();
            if (mapManager == null)
            {
                
            }
            else
            {
               
            }
        }

        if (backgroundSoundsList.Count == 0)
        {
            
        }
    }

    private void CheckCurrentTile()
    {
        if (playerTransform == null || mapManager == null)
        {
            
            return;
        }

        Vector2 playerPosition = playerTransform.position;
        FloorType detectedFloorType = mapManager.GetCurrentFloorType(playerPosition);

        

        if (detectedFloorType != currentFloorType)
        {
            previousFloorType = currentFloorType;
            currentFloorType = detectedFloorType;
            
            OnFloorTypeChanged();
        }
    }

    private void OnFloorTypeChanged()
    {
       

        if (enableBackgroundAudio)
        {
            PlayBackgroundAudio(currentFloorType);
        }
        else
        {
           
        }
    }

    private void PlayBackgroundAudio(FloorType floorType)
    {
      

        if (!backgroundSoundsDict.ContainsKey(floorType))
        {
        
            StopBackgroundAudio();
            return;
        }

        BackgroundSounds backgroundSound = backgroundSoundsDict[floorType];
    

        if (backgroundSound.backgroundClips.Length == 0)
        {
      
            StopBackgroundAudio();
            return;
        }

        // ��Ǩ�ͺ AudioClips
        for (int i = 0; i < backgroundSound.backgroundClips.Length; i++)
        {
            if (backgroundSound.backgroundClips[i] == null)
            {

            }
            else
            {

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

            return;
        }



        // ����� fade
        fadeCoroutine = StartCoroutine(FadeToNewBackgroundAudio(selectedClip, backgroundSound.volume, backgroundSound.loop));

        lastBackgroundClipIndex[floorType] = clipIndex;

    }

    private System.Collections.IEnumerator FadeToNewBackgroundAudio(AudioClip newClip, float targetVolume, bool loop)
    {

        float startVolume = backgroundAudioSource.volume;

        // Fade out ���§���
        if (backgroundAudioSource.isPlaying)
        {
            
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


        // Fade in ���§����
       
        for (float t = 0; t < backgroundFadeTime / 2; t += Time.deltaTime)
        {
            backgroundAudioSource.volume = Mathf.Lerp(0, targetVolume, t / (backgroundFadeTime / 2));
            yield return null;
        }

        backgroundAudioSource.volume = targetVolume;
      
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

            return;
        }

        FootstepSounds footstepSound = footstepSoundsDict[floorType];
        if (footstepSound.footstepClips.Length == 0)
        {

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
       
        PlayBackgroundAudio(FloorType.Grass);
    }

    [ContextMenu("Test Play Wood Background")]
    public void TestPlayWoodBackground()
    {
       
        PlayBackgroundAudio(FloorType.Wood);
    }

    [ContextMenu("Start Background Audio Now")]
    public void StartBackgroundAudioNow()
    {
      
        if (playerTransform != null && mapManager != null)
        {
            Vector2 playerPosition = playerTransform.position;
            FloorType detectedFloorType = mapManager.GetCurrentFloorType(playerPosition);
            currentFloorType = detectedFloorType;
            PlayBackgroundAudio(currentFloorType);
        }
        else
        {
           
        }
    }

    [ContextMenu("Show Audio Status")]
    public void ShowAudioStatus()
    {
      
        if (backgroundAudioSource != null)
        {
  
       
        }
      
    }

    // ���ʹ����Ѻ���¡�ҡ��¹͡
    public void StartBackgroundMusic()
    {
        if (enableBackgroundAudio && playerTransform != null && mapManager != null)
        {
            StartCoroutine(InitialTileCheck());
        }
    }


}