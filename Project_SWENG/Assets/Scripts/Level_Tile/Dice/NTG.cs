using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NTG : MonoBehaviour
{
    [SerializeField] GameObject[] Nums;

    float moveSpeed = 2f;
    float scaleSpeed = 5f;

    public void convert(int value)
    {
        GameObject convertedNum = Nums[value];

        StartCoroutine(SpawnMoveScale(convertedNum));
    }

    private IEnumerator SpawnMoveScale(GameObject prefab)
    {
        GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        spawnedObject.transform.LookAt(Camera.main.transform);
        Vector3 targetPosition = spawnedObject.transform.position + new Vector3(0f, 2f, 0f);
        Vector3 targetScale = new Vector3(5f, 5f, 5f);

        while (Vector3.Distance(spawnedObject.transform.position, targetPosition) > 0.01f)
        {
            spawnedObject.transform.position = Vector3.MoveTowards(spawnedObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            spawnedObject.transform.localScale = Vector3.Lerp(spawnedObject.transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(distoryObj(spawnedObject));
    }

    IEnumerator distoryObj(GameObject GO)
    {
        yield return new WaitForSeconds(1f);
        Destroy(GO);
    }
}
