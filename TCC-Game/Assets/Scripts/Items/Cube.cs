using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [field: SerializeField] private AudioClip _sfx { get; set; }
    private ActorSFX _actorSFX { get; set; }
    private Rigidbody2D _rig { get; set; }
    private bool _canPlayAudio { get; set; }

    private void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _actorSFX = GetComponent<ActorSFX>();
        _canPlayAudio = true;
    }

    void Update()
    {
        if (!_rig.IsSleeping() && _rig.velocity.magnitude > 0.5)
        {

            if (_canPlayAudio)
            {
                StartCoroutine("PlayAudioDelay");
                _canPlayAudio = false;
            }
        }
    }

    IEnumerator PlayAudioDelay()
    {
        _actorSFX.PlaySFX(_sfx);
        yield return new WaitForSeconds(1);
        _canPlayAudio = true;
    }
}
