using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	static int clicks = 0;
	static int level = 0;
	Vector3 original_scale;
	Color original_color;
	static int max_balls_on_screen = 1;
	static int difficulty = 0;
	int clicks_per_level = 10;
	public GameObject prefab;
	public GUIText max_text;
	public GUIText game_over_text;

	// Use this for initialization
	void Start () {
		original_scale = prefab.transform.localScale;
		original_color = prefab.renderer.material.color;
		max_text.text = "Max Balls: " + max_balls_on_screen;
		transform.position = new Vector3 (Random.Range((float)-6,6),6,0);
	}

	public void AfterDifficultySelection(int diff){
		difficulty = diff;
		if (difficulty == 1) {
			rigidbody.drag = 2;
			prefab.rigidbody.drag = difficulty;
		} else if (difficulty == 2) {
			rigidbody.drag = 1;
			prefab.rigidbody.drag = 1;
		} else {
			rigidbody.drag = (float)0.5;
			prefab.rigidbody.drag = (float)0.5;
		}
		rigidbody.useGravity = true;
		GameObject[] UI_objs = GameObject.FindGameObjectsWithTag ("UI");
		for (int i = 0; i < UI_objs.Length; i++) {
			UI_objs[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (difficulty == 0) {
			return;
		}

		// Shrink between clicks
		if (transform.localScale.x > 1) {
			transform.localScale -= Vector3.one * Time.deltaTime * ((float)level + 1)*difficulty / 10;
			renderer.material.color = new Color(renderer.material.color.r + Time.deltaTime/(float)2, renderer.material.color.g - Time.deltaTime/(float)2, 0);
		}

		// Don't exit viewing area
		if (Mathf.Abs (transform.position.x) > 7) {
			// Uncomment out for continuous playing field
			//transform.position = new Vector3(transform.position.x*-1, transform.position.y, transform.position.z);

			// Reverse velocity in the x direction - ball "bounces off" side walls
			rigidbody.velocity = new Vector3 (rigidbody.velocity.x * -1, rigidbody.velocity.y, rigidbody.velocity.z);
			// Don't get stuck on the edge - shift slightly into the playing area
			transform.position = new Vector3 (transform.position.x - transform.position.x / (float)100, transform.position.y, transform.position.z);
		}

		// Add more balls every n clicks
		if (clicks >= clicks_per_level) {
			level++;
			clicks = 0;
			Instantiate (prefab, new Vector3 (Random.Range ((float)-6, 6), 6, 0), Quaternion.identity);
			max_balls_on_screen = Mathf.Max (level+1, max_balls_on_screen);
			max_text.text = "Max Balls: " + max_balls_on_screen;
		}
	}

	void OnBecameInvisible() {
		if (transform.position.y > 0) {
			return;
		}
		if (GameObject.FindGameObjectsWithTag ("Ball").Length == 1) {
			Instantiate (game_over_text);
		} else {
			level--;
			// Don't keep around balls that fall through floor
			Destroy (this.gameObject);
		}
	}

	void OnMouseDown() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			renderer.material.color =  original_color;
			transform.localScale = original_scale;
			clicks++;
			rigidbody.AddForce(new Vector3((transform.position.x - hitInfo.point.x)*Time.deltaTime*40000,Time.deltaTime*40000,0));
		}
	}
}
