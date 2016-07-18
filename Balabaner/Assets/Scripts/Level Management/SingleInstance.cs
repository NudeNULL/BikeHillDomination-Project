using UnityEngine;
using System.Collections;

public class SingleInstance : MonoBehaviour
{
	public bool firstInstance;

	void Awake()
	{
		if (FindObjectOfType<SingleInstance>().firstInstance)
		{
			Destroy(gameObject);
		}
		else
		{
			firstInstance = true;
			DontDestroyOnLoad(gameObject);
		}
	}
}
