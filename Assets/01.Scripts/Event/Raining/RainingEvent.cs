using System.Collections;
using UnityEngine;

public class RainingEvent : Event
{
    [SerializeField] private GameObject thunder;
    [SerializeField] private Light _light;
    [SerializeField] private GameObject rain;
    [SerializeField] private AudioClip AudioClip;
    [SerializeField] private AudioClip sea;

    [SerializeField] private float intensityValue;
    private float _defaultIntensity;

    private Transform playerTrm;
    private Coroutine _coroutine;

    private float RandomTime;

    private void Awake()
    {
        _defaultIntensity = _light.intensity;
        playerTrm = PlayerManager.Instance.PlayerTrm;
    }

    private void Update()
    {
        if (rain.activeInHierarchy)
        {
            rain.transform.position = playerTrm.position;
            RandomTime -= Time.deltaTime;

            if (RandomTime <= 0)
                endTrigger = true;
        }
    }

    private IEnumerator RainingRoutine()
    {
        RandomTime = Random.Range(60, 120);
        VolumeManager.Instance.SkyChange(true);
        yield return new WaitForSeconds(2f);
        rain.SetActive(true);
        SoundManager.Instance.PlayBGM(AudioClip);

        while (true)
        {
            int random = Random.Range(5, 10);
            yield return new WaitForSeconds(random);
            Vector3 randomPos = new Vector3(playerTrm.position.x + Random.Range(-10, 10), 10,
                playerTrm.position.z + Random.Range(-10, 10));
            Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 100);

            thunder.transform.position = hit.point;
            thunder.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            _light.intensity = intensityValue;
            yield return new WaitForSeconds(0.2f);
            _light.intensity = _defaultIntensity;
            thunder.SetActive(false);
            yield return null;
        }
    }

    protected override void EventStartMethod()
    {
        _coroutine = StartCoroutine(RainingRoutine());
    }

    protected override void EventStopMethod()
    {
        StopCoroutine(_coroutine);
        rain.SetActive(false);
        SoundManager.Instance.PlayBGM(sea);
        thunder.SetActive(false);
        VolumeManager.Instance.SkyChange(false);
        _light.intensity = _defaultIntensity;
    }
}