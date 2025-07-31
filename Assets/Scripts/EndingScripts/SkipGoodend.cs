using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SkipGoodend : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public List<double> sceneStartTimes = new List<double> { 1.0, 11.0, 21.0, 31.0, 41.0, 54.5 };
    private int currentSceneIndex = 0;
    public string nextSceneName;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSceneIndex++;

            if (currentSceneIndex < sceneStartTimes.Count)
            {
                double targetTime = sceneStartTimes[currentSceneIndex];
                timelineDirector.time = targetTime;
                timelineDirector.Evaluate();
            }
            else
            {
                timelineDirector.time = timelineDirector.duration;
                timelineDirector.Evaluate();
                timelineDirector.Stop();

                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
