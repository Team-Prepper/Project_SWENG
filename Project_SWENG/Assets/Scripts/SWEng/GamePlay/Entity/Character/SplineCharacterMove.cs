using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWEng.Data;
using EasyH;

namespace SWEng.GamePlay {
    
    public class SplineCharacterMove : CharacterMoveBase
    {
        [SerializeField] private float _movementDuration;
        [SerializeField] private float _acceleration = 1;

        public override void Move(
            ICharacter character, IList<GridCoord2D> path)
        {
            StartCoroutine(_RotationCoroutine(
                character, path));

        }

        private Vector3 ConvertToVector3(GridCoord2D target)
        {
            return Coord2DManager.Instance.
                Convertor.ConvertToVector3(target);
        }

        private IEnumerator _RotationCoroutine(
            ICharacter character, IList<GridCoord2D> path)
        {
            yield return null;

            character.Animation.PlayAnim("SetBoolTrue", "IsWalk");

            Vector3 startVector = character.transform.forward;
            Vector3 startPos = character.transform.position;

            ISpline spline = new HermiteSpline();


            float timeElapsed = 0;

            for (int i = 0; i < path.Count; i++)
            {
                character.DicePoint.UsePoint(2);

                Vector3 endPos = ConvertToVector3(path[i]);
                Vector3 endVector = endPos - startPos;

                if (i < path.Count - 1)
                {
                    endVector = (ConvertToVector3(path[i + 1])
                        - startPos) * 0.5f;
                }

                endVector *= _acceleration;

                Vector3 beforePos = startPos;

                while (timeElapsed < _movementDuration)
                {
                    timeElapsed += Time.deltaTime;
                    float t = timeElapsed / _movementDuration;

                    float x = spline.Get(startPos.x, endPos.x,
                        startVector.x, endVector.x, t);
                    float z = spline.Get(startPos.z, endPos.z,
                        startVector.z, endVector.z, t);

                    Vector3 nowPos = new Vector3(x, 0, z);

                    character.transform.SetPositionAndRotation
                        (nowPos, Quaternion.LookRotation(
                            nowPos - beforePos, Vector3.up));

                    beforePos = nowPos;

                    yield return null;
                }

                timeElapsed -= _movementDuration;

                character.EntityTransform.Pos = path[i];
                startPos = endPos;
                startVector = endVector;
            }

            character.transform.position =
                ConvertToVector3(path[path.Count - 1]);

            character.Animation.PlayAnim("SetBoolFalse", "IsWalk");
            character.ActionEnd(0);

        }

    }
}