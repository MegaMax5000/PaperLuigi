using UnityEngine;
using System.Collections;

public class HammerScript : MonoBehaviour {

	public GameObject skin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.tag != "Player" && collision.collider.tag != "boundary")
		{
			print ("HIT");
			StartCoroutine (Hit ());
		}
	}

	IEnumerator Hit()
	{
		skin.GetComponent<Animator> ().enabled = false;
		yield return new WaitForSeconds (.3f);
		skin.GetComponent<Animator> ().enabled = true;
		skin.GetComponent<AnimationControl> ().SetAnimationBool ("IDLE", true);
		yield return new WaitForSeconds (.1f);
		skin.GetComponent<AnimationControl> ().SetAnimationBool ("IDLE", false);
	}
}
