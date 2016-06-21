using UnityEngine;

static class Helper
{
	public static void Parsie(this Vector3 v3, Vector2 v2)
	{
		v3.x = v2.x;
		v3.y = v2.y;
		v3.z = 0;
	}
}

