using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneLoaderScript : MonoBehaviour {

    private bool loadScene = false;
    [SerializeField]
    private Text LoadingText;
    [SerializeField]
    private Text ImpsKilledText;
    [SerializeField]
    private Text TimesPlayerKilledText;
    public ImpSpawner impSpawner;
    public int mult;
    private bool Begin = false;
    public float min;
    public float max;


    void Start() 
    {
        ImpsKilledText.text = CharacterBehavior.ImpsKilled.ToString();
        TimesPlayerKilledText.text = CharacterBehavior.TimesPlayerDied.ToString();
        if (mult / Mathf.Sqrt(CharacterBehavior.ImpsKilled) > min && mult / Mathf.Sqrt(CharacterBehavior.ImpsKilled) < max) 
        {
            impSpawner.waitTime = mult / Mathf.Sqrt(CharacterBehavior.ImpsKilled);
        }
        else if (mult / Mathf.Sqrt(CharacterBehavior.ImpsKilled) <= min) 
        {
            impSpawner.waitTime = min;
        }
        else if (mult / Mathf.Sqrt(CharacterBehavior.ImpsKilled) >= max) 
        {
            impSpawner.waitTime = max;
        }
       
    }
    void Update()
    {

        // If the player has pressed the space bar and a new scene is not loading yet...
        if (loadScene == false)
        {

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            // ...change the instruction text to read "Loading..."
            LoadingText.text = "Loading...";

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());

        }

        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            LoadingText.color = new Color(LoadingText.color.r, LoadingText.color.g, LoadingText.color.b, Mathf.PingPong(Time.time, 1));
            ImpsKilledText.color = new Color(ImpsKilledText.color.r, ImpsKilledText.color.g, ImpsKilledText.color.b, Mathf.PingPong(Time.time, 1));
            TimesPlayerKilledText.color = new Color(TimesPlayerKilledText.color.r, TimesPlayerKilledText.color.g, TimesPlayerKilledText.color.b, Mathf.PingPong(Time.time, 1));
        }

    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(8);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = Application.LoadLevelAsync(MenuScript.levelNum);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }

}
