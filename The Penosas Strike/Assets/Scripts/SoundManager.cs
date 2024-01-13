using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
	public static SoundManager instance;
	[Range(0f,1f)]
	public float volume;
	public AudioClip hitSFX;
	public AudioClip lifePlus;
	public AudioClip newRecord;
	public AudioClip insaneMode;
	public AudioClip miss;
	public AudioClip click;
	public AudioClip shot;
	public AudioClip explosion;
	public AudioClip gameOver;
	public AudioClip birdSwoosh;
	public AudioClip bombFall;
	public AudioClip countdown;
	public AudioClip countdownGo;
	public AudioClip chickenDie;
	public AudioClip pointScore;

	private AudioSource audioSource;

	private void Awake() 
	{
		if ( instance == null)
			instance = this;
		else
			Destroy(this.gameObject);

		audioSource = GetComponent<AudioSource>();
	}

	public void PlayAudio(ref AudioClip audio)	
	{		
		audioSource.PlayOneShot(audio, volume);		
	}

	public void PlayChickenDieSound()
	{
		StartCoroutine(ChickenDieSound());
	}

	private IEnumerator ChickenDieSound()
	{
		yield return new WaitForSeconds(0.15f);
		SoundManager.instance.PlayAudio(ref SoundManager.instance.chickenDie);
	}
}
