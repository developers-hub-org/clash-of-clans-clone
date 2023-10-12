namespace DevelopersHub.ClashOfWhatecer
{
    using System.Buffers.Text;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UIElements;
    using static UnityEngine.GraphicsBuffer;

    public class UI_Projectile : MonoBehaviour
    {

        private float _launchHeight = 1;
        private Transform _target = null;
        private Vector3 _lastKnowPosition = Vector3.zero;
        private Vector3 _start = Vector3.zero;
        private bool active = false;
        private float time = 0;
        private float timer = 0;
        private Vector3 _curvePoint1 = Vector3.zero;
        private Vector3 _curvePoint2 = Vector3.zero;

        private Vector3 movePosition;
        private float playerX;
        private float targetX;
        private float nextX;
        private float dist;
        private float baseY;
        private float _height;

        public void Initialize(Vector3 start, Transform target, float speed, float curveHeight = 0)
        {
            if (target != null)
            {
                _launchHeight = curveHeight;
                float distance = Vector3.Distance(start, target.position);
                time = distance / speed;
                timer = 0;
                _start = start;
                _target = target;
                _lastKnowPosition = _target.position;
                active = true;
                transform.position = _target.position;


                // transform.LookAt(target);
            }
        }

        private void Update()
        {
            if (active)
            {
                timer += Time.deltaTime;
                if(timer > time) { timer = time; }
                if(_target && _target != null)
                {
                    _lastKnowPosition = _target.position;
                }
                if (_launchHeight > 0)
                {
                    playerX = _start.x;
                    targetX = _lastKnowPosition.x;
                    dist = targetX - playerX;
                    nextX = Mathf.Lerp(_start.x, targetX, timer / time);
                    baseY = Mathf.Lerp(_start.y, _lastKnowPosition.y, (nextX - playerX) / dist);
                    _height = _launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                    movePosition = new Vector3(nextX, baseY + _height, transform.position.z);
                    transform.rotation = LookAtTarget(movePosition - transform.position);
                    transform.position = movePosition;
                    if (Vector3.Distance(movePosition, _lastKnowPosition) <= 0.1f)
                    {
                        /*
                        if (_target != null)
                        {
                            _target.TakeDamage(_damage, _splashRange, _slowTime, _slowPercentage);
                            if (_hitEffect != null)
                            {
                                Transform effect = Instantiate(_hitEffect, transform.position, Quaternion.Euler(0, 0, 0)).transform;
                                effect.localScale = Vector3.one;
                            }
                        }
                        else
                        {
                            if (_missEffect != null)
                            {
                                Transform effect = Instantiate(_missEffect, transform.position, Quaternion.Euler(0, 0, 0)).transform;
                                effect.localScale = Vector3.one;
                            }
                        }
                        */
                        Destroy(gameObject);
                    }
                }
                else
                {
                    float t = timer / time;
                    transform.position = Vector3.Lerp(_start, _lastKnowPosition, t);
                    transform.rotation = LookAtTarget(_lastKnowPosition - transform.position);
                    if (transform.position.x == _lastKnowPosition.x && transform.position.y == _lastKnowPosition.y)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        private static Vector3 CubeBezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float r = 1f - t;
            float f0 = r * r * r;
            float f1 = r * r * t * 3;
            float f2 = r * t * t * 3;
            float f3 = t * t * t;
            return new Vector3(f0 * p0.x + f1 * p1.x + f2 * p2.x + f3 * p3.x, f0 * p0.y + f1 * p1.y + f2 * p2.y + f3 * p3.y, f0 * p0.z + f1 * p1.z + f2 * p2.z + f3 * p3.z);
        }

        public static Quaternion LookAtTarget(Vector2 r)
        {
            return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
        }

        public static float GetCutveHeight(Data.BuildingID id)
        {
            switch (id) 
            {
                case Data.BuildingID.mortor: return 1f;
                default : return 0;
            } 
        }

    }
}