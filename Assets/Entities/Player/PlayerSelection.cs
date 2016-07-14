using UnityEngine;
using System.Collections;

public class PlayerSelection : MonoBehaviour {

	public float speed;
	public Color selectedColor;
	public Color cancelColor;

	private SpriteRenderer sprite;

	void Awake(){
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = selectedColor;
	}

	void Update () {
		transform.RotateAround(transform.position, new Vector3(0,0,1), speed * Time.deltaTime);
	}

	public void SetActive(bool active){
		if(sprite == null){
			sprite = GetComponent<SpriteRenderer>();
		}
		sprite.enabled = active;
	}

	public void SetCanceled(){
		sprite.color = cancelColor;
	}

	public void SetSelected(){
		sprite.color = selectedColor;
	}

}
