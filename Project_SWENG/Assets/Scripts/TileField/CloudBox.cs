using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudBox : MonoBehaviour
{
    public static CloudBox Instance;
    [SerializeField] List<GameObject> clouds = new List<GameObject>();

    public Dictionary<HexCoordinate, GameObject> cloudBox = new Dictionary<HexCoordinate, GameObject>();

    List<HexCoordinate> closeIndex = new List<HexCoordinate>();

    private void Awake()
    {
        Instance = this;
    }

    private void CreatCloud(object sender, EventArgs e)
    {

        foreach (var tile in HexGrid.Instance.hexTileDict.Keys)
        {
            
            cloudBox.Add(tile, Instantiate(clouds[Random.Range(0, clouds.Count)]));

            cloudBox.TryGetValue(tile, out GameObject cloud);
            HexGrid.Instance.hexTileDict.TryGetValue(tile, out Hex cloudPos);

            cloud.transform.position = cloudPos.gameObject.transform.position + new Vector3(0f, 3f, 0f);
            cloud.transform.SetParent(gameObject.transform);
            
        }
    }

    public void CloudActiveFalse(HexCoordinate hexCoordinate)
    {
        foreach (HexCoordinate cloudNeighbours in HexGrid.Instance.GetNeighboursDoubleFor(hexCoordinate))
        {
            foreach(HexCoordinate cloud in HexGrid.Instance.GetNeighboursFor(cloudNeighbours))
            {
                if (!closeIndex.Contains(cloud))
                {
                    closeIndex.Add(cloud);
                    StartCoroutine(ActiveFalseCo(cloud));
                }
            }
            
        }     
    }

    IEnumerator ActiveFalseCo(HexCoordinate index)
    {
        for (int i = 0; i < 10; i++)
        {
            cloudBox[index].transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
        cloudBox[index].SetActive(false);
        yield break;
    }
}
