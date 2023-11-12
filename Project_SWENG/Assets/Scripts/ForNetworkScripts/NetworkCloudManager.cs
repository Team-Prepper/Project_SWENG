using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkCloudManager : MonoBehaviour
{
    [Header("Network")]
    private PhotonView _PhotonView;

    [SerializeField] List<GameObject> clouds = new List<GameObject>();

    public Dictionary<HexCoordinate, GameObject> cloudBox = new Dictionary<HexCoordinate, GameObject>();

    List<HexCoordinate> closeIndex = new List<HexCoordinate>();

    protected void Awake()
    {
        _PhotonView = GetComponent<PhotonView>();
        //NetworkGridMaker.EventConvertMaterials += CreatCloudHandler;
        CreatCloudAtNetwork();
    }

    private void CreatCloudAtNetwork()
    {
        _PhotonView.RPC("CreatCloud", RpcTarget.All, null);
    }

    private void CreatCloudHandler(object sender, EventArgs e)
    {
        _PhotonView.RPC("CreatCloud", RpcTarget.All, null);
    }

    [PunRPC]
    private void CreatCloud()
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
        Debug.Log(hexCoordinate + "Remove Cloud");
        foreach (HexCoordinate cloudNeighbours in HexGrid.Instance.GetNeighboursDoubleFor(hexCoordinate))
        {
            foreach (HexCoordinate cloud in HexGrid.Instance.GetNeighboursFor(cloudNeighbours))
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
