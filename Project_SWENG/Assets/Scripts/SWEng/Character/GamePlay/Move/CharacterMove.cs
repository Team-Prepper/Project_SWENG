using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BKTools.Gaming.GridMap2D;

namespace SWEng {
    public class CharacterMove : CharacterMoveBase
    {

        [SerializeField] private float _movementDuration = 0.5f;
        [SerializeField] private float _moveRotationDuration = 0.4f;

        public override void Move(ICharacter character,
            IList<GridCoord2D> path)
        {
            StartCoroutine(_RotationCoroutine(
                character, path, _moveRotationDuration));

        }

        private IEnumerator _RotationCoroutine(ICharacter character,
            IList<GridCoord2D> path, float rotationDuration)
        {
            character.Animation.PlayAnim("SetBoolTrue", "IsWalk");

            foreach (GridCoord2D targetPos in path)
            {
                if (EntityManager.Instance.
                    GetEntityAt(targetPos) != null) break;

                Vector3 targetPosVector3 = Coord2DManager.Instance.
                    Convertor.ConvertToVector3(targetPos);

                character.DicePoint.UsePoint(2);

                Vector3 startPosition = character.transform.position;
                Vector3 direction = targetPosVector3 - startPosition;

                Quaternion startRotation = character.transform.rotation;
                Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

                float timeElapsed;

                // 회전

                if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
                {
                    timeElapsed = 0;

                    while (timeElapsed < rotationDuration)
                    {
                        timeElapsed += Time.deltaTime;
                        character.transform.rotation =
                            Quaternion.Lerp(startRotation, endRotation, timeElapsed / rotationDuration);

                        yield return null;
                    }

                    character.transform.rotation = endRotation;

                }

                timeElapsed = 0;

                while (timeElapsed < _movementDuration)
                {
                    timeElapsed += Time.deltaTime;
                    character.transform.position = Vector3.Lerp(
                        startPosition, targetPosVector3, timeElapsed / _movementDuration);

                    yield return null;
                }

                character.transform.position = targetPosVector3;
                character.EntityTransform.Pos = targetPos;
            }

            character.Animation.PlayAnim("SetBoolFalse", "IsWalk");
            character.ActionEnd(0);

        }

    }
}