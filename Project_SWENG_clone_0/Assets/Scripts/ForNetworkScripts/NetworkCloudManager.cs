using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkCloudManager : MonoSingleton<NetworkCloudManager>
{

    [SerializeField] List<GameObject> clouds = new List<GameObject>();

    public Dictionary<Vector3Int, GameObject> cloudBox = new Dictionary<Vector3Int, GameObject>();

    List<Vector3Int> closeIndex = new List<Vector3Int>();

    protected override void OnCreate()
    {
        NetworkGridMaker.EventSetNavComplete += CreatCloud;
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

    public void CloudActiveFalse(Vector3Int hexCoordinate)
    {
        foreach (Vector3Int cloudNeighbours in HexGrid.Instance.GetNeighboursDoubleFor(hexCoordinate))
        {
            foreach (Vector3Int cloud in HexGrid.Instance.GetNeighboursFor(cloudNeighbours))
            {
                if (!closeIndex.Contains(cloud))
                {
                    closeIndex.Add(cloud);
                    StartCoroutine(ActiveFalseCo(cloud));
                }
            }

        }
    }

    IEnumerator ActiveFalseCo(Vector3Int index)
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
