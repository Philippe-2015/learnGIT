using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	
	public float currentHealth = 5;
	public float lifePoints = 0;
	public float damageEffectPause = 0.2f;

	private float deathDelay = 4f;
	public float respawnDelay = 1f;
	public GameObject deathPrefab;

	private string healthText = "Health";
	private string lifePointsText = "LifePoint";
	private string maxHealthText = "MaxHealth";

	// Use this for initialization
	void Start () {
		currentHealth = InventoryManager.inventory.GetItems (healthText);
		lifePoints = InventoryManager.inventory.GetItems (lifePointsText);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > 0)
			lifePoints = InventoryManager.inventory.GetItems (lifePointsText);
	}
	
	void ApplyDamage(float damage){
		
		if (currentHealth > 0){
			currentHealth -= damage;
			
			if (currentHealth < 0){
				currentHealth = 0;
			}
			if(currentHealth == 0){
				lifePoints -= 1;

				if(lifePoints <= 0){
					StartCoroutine(GameOver(deathDelay));
				}else{
					StartCoroutine(RestartScene(respawnDelay));
				}

			}else{
				StartCoroutine(DamageEffect());
				InventoryManager.inventory.SetItems (healthText, currentHealth);
			}
		}
	}
	
	IEnumerator RestartScene(float delay){
		RespawnDeathPrefab();
		yield return new WaitForSeconds (delay);
		currentHealth = InventoryManager.inventory.GetItems (maxHealthText);
		InventoryManager.inventory.SetItems (healthText, currentHealth);
		InventoryManager.inventory.SetItems (lifePointsText, lifePoints);
		InventoryManager.inventory.SetItems ("coin", 0);
		Application.LoadLevel (Application.loadedLevel);
	}

	IEnumerator GameOver(float delay){
		RespawnDeathPrefab();
		yield return new WaitForSeconds (delay);
		Application.LoadLevel (0);
	}

	IEnumerator DamageEffect(){
		GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(damageEffectPause);
		GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(damageEffectPause);
		GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(damageEffectPause);
		GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(damageEffectPause);
	}

	void RespawnDeathPrefab(){
		if (deathPrefab != null) {
			GameObject go = (GameObject) Instantiate(deathPrefab,transform.position,Quaternion.identity);
			go.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
		}
	}

	void OnGUI(){
		if (currentHealth <= 0 && lifePoints <= 0) {
			GUI.Label(new Rect((Screen.width -80)/2,(Screen.height -30)/2, 80, 30), "GAME OVER");
		}

	}

}
