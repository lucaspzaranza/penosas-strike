using UnityEngine;

public class AnimationDestroyer : MonoBehaviour 
{		
	public void DestroyAnimation()
	{		
		Destroy(this.gameObject);
	}

	public void ReturnAnimationToPool()
	{
		var gObj = gameObject;
		ObjectPooler.Instance.ReturnToPool(ref gObj);
	}	
}