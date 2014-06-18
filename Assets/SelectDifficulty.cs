using UnityEngine;
using System.Collections;

public class SelectDifficulty : MonoBehaviour {

	public Ball sphere;
	void OnMouseDown() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			sphere.gameObject.SetActive(true);
			if (this.name == "Easy") {
				sphere.AfterDifficultySelection(1);
			} else if (this.name == "Normal") {
				sphere.AfterDifficultySelection(2);
			} else {
				sphere.AfterDifficultySelection(3);
			}
		}
	}
}
