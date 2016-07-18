using System.Collections.Generic;

public static class Helper
{
	public static List<GhostReplayManager.Transform> Parsie(this UnityEngine.Transform[] transforms)
	{
		var list = new List<GhostReplayManager.Transform>(transforms.Length);
		foreach (var transform in transforms)
		{
			list.Add(new GhostReplayManager.Transform()
			{
				position = transform.position,
				zAxis = transform.eulerAngles.z
			});
		}

		return list;
	}
}

