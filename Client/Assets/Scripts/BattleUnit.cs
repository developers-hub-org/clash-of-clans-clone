namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.TextCore.Text;

    public class BattleUnit : MonoBehaviour
    {

        public Data.UnitID id = Data.UnitID.barbarian;
        private Vector3 lastPosition = Vector3.zero;
        private int i = -1; public int index { get { return i; } }
        private long _id = 0; public long databaseID { get { return _id; } }
        [HideInInspector] public UI_Bar healthBar = null;
        [HideInInspector] public Data.Unit data = null;
        [HideInInspector] public Vector3 positionOffset = Vector3.zero;
        [HideInInspector] public Vector3 positionTarget = Vector3.zero;
        [HideInInspector] public Vector3 targetPosition { get { return positionTarget + positionOffset; } }
        [HideInInspector] public bool moving = false;

        [Header("Weapon")]
        public UI_Projectile projectilePrefab = null;
        public Transform targetPoint = null;
        public Transform shootPoint = null;

        [Header("Movement")]
        [SerializeField] private Transform baseTransform = null;
        [SerializeField] private Transform front = null;
        [SerializeField] private Transform back = null;
        [SerializeField] private Transform right = null;
        [SerializeField] private Transform left = null;
        [SerializeField] private ParticleSystem moveEffect = null;
        [SerializeField] private ParticleSystem transitionEffect = null;

        private bool _inCamp = false;
        private Building _camp = null;

        private void Awake()
        {
            if (moveEffect != null)
            {
                moveEffect.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (healthBar && healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }
        }

        public void Initialize(int index, long id, Data.Unit unit)
        {
            positionOffset.x = UnityEngine.Random.Range(UI_Main.instanse._grid.cellSize * -0.45f, UI_Main.instanse._grid.cellSize * 0.45f);
            positionOffset.y = UnityEngine.Random.Range(UI_Main.instanse._grid.cellSize * -0.45f, UI_Main.instanse._grid.cellSize * 0.45f);
            data = unit;
            _id = id;
            i = index;
            lastPosition = transform.position;
            front.gameObject.SetActive(true);
            back.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
        }

        private void Update()
        {
            bool prevMoving = moving;
            if (transitionEffect != null && moving != prevMoving)
            {
                transitionEffect.Play();
            }
            if (transform.position != targetPosition)
            {
                SetLookDirection(targetPosition);
                transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);
            }
            if (moveEffect != null)
            {
                moveEffect.gameObject.SetActive(moving);
                if (baseTransform != null)
                {
                    baseTransform.gameObject.SetActive(!moving);
                }
            }
            if (moving)
            {
                if (id != Data.UnitID.healer && id != Data.UnitID.dragon && id != Data.UnitID.babydragon && id != Data.UnitID.balloon)
                {
                    // Play Move Animation
                }
                else
                {
                    // Play Fly Animation
                }
            }
            else
            {
                // Play Idle Animation
            }
            lastPosition = transform.position;
        }

        public void Attack(Vector3 position)
        {
            moving = false;
            transform.position = targetPosition;
            SetLookDirection(position);
            Attack();
        }

        public void Attack()
        {
            // Play Attack Animation
        }

        private enum Direction
        {
            front, back, right, left
        }

        private void SetLookDirection(Vector3 target)
        {
            Direction d = Direction.front;
            if(id != Data.UnitID.balloon)
            {
                Vector3 direction = target - transform.position;
                if (direction.y > 0)
                {
                    d = Direction.back;
                    if (direction.x > 0 && direction.x > direction.y)
                    {
                        d = Direction.right;
                    }
                    else if (direction.x < 0 && Mathf.Abs(direction.x) > direction.y)
                    {
                        d = Direction.left;
                    }
                }
                else if (direction.y < 0)
                {
                    if (direction.x > 0 && direction.x > direction.y)
                    {
                        d = Direction.right;
                    }
                    else if (direction.x < 0 && Mathf.Abs(direction.x) > direction.y)
                    {
                        d = Direction.left;
                    }
                }
                else
                {
                    if (direction.x > 0)
                    {
                        d = Direction.right;
                    }
                    else if (direction.x < 0)
                    {
                        d = Direction.left;
                    }
                }
            }
            front.gameObject.SetActive(d == Direction.front);
            back.gameObject.SetActive(d == Direction.back);
            right.gameObject.SetActive(d == Direction.right);
            left.gameObject.SetActive(d == Direction.left);
        }

    }
}