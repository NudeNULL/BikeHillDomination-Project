using UnityEngine;

public class MenuMotionController : MonoBehaviour {

    Animator MenuBikerController;

	// Use this for initialization
	void Start ()
    {
        MenuBikerController = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if (MenuBikerController.GetInteger("MotionNo") != 0 && !MenuBikerController.GetCurrentAnimatorStateInfo(0).IsName("MenuMotion"))
            MenuBikerController.SetInteger("MotionNo", 0);
        else if (Random.Range(0, 400) == 0)
        {
            MenuBikerController.SetInteger("MotionNo", Random.Range(0, 3));
        }
    }
}
