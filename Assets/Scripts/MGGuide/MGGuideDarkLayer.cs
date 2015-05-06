using UnityEngine;
using System.Collections;

public class MGGuideDarkLayer : MonoBehaviour {
    public GameObject dark_mid, dark_other;
    private GameObject dark_midClone, dark_otherUpClone, dark_otherDownClone, dark_otherLeftClone, dark_otherRightClone;
	// Use this for initialization
    public void createAllDarkLayerInPos()
    {
        dark_midClone = GameObject.Instantiate(dark_other, Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
        dark_midClone.GetComponent<MGGuideDarkMidListenr>().isEnable = true;
        Vector3 dark_midCloneSize = dark_midClone.GetComponent<SpriteRenderer>().bounds.size, dark_midCloneScale = dark_midClone.transform.localScale;
        dark_midClone.transform.localScale = new Vector3(dark_midCloneScale.x * (MGGlobalDataCenter.defaultCenter().screenRightX - MGGlobalDataCenter.defaultCenter().screenLiftX / dark_midCloneSize.x),
            dark_midCloneScale.y * ((MGGlobalDataCenter.defaultCenter().screenTopY - MGGlobalDataCenter.defaultCenter().screenBottomY) / dark_midCloneSize.y), 1);
    }
	public void createDarkLayerInPos(Vector3 pos)
    {
        dark_midClone = GameObject.Instantiate(dark_mid, pos, Quaternion.Euler(0, 0, 0)) as GameObject;
        dark_midClone.transform.localScale = new Vector3(1.5f,1.5f,1);
        dark_midClone.GetComponent<MGGuideDarkMidListenr>().isEnable = true;
        Vector3 dark_midCloneSize = dark_midClone.GetComponent<SpriteRenderer>().bounds.size;
        float up = pos.y + dark_midCloneSize.y / 2, down = pos.y - dark_midCloneSize.y / 2, left = pos.x - dark_midCloneSize.x / 2, right = pos.x + dark_midCloneSize.x / 2;

        dark_otherUpClone = GameObject.Instantiate(dark_other, new Vector3(pos.x,up + (MGGlobalDataCenter.defaultCenter().screenTopY - up)/2, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        dark_otherUpClone.name = "dark_otherUpClone";
        dark_otherUpClone.GetComponent<MGGuideDarkMidListenr>().isEnable = false;
        Vector3 dark_otherUpCloneSize = dark_otherUpClone.GetComponent<SpriteRenderer>().bounds.size, dark_otherUpCloneScale = dark_otherUpClone.transform.localScale;
        dark_otherUpClone.transform.localScale = new Vector3(dark_otherUpCloneScale.x * (dark_midCloneSize.x / dark_otherUpCloneSize.x),
            dark_otherUpCloneScale.y * ((MGGlobalDataCenter.defaultCenter().screenTopY - up) / dark_otherUpCloneSize.y), 1);

        dark_otherDownClone = GameObject.Instantiate(dark_other, new Vector3(pos.x,down + (MGGlobalDataCenter.defaultCenter().screenBottomY - down) / 2, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        dark_otherDownClone.name = "dark_otherDownClone";
        dark_otherDownClone.GetComponent<MGGuideDarkMidListenr>().isEnable = false;
        Vector3 dark_otherDownCloneSize = dark_otherDownClone.GetComponent<SpriteRenderer>().bounds.size, dark_otherDownCloneScale = dark_otherDownClone.transform.localScale;
        dark_otherDownClone.transform.localScale = new Vector3(dark_otherDownCloneScale.x * (dark_midCloneSize.x / dark_otherDownCloneSize.x),
            dark_otherDownCloneScale.y * ((MGGlobalDataCenter.defaultCenter().screenBottomY - down) / dark_otherDownCloneSize.y), 1);

        dark_otherLeftClone = GameObject.Instantiate(dark_other, new Vector3(left + (MGGlobalDataCenter.defaultCenter().screenLiftX - right) / 2,0,0), Quaternion.Euler(0, 0, 0)) as GameObject;
        dark_otherLeftClone.name = "dark_otherLeftClone";
        dark_otherLeftClone.GetComponent<MGGuideDarkMidListenr>().isEnable = false;
        Vector3 dark_otherLeftCloneSize = dark_otherLeftClone.GetComponent<SpriteRenderer>().bounds.size, dark_otherLeftCloneScale = dark_otherLeftClone.transform.localScale;
        dark_otherLeftClone.transform.localScale = new Vector3(dark_otherLeftCloneScale.x * ((MGGlobalDataCenter.defaultCenter().screenLiftX - right) / dark_otherLeftCloneSize.x),
            dark_otherLeftCloneScale.y * ((MGGlobalDataCenter.defaultCenter().screenTopY - MGGlobalDataCenter.defaultCenter().screenBottomY) / dark_otherLeftCloneSize.y), 1);

        dark_otherRightClone = GameObject.Instantiate(dark_other, new Vector3(right + (MGGlobalDataCenter.defaultCenter().screenRightX - left) / 2, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        dark_otherRightClone.name = "dark_otherRightClone";
        dark_otherRightClone.GetComponent<MGGuideDarkMidListenr>().isEnable = false;
        Vector3 dark_otherRightCloneSize = dark_otherRightClone.GetComponent<SpriteRenderer>().bounds.size, dark_otherRightCloneScale = dark_otherRightClone.transform.localScale;
        dark_otherRightClone.transform.localScale = new Vector3(dark_otherRightCloneScale.x * ((MGGlobalDataCenter.defaultCenter().screenRightX - left) / dark_otherRightCloneSize.x),
            dark_otherRightCloneScale.y * ((MGGlobalDataCenter.defaultCenter().screenTopY - MGGlobalDataCenter.defaultCenter().screenBottomY) / dark_otherRightCloneSize.y), 1);

    }
    public void destoryDarkLayer()
    {
        Debug.Log("destoryDarkLayer");
        if (dark_midClone)
            Destroy(dark_midClone);
        if (dark_otherUpClone)
            Destroy(dark_otherUpClone);
        if (dark_otherDownClone)
            Destroy(dark_otherDownClone);
        if (dark_otherLeftClone)
            Destroy(dark_otherLeftClone);
        if (dark_otherRightClone)
            Destroy(dark_otherRightClone);
    }
}
