using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour, ICharacterComponent {

    ICharacterController _cc;

    [SerializeField] private float _movementDuration = 0.5f;
    [SerializeField] private float _moveRotationDuration = 0.4f;

    public void SetCC(ICharacterController cc) {
        _cc = cc;
    }

    public void TryAddAction(IList<IActionSelector.Action> target) {
        if (_cc.GetPoint() < 2) return;

        target.Add(IActionSelector.Action.Move);
    }

    public void Move(Queue<Vector3> path)
    {
        StartCoroutine(_RotationCoroutine(path, _moveRotationDuration));

    }

    private IEnumerator _RotationCoroutine(Queue<Vector3> path, float rotationDuration)
    {
        _cc.PlayAnim("SetBoolTrue", "IsWalk");

        foreach (Vector3 targetPos in path)
        {
            if (HexGrid.Instance.GetMapUnitAt(targetPos).Entity != null) break;

            _cc.UsePoint(2);

            Vector3 startPosition = _cc.transform.position;
            Vector3 direction = targetPos - startPosition;

            Quaternion startRotation = _cc.transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

            float timeElapsed;

            // È¸Àü

            if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
            {
                timeElapsed = 0;

                while (timeElapsed < rotationDuration)
                {
                    timeElapsed += Time.deltaTime;
                    _cc.transform.rotation =
                        Quaternion.Lerp(startRotation, endRotation, timeElapsed / rotationDuration);

                    yield return null;
                }

                _cc.transform.rotation = endRotation;

            }

            timeElapsed = 0;

            while (timeElapsed < _movementDuration)
            {
                timeElapsed += Time.deltaTime;
                _cc.transform.position = Vector3.Lerp(startPosition, targetPos, timeElapsed / _movementDuration);

                yield return null;
            }
            _cc.transform.position = targetPos;

            _cc.MoveTo(HexCoordinate.ConvertFromVector3(startPosition), HexCoordinate.ConvertFromVector3(targetPos));
        }

        _cc.PlayAnim("SetBoolFalse", "IsWalk");
        _cc.ActionEnd(0);

    }

}