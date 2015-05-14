using UnityEngine;
using System.Collections;

public class sloganScript : MonoBehaviour
{
    public Camera NGUICamera;
    public GameObject sloganItem;
    public string text;
    public int charsPerSecond;
	// Use this for initialization
    private int lines,numOfOneLine=12;
    private GameObject[] sloganItemArray;
    private float timer;
    private int index;
    private bool isClicked;
	void Start () {
        isClicked = false;
        index = 0;
        timer = (float)numOfOneLine / (float)charsPerSecond;
        this.gameObject.layer = 10;
        lines = (int)Mathf.Ceil((float)text.Length / (float)numOfOneLine);
        if (lines == 0) return;
        sloganItemArray = new GameObject[lines];
        float screenRight = MGGlobalDataCenter.defaultCenter().screenRightX, screenTop = MGGlobalDataCenter.defaultCenter().screenTopY;
        Vector3 sloganSize=Vector3.zero,boxSize=Vector3.zero;
        float firstDis = 0, eachDis = 0, firstDisPiex = 0, eachDisPiex = 0;
        for (int i = 0; i < lines; ++i)
        {
            Debug.Log(i);
            
            sloganItemArray[i] = GameObject.Instantiate(sloganItem, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            sloganItemArray[i].SetActive(false);
            sloganItemArray[i].GetComponent<UILabel>().text = text.Substring(Mathf.Min(i * numOfOneLine, text.Length - 1), ((i + 1) * numOfOneLine > text.Length ? text.Length - i * numOfOneLine : numOfOneLine));
            firstDisPiex =  0.5f * sloganItemArray[i].GetComponent<UILabel>().width;
            eachDisPiex = 1.5f * sloganItemArray[i].GetComponent<UILabel>().width;
            sloganSize = piexlToNGUI(sloganItemArray[i].GetComponent<UILabel>().width, sloganItemArray[i].GetComponent<UILabel>().height);
            firstDis = 0.5f * sloganSize.x;
            eachDis = 1.5f * sloganSize.x;
            if (i == 0)
            {
                sloganItemArray[index].SetActive(true);
                this.GetComponent<UILabel>().width = (int)(firstDisPiex + eachDisPiex * (lines - 1) + sloganItemArray[i].GetComponent<UILabel>().width / 2);
                this.GetComponent<UILabel>().height = sloganItemArray[index].GetComponent<UILabel>().height;
                boxSize = piexlToNGUI(this.GetComponent<UILabel>().width, this.GetComponent<UILabel>().height);
                transform.position = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, 0);
            }
            sloganItemArray[i].transform.parent = transform;
            sloganItemArray[i].layer = 10;
            sloganItemArray[i].transform.position = new Vector3(transform.position.x + boxSize.x / 2 - i * eachDis - firstDis, transform.position.y + boxSize.y / 2 - sloganSize.y / 2, 0);
            sloganItemArray[i].transform.localScale = new Vector3(1, 1, 1);
            sloganItemArray[i].GetComponent<TypewriterEffect>().charsPerSecond = charsPerSecond;
            
        }
        InvokeRepeating("nextSlogan",timer+0.1f,timer);
	}
    Vector3 piexlToNGUI(float x, float y)
    {
        Vector3 pos = MGFoundtion.pixelToWroldPoint(x, y);
        return MGFoundtion.WorldPointToNGUIPoint(new Vector3(pos.x - MGGlobalDataCenter.defaultCenter().screenLiftX, pos.y - MGGlobalDataCenter.defaultCenter().screenBottomY), NGUICamera);
    }
    void nextSlogan()
    {
        ++index;
        if (index >= lines)
        {
            CancelInvoke("nextSlogan");
        }
        else
        {
            sloganItemArray[index].SetActive(true);
            sloganItemArray[index].GetComponent<TypewriterEffect>().charsPerSecond = charsPerSecond;
        }
    }
    public void onClick()
    {
        if (isClicked) return;
        isClicked = true;
        print("OnSloganClick");
        CancelInvoke("nextSlogan");
        for (int i = 0; i < lines; ++i)
        {
            sloganItemArray[i].SetActive(true);
            sloganItemArray[i].GetComponent<TypewriterEffect>().charsPerSecond = 20000;
        }
    }
	
}
