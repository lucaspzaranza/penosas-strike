using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinChangeValidator : MonoBehaviour
{   
    private void OnEnable()
    {
        if(OptionMenuDataSaver.instance.isDefaultSkin)
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
                SceneManager.LoadScene(0);
        }
        else 
        {
            if(SceneManager.GetActiveScene().buildIndex != 1)
                SceneManager.LoadScene(1);
        }
    }
}
