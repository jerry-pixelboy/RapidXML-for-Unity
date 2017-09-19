using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RapidXml;

public class TestRapidXML : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		string content = Resources.Load<TextAsset> ("Armory").text;
		List<ArmoryPo> dataList = XMLUtils.RapidParse<ArmoryPo> (content, "RECORDS");
		foreach (ArmoryPo ap in dataList) {
			Debug.Log (ap.name);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
