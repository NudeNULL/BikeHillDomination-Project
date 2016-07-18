using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GhostReplayManager : MonoBehaviour
{
	public class Transform
	{
		public Vector2 position;
		public float zAxis;
	}

	public class Frame
	{
		public List<Transform> transforms;
	}

	public bool isFirstInstance = false;

	string currentLevel = string.Empty;

	int currentFrame;

	void Awake()
	{
		if (!isFirstInstance)
		{
			GhostReplayManager GRM = FindObjectOfType<GhostReplayManager>();
			if (GRM.isFirstInstance)
			{
				GRM.SetTransform(ghostTransforms, playerTransforms);
				Destroy(gameObject);
			}
			else
			{
				isFirstInstance = true;
				DontDestroyOnLoad(gameObject);
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		currentFrame = 0;

		if (currentLevel != SceneManager.GetActiveScene().name)
		{
			currentLevel = SceneManager.GetActiveScene().name;
			frames = new List<Frame>(Constants.FRAMES_DEFAULT_LIST_CAPACITY);
		}
	}

	void FixedUpdate()
	{
		if (currentFrame < frames.Count)
		{
			for (var i = 0; i < frames[currentFrame].transforms.Count; i++)
			{
				var transform = frames[currentFrame].transforms[i];
				ghostTransforms[i].position = transform.position;
				ghostTransforms[i].eulerAngles = new Vector3(ghostTransforms[i].eulerAngles.x, ghostTransforms[i].eulerAngles.y, transform.zAxis);
				transform.position = playerTransforms[i].position;
				transform.zAxis = playerTransforms[i].eulerAngles.z;
			}
		}
		else
		{
			frames.Add(new Frame() { transforms = playerTransforms.Parsie() });
		}

		currentFrame++;
	}

	public void SetTransform(UnityEngine.Transform[] ghostTransforms, UnityEngine.Transform[] playerTransforms)
	{
		this.ghostTransforms = ghostTransforms;
		this.playerTransforms = playerTransforms;
		currentFrame = 0;
	}

	public void RemoveFramesTail()
	{
		frames.RemoveRange(currentFrame, frames.Count - currentFrame);
	}

	public static List<Frame> frames;

	public UnityEngine.Transform[] ghostTransforms;
	public UnityEngine.Transform[] playerTransforms;

}
