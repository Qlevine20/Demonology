using UnityEngine;
using System.Collections;

public class UnscaleTime : MonoBehaviour 
{
     // Update is called once per frame
    private ParticleSystem psys;
    void Start() 
    {
        psys = GetComponentInChildren<ParticleSystem>();
    }
     void Update()
     {
         if (Time.timeScale < 0.01f)
         {
             psys.Simulate(Time.unscaledDeltaTime, true, false);
         }
     }
 }

