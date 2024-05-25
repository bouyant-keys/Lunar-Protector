using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBase : MonoBehaviour
{
    SFXManager _sfxManager;
    [SerializeField] Animator _animator;
    [SerializeField] Animation _baseHit;

    private void Start()
    {
        _sfxManager = FindObjectOfType<SFXManager>();
    }

    public void PlayAnim(int num)
    {
        StartCoroutine(WaitForAnim(num));
    }

    public void SoundEffect()
    {
        _sfxManager.PlayClip(_sfxManager._baseHit);
    }

    //Literally does not work??? For no reason???? I hate the unity animation system
    private IEnumerator WaitForAnim(int num)
    {
        for (int i = 0; i < num; i++)
        {
            _animator.SetTrigger("Hit");
            yield return new WaitForSeconds(_baseHit.clip.length);
        }
    }
}
