using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    IDictionary<Renderer, Material[]> _glowMatDict = new Dictionary<Renderer, Material[]>();
    IDictionary<Renderer, Material[]> _originMatDict = new Dictionary<Renderer, Material[]>();

    IDictionary<Color, Material> _cachedMat = new Dictionary<Color, Material>();

    [SerializeField] SpriteRenderer _sprRenderer;
    public Material glowMaterial;

    private bool isGlowing = false;

    private Color validSpaceColor = Color.green;
    private Color originalGlowColor;
    private Color originalSpriteColor;

    private void Awake()
    {
        //PrepareMaterialDictionaries();
        setDictionaries();
        originalGlowColor = glowMaterial.GetColor("_GlowColor");
        originalSpriteColor = new Color(.2f,.2f,.2f,.4f);
    }

    // ref
    private void PrepareMaterialDictionaries()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originalMaterials = renderer.materials;
            _originMatDict.Add(renderer, originalMaterials);

            Material[] newMaterials = new Material[renderer.materials.Length];

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if (_cachedMat.TryGetValue(originalMaterials[i].color, out mat) == false)
                {
                    mat = new Material(glowMaterial); 
                    //By default, Unity considers a color with the property name name "_Color" to be the main color
                    mat.color = originalMaterials[i].color;
                    _cachedMat[mat.color] = mat;
                }
                newMaterials[i] = mat;
            }
            _glowMatDict.Add(renderer, newMaterials);
        }
    }

    // new selection 
    private void setDictionaries()
    {
        Transform selectObject = transform.Find("Selection");

        if (selectObject == null) return;

        Renderer[] renderers = selectObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] originalMaterials = renderer.materials;
            _originMatDict.Add(renderer, originalMaterials);

            Material[] newMaterials = new Material[renderer.materials.Length];

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if (_cachedMat.TryGetValue(originalMaterials[i].color, out mat) == false)
                {
                    mat = new Material(glowMaterial);
                    //By default, Unity considers a color with the property name name "_Color" to be the main color
                    mat.color = originalMaterials[i].color;
                    _cachedMat[mat.color] = mat;
                }
                newMaterials[i] = mat;
            }
            _glowMatDict.Add(renderer, newMaterials);
        }
    }

    public void SetSprite(Sprite spr, bool isActive)
    {
        _sprRenderer.gameObject.SetActive(isActive);
        _sprRenderer.sprite = spr;

    }

    internal void HighlightValidPath()
    {
        if (isGlowing == false)
            return;

        SetHighlight(0f, 0f, validSpaceColor);
    }

    private void SetHighlight(float dynamic, float power, Color color)
    {

        foreach (Material[] mats in _glowMatDict.Values)
        {
            foreach (Material item in mats)
            {
                item.SetFloat("_DynamicGlow", dynamic);
                item.SetFloat("_GlowPower", power);
                item.SetColor("_GlowColor", color);
            }
        }

    }

    public void ToggleGlow(bool state)
    {
        if (isGlowing == state)
            return;

        isGlowing = state;

        IDictionary<Renderer, Material[]> dict;

        if (isGlowing)
        {
            SetHighlight(1, 1f, originalGlowColor);
            dict = _glowMatDict;
        }
        else
        {
            dict = _originMatDict;
        }

        foreach (Renderer renderer in dict.Keys)
        {
            renderer.materials = dict[renderer];
        }

        Transform selectObject = transform.Find("Selection");
        selectObject.GetComponentInChildren<Renderer>().enabled = state;
    }

    public void OnMouseToggleGlow(bool isOn) // just Cursor on the tile
    {
        GameObject selectObject = transform.Find("OnMouse").gameObject;
        MeshRenderer meshRenderer = selectObject.GetComponent<MeshRenderer>();

        if(meshRenderer == null) return;

        meshRenderer.enabled = isOn;

    }
}
