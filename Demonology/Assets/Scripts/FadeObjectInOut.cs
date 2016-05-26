using UnityEngine;
using System.Collections;

public class FadeObjectInOut : MonoBehaviour
{
	// store colours
	private Color[] colors;
	
	// check the alpha value of most opaque object
	float MaxAlpha()
	{
		float maxAlpha = 0.0f; 
		Renderer[] rendererObjects = GetComponents<Renderer>(); 
		foreach (Renderer item in rendererObjects)
		{
			//maxAlpha = Mathf.Max (maxAlpha, item.material.color.a); 
			maxAlpha = 255f;
		}
		return maxAlpha; 
	}
	
	// fade sequence
	IEnumerator FadeSequence (float fadingOutTime)
	{
		// log fading direction, then precalculate fading speed as a multiplier
		bool fadingOut = (fadingOutTime < 0.0f);
		float fadingOutSpeed = 1.0f / fadingOutTime; 
		
		// grab all child objects
		Renderer[] rendererObjects = GetComponents<Renderer>(); 
		if (colors == null)
		{
			//create a cache of colors if necessary
			colors = new Color[rendererObjects.Length]; 
			
			// store the original colours for all child objects
			for (int i = 0; i < rendererObjects.Length; i++)
			{
				colors[i] = rendererObjects[i].material.color; 
			}
		}
		
		// make all objects visible
		for (int i = 0; i < rendererObjects.Length; i++)
		{
			rendererObjects[i].enabled = true;
		}
		
		
		// get current max alpha
		float alphaValue = MaxAlpha();
		
		// iterate to change alpha value 
		while ( (alphaValue >= 0.0f && fadingOut) || (alphaValue <= 1.0f && !fadingOut)) 
		{
			alphaValue += Time.deltaTime * fadingOutSpeed; 
			
			for (int i = 0; i < rendererObjects.Length; i++)
			{
				Color newColor = (colors != null ? colors[i] : rendererObjects[i].material.color);
				newColor.a = Mathf.Min ( newColor.a, alphaValue ); 
				newColor.a = Mathf.Clamp (newColor.a, 0.0f, 1.0f); 				
				rendererObjects[i].material.SetColor("_Color", newColor) ; 
			}

			ParticleSystem cParts = GetComponent<ParticleSystem> ();
			if (cParts != null) {
				Color newColor = cParts.startColor;
				newColor.a = Mathf.Min ( newColor.a, alphaValue ); 
				newColor.a = Mathf.Clamp (newColor.a, 0.0f, 1.0f); 				
				cParts.startColor = newColor;
				/*ParticleSystem.Particle[] m_Particles;
				int numParticlesAlive = cParts.GetParticles(m_Particles);

				// Change only the particles that are alive
				for (int i = 0; i < numParticlesAlive; i++)
				{
					m_Particles[i].velocity += Vector3.up * m_Drift;
				}*/
			}
			
			yield return null; 
		}
		
		// turn objects off after fading out
		/*if (fadingOut)
		{
			for (int i = 0; i < rendererObjects.Length; i++)
			{
				rendererObjects[i].enabled = false; 
			}
		}*/
	}

	public void FadeIn (float newFadeTime)
	{
		StopAllCoroutines(); 
		StartCoroutine("FadeSequence", newFadeTime); 
	}

	public void FadeOut (float newFadeTime)
	{
		StopAllCoroutines(); 
		StartCoroutine("FadeSequence", -newFadeTime); 
	}
}