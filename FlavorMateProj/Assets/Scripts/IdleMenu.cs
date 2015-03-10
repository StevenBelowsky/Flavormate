using UnityEngine;
using System.Collections;

public class IdleMenu : Menu {

	[SerializeField]
	private float secondsUntilIdle = 30f;

	public bool isIdle = false;

	void Start () 
	{
		StartTimer();
	}
	
	void Update() {


		if(Input.GetMouseButton(0))
		{
			ResetTimer();
			if(MenuManager.currentMenu.Contains("Idle"))
			{
				MenuManager.ChangeMenu("Juices");
			}
            /* why?
           RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            hits = Physics.RaycastAll(Camera.main.transform.position, ray.direction, 10f);
            int i = 0;
            while (i < hits.Length) {
                RaycastHit hit = hits[i];
                if(hit.transform == transform)
                {
                    ResetTimer();
                    if(MenuManager.currentMenu.Contains("Idle"))
                    {
                        MenuManager.ChangeMenu("Juices");
                    }

                    break;
                }
                i++;
            }
            */
        }

	}

	void OnGUI()
	{
		if(!isActive)
			return;
		GUI.Label(new Rect(Screen.width / 3f, Screen.height / 2, 200f, 200f), "Tap anywhere to begin.");
	}

	void OnMouseDown()
	{
		isIdle = false;
	}

	private void ResetTimer()
	{
		StopTimer();
		StartTimer ();
	}

	private void StartTimer()
	{
		StartCoroutine("IdleTimer");
	}
	private void StopTimer()
	{
		StopCoroutine("IdleTimer");
	}

	private IEnumerator IdleTimer()
	{
		for(int i = 0; i < secondsUntilIdle; i++)
		{

			yield return new WaitForSeconds(1);
		}
		isIdle = true;
		MenuManager.ChangeMenu("IdleMenu");
	}


}
