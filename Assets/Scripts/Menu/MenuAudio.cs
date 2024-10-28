using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private bool toggleOn = true;

    [SerializeField] private Animator _animator;
    private float volume;

    private void Start()
    {
        volume = audioSource.volume;
    }
    public void ToggleAudio()
    {
        if (audioSource != null)
        {
            if (toggleOn)
            {
                _animator.SetFloat("Blend", 0);
                audioSource.volume = volume;
                toggleOn = false;
            }
            else
            {
                _animator.SetFloat("Blend", 1);
                audioSource.volume = 0;
                toggleOn = true;
            }
        }
    }
}
