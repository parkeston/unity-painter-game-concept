using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	private static UIManager instance;

	[SerializeField] private GameObject inGameMenu;
	[SerializeField] private KeyCode pauseKey;
	private bool pausedValue;

	//change locked to confined when build
	private bool IsPaused
	{
		get => pausedValue;
		set
		{
			pausedValue = value;
			inGameMenu.SetActive(value);
			Time.timeScale = value ? 0f : 1f;
			Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = value;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			SceneManager.sceneLoaded +=delegate 
			{
				if (SceneManager.GetActiveScene().buildIndex == 0)
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
			};
		}
		else
			Destroy(gameObject);
	}

	public void LoadByIndex(int index)
	{
		SceneManager.LoadScene(index);
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void Update()
	{
		if (Input.GetKeyDown(pauseKey) && SceneManager.GetActiveScene().buildIndex!=0)
			IsPaused = !IsPaused;
	}

	public void Resume()
	{
		IsPaused = false;
	}
}
