using UnityEngine;
using System.Collections;

public class BrowserTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Browser browser = GetComponent<Browser>();
        browser.OpenFile("C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example"
                + "\\1037_C14\\walking6\\Outputs\\");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
