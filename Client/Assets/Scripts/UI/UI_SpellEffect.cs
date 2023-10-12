namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UI_SpellEffect : MonoBehaviour
    {

        private Data.SpellID _id = Data.SpellID.lightning;
        private long _databaseID = 0; public long DatabaseID { get { return _databaseID; } }
        // [SerializeField] private SpriteRenderer _renderer = null;

        [Header("Effects")]
        [SerializeField] private GameObject lightnignPulse = null;
        [SerializeField] private GameObject lightnignEffect = null;
        [SerializeField] private GameObject healingEffect = null;
        [SerializeField] private GameObject rageEffect = null;
        [SerializeField] private GameObject freezeEffect = null;
        [SerializeField] private GameObject invisibilityEffect = null;
        /*
        [Header("Colors")]
        [SerializeField] private Color lightningColor;
        [SerializeField] private Color healingColor;
        [SerializeField] private Color rageColor;
        [SerializeField] private Color freezeColor;
        [SerializeField] private Color invisibilityColor;
        */
        public void Initialize(Data.SpellID id, long databaseID, float radius)
        {
            if (healingEffect != null) { healingEffect.SetActive(id == Data.SpellID.healing); }
            if (invisibilityEffect != null) { invisibilityEffect.SetActive(id == Data.SpellID.invisibility); }
            if (rageEffect != null) { rageEffect.SetActive(id == Data.SpellID.rage); }
            if (freezeEffect != null) { freezeEffect.SetActive(id == Data.SpellID.freeze); }
            if (lightnignEffect != null) { lightnignEffect.SetActive(id == Data.SpellID.lightning); }
            /*
            switch (id)
            {
                case Data.SpellID.lightning:
                    _renderer.color = lightningColor;
                    break;
                case Data.SpellID.healing:
                    _renderer.color = healingColor;
                    break;
                case Data.SpellID.rage:
                    _renderer.color = rageColor;
                    break;
                case Data.SpellID.jump:

                    break;
                case Data.SpellID.freeze:
                    _renderer.color = freezeColor;
                    break;
                case Data.SpellID.invisibility:
                    _renderer.color = invisibilityColor;
                    break;
                case Data.SpellID.recall:

                    break;
                case Data.SpellID.earthquake:

                    break;
                case Data.SpellID.haste:

                    break;
                case Data.SpellID.skeleton:

                    break;
                case Data.SpellID.bat:

                    break;
            }
            */
            _id = id;
            _databaseID = databaseID;
            Vector3 scale = transform.localScale;
            scale.x = radius;
            scale.y = radius;
            transform.localScale = scale;
        }

        public void Pulse()
        {
            switch (_id)
            {
                case Data.SpellID.lightning:
                    if (lightnignPulse != null)
                    {
                        GameObject go = Instantiate(lightnignPulse, transform.position, Quaternion.identity);
                        Destroy(go, 1f);
                    }
                    break;
                case Data.SpellID.healing:

                    break;
                case Data.SpellID.rage:

                    break;
                case Data.SpellID.jump:

                    break;
                case Data.SpellID.freeze:

                    break;
                case Data.SpellID.invisibility:

                    break;
                case Data.SpellID.recall:

                    break;
                case Data.SpellID.earthquake:

                    break;
                case Data.SpellID.haste:

                    break;
                case Data.SpellID.skeleton:

                    break;
                case Data.SpellID.bat:

                    break;
            }
        }

        public void End()
        {
            Destroy(gameObject);
        }

    }
}