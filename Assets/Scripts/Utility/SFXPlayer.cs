using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance;

    [SerializeField] ObjectPooler _sfxSources;
    [SerializeField] AudioSource _musicPlayer;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnEnable()
    {
    }


    public void PlaySoundEffect(Vector3 pos, AudioClip effect, bool random = true)
    {
        GameObject go = _sfxSources.GetObject();

        AudioSource AS = go.GetComponent<AudioSource>();

        AS.clip = effect;

        if (random)
            AS.pitch = 0.5f + Random.Range(-0.05f, 0.05f);

        go.transform.position = pos;

        AS.Play();

        StartCoroutine(DisableSourcesAfter(AS));
    }

    public void PlayMusic()
    {
        _musicPlayer.Play();
    }

    public void MuteMusic(bool val)
    {
        if (val)
            _musicPlayer.volume = 0f;
        else
            _musicPlayer.volume = 1f;

    }

    IEnumerator DisableSourcesAfter(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length +0.01f);
        source.gameObject.SetActive(false);
    }
}
