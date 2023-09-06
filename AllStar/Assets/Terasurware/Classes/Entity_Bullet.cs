using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Bullet : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int index;
		public string name;
		public float damage;
		public int bulletCount;
	}
}