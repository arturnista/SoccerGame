using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamDatabase : MonoBehaviour {

	public List<Team> teamDatabase;

	public int NextIndex(int index){
		return (index + 1) % teamDatabase.Count;
	}

	public int PreviousIndex(int index){
		int idx = index - 1;
		if(idx < 0){
			idx = teamDatabase.Count - 1;
		}
		return idx;
	}
}
