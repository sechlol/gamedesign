﻿using UnityEngine;
using System.Collections;

public class SeaHandler : MonoBehaviour {
	
	EditableMesh mesh;
	
	void Start () 
	{
		mesh = new EditableMesh();
		
		mesh.Create();

		Texture2D seaTex = Resources.Load<Texture2D>("seaTest");
		//mesh.SetTexture(seaTex);

		mesh.SetColor(Color.blue);
	}
	
	void Update () 
	{
		
		int i = 0;
		
		float time = Time.time;
		
		while (i < 81) 
		{
			Vector3 position = mesh.GetVertex(i,1); 
			float surfaceY = GetSurfaceY( position.x, time );
			Vector3 newPosition = new Vector3( position.x, surfaceY, position.z );
			
			mesh.SetVertex(i,1, newPosition);

			Vector2 uvSurface = new Vector2( i*0.1f, 1.0f ); 
			Vector2 uvBottom = new Vector2( i*0.1f, 1.0f-surfaceY*0.5f ); 
			mesh.SetUv(i, 1, uvSurface);
			mesh.SetUv(i, 0, uvBottom);

			i++;
		}
		
		mesh.UpdateMesh();
		
	}
	
	
	public float GetSurfaceY( float x, float time)
	{
		//		return 4.0f + 0.8f * Mathf.Sin(time*0.4f + x*0.7f) 
		//			- 0.1f * Mathf.Cos(time*2 + x*2.0f);
		return 4.0f + 0.38f * Mathf.Sin(time*0.4f + x*0.7f) 
			- 0.1f * Mathf.Cos(time*2 + x*2.0f);
		/*return 4.0f + 0.3f * Mathf.Sin(time*0.4f + x*0.7f) 
			- 0.1f * Mathf.Cos(time*0.13f + x*2.0f); */
	}
	
}
