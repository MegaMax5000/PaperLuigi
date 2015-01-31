using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetAnimationBool(string var, bool val)
	{
		anim.SetBool (var, val);
	}

	public bool GetAnimationBool(string var)
	{
		return anim.GetBool (var);
	}
}
