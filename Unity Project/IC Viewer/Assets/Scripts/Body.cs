﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Body : MonoBehaviour
{
    public static Sprite starSprite, blackHoleSprite, pixelSprite;

    public string pack, author, bodyName, bodyClass;
    public float soi;
    public Vector3 position;

    //public GameBody gameBody;
    public DisplaySOI displaySOI;
    public SpriteRenderer spriteRenderer;

    public GameObject planeShadow, planeRay;

    public static List<GameObject> bodies { get; set; }

    public TextMeshProUGUI nameText;

    

    private void Start()
    {
        if (!GameObject.Find("Bodies"))
        {
            GameObject gt = new GameObject("Bodies");
            gt.transform.position = Vector3.zero;
        }
        if (!GameObject.Find("Plane Shadows"))
        {
            GameObject gt = new GameObject("Plane Shadows");
            gt.transform.position = Vector3.zero;
            gt.transform.SetParent(GameObject.Find("Bodies").transform);
        }
        if (!GameObject.Find("Plane Rays"))
        {
            GameObject gt = new GameObject("Plane Rays");
            gt.transform.position = Vector3.zero;
            gt.transform.SetParent(GameObject.Find("Bodies").transform);
        }

        if (gameObject.transform.parent != GameObject.Find("Bodies"))
        {
            gameObject.transform.parent = GameObject.Find("Bodies").transform;
        }

        bodies.Add(gameObject);

        Sprite s;
        Color c;

        ParseSpectralClass(this, out s, out c);

        spriteRenderer.sprite = s;
        spriteRenderer.color = c;


        planeShadow = new GameObject(bodyName + " Plane Shadow");
        planeShadow.transform.SetParent(GameObject.Find("Plane Shadows").transform);

        SpriteRenderer srps = planeShadow.AddComponent<SpriteRenderer>();
        srps.sprite = blackHoleSprite;
        srps.color = ColorManager.secondaryColor;
        srps.sortingOrder = -2;

        planeShadow.transform.position = new Vector3(gameObject.transform.position.x, Camera.main.GetComponent<CameraController>().planeLevel, gameObject.transform.position.z);
        planeShadow.transform.eulerAngles = Vector3.right * 90f;

        //gameBody.shadow = planeShadow; //TO DO: DELETE SHADOW FROM GAMEBODY

        planeRay = new GameObject(bodyName + " Ray");
        planeRay.transform.SetParent(GameObject.Find("Plane Rays").transform);

        SpriteRenderer srpr = planeRay.AddComponent<SpriteRenderer>();
        srpr.sprite = pixelSprite;
        srpr.color = ColorManager.secondaryColor;
        srpr.sortingOrder = -2;

        planeRay.transform.position = new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y - gameObject.transform.position.y) / 2f, gameObject.transform.position.z);
        //gameBody.ray = planeRay; //TO DO: DELETE PLANE RAY FROM GAMEBODY
        planeRay.transform.localScale = new Vector3(3f, (gameObject.transform.position.y - planeShadow.transform.position.y) * 100f, 0);

        GameObject textGO = new GameObject(gameObject.name + " Name Text");
        textGO.transform.parent = GameObject.Find("World Canvas").transform;

        nameText = textGO.AddComponent<TextMeshProUGUI>();
        nameText.text = "   " + gameObject.name;

        Font font = GameObject.Find("Manager").GetComponent<EditorManager>().globalFont;
        //Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        nameText.color = ColorManager.thirdColor; //new Color(0.847f, 0.847f, 0.847f, 1);
        nameText.autoSizeTextContainer = false;
        nameText.fontSize = 14;


        nameText.rectTransform.sizeDelta = new Vector2(100, 15);
        nameText.rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        nameText.rectTransform.pivot = new Vector2(0, 0.5f);

    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        planeRay.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);

        nameText.rectTransform.anchoredPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y);
        nameText.gameObject.transform.position = new Vector3(nameText.gameObject.transform.position.x, nameText.gameObject.transform.position.y, gameObject.transform.position.z);
        nameText.rectTransform.eulerAngles = new Vector3(CameraController.angle.x, CameraController.angle.y, 0);

        planeShadow.transform.position = new Vector3(transform.position.x, Camera.main.GetComponent<CameraController>().planeLevel, transform.position.z);
        planeRay.transform.position = new Vector3(gameObject.transform.position.x, (planeShadow.transform.position.y + gameObject.transform.position.y) / 2f, gameObject.transform.position.z);
        planeRay.transform.localScale = new Vector3(3f, (gameObject.transform.position.y - planeShadow.transform.position.y) * 100f, 0);

        


        if (EditorManager.colorManagerSetter.update)
        {
            nameText.color = ColorManager.thirdColor;

            planeRay.GetComponent<SpriteRenderer>().color = ColorManager.secondaryColor;
            planeShadow.GetComponent<SpriteRenderer>().color = ColorManager.secondaryColor;
        }

        

        //Name / Ray fading from far
        float dname = Vector3.Distance(nameText.gameObject.transform.position, Camera.main.gameObject.transform.position);
        if (dname < EditorManager.minFadeDistanceStart * 5 || dname > EditorManager.maxFadeDistanceStart * 5)
        {
            Color c = ColorManager.thirdColor;
            Color c2 = ColorManager.secondaryColor;

            //if it's close from camera
            if (dname < EditorManager.minFadeDistanceStart * 5)
            {
                c.a = c2.a = (dname - EditorManager.minFadeDistanceEnd * 5) / (EditorManager.minFadeDistanceStart * 5 - EditorManager.minFadeDistanceEnd * 5);
            }

            //if it's far from camera
            if (dname > EditorManager.maxFadeDistanceStart * 5)
            {
                c.a = c2.a = (dname - EditorManager.maxFadeDistanceEnd * 5) / (EditorManager.maxFadeDistanceStart * 5 - EditorManager.maxFadeDistanceEnd * 5);
            }

            nameText.color = c;

            //if (dplane < EditorManager.minFadeDistanceStart || dplane < EditorManager.maxFadeDistanceStart)
            //{
            planeRay.GetComponent<SpriteRenderer>().color = c2;
            planeShadow.GetComponent<SpriteRenderer>().color = c2;
            //}
        }
        else
        {
            //Ray / Shadow fading
            float dplane = Vector3.Distance(planeShadow.transform.position, gameObject.transform.position);
            if (dplane < EditorManager.minFadeDistanceStart || dplane > EditorManager.maxFadeDistanceStart)
            {
                Color c = ColorManager.secondaryColor;

                //if it's close from body
                if (dplane < EditorManager.minFadeDistanceStart)
                {
                    c.a = (dplane - EditorManager.minFadeDistanceEnd) / (EditorManager.minFadeDistanceStart - EditorManager.minFadeDistanceEnd);
                }

                //if it's far from body
                if (dplane > EditorManager.maxFadeDistanceStart)
                {
                    c.a = (dplane - EditorManager.maxFadeDistanceEnd) / (EditorManager.maxFadeDistanceStart - EditorManager.maxFadeDistanceEnd);
                }

                planeRay.GetComponent<SpriteRenderer>().color = c;
                planeShadow.GetComponent<SpriteRenderer>().color = c;
            }
        }

        


        
    }

    public static void ParseSpectralClass(Body b, out Sprite sprite, out Color color)
    {
        SpriteRenderer sr = new SpriteRenderer();
        color = Color.white;
        sprite = starSprite;

        if (IsSpectralClassSupported(b))
        {
            if (b.bodyClass == "M")
            {
                sprite = starSprite;
                color = new Color32(255, 163, 79, 255);
            }
            if (b.bodyClass == "K")
            {
                sprite = starSprite;
                color = new Color32(255, 213, 180, 255);
            }
            if (b.bodyClass == "G")
            {
                sprite = starSprite;
                color = new Color32(255, 237, 222, 255);
            }
            if (b.bodyClass == "F")
            {
                sprite = starSprite;
                color = new Color32(247, 245, 255, 255);
            }
            if (b.bodyClass == "A")
            {
                sprite = starSprite;
                color = new Color32(209, 222, 255, 255);
            }
            if (b.bodyClass == "B")
            {
                sprite = starSprite;
                color = new Color32(181, 205, 255, 255);
            }
            if (b.bodyClass == "O")
            {
                sprite = starSprite;
                color = new Color32(112, 157, 255, 255);
            }
            if (b.bodyClass == "X")
            {
                sprite = blackHoleSprite;
                color = new Color32(40, 40, 40, 255);
            }
            if(b.bodyClass == "Unknown")
            {
                sprite = blackHoleSprite;
                color = new Color(1, 0, 1, 1);
            }
        }

        else
        {
            sprite = blackHoleSprite;
            color = new Color(1, 0, 1, 1);
            Logger.Warning(b.bodyName + "'s spectral class [" + b.bodyClass + "]" +
                " is not supported by the spectral parser !");
        }
    }
    public static bool IsSpectralClassSupported(Body b)
    {
        return SpectralClasses.IsDefined(typeof(SpectralClasses), b.bodyClass);
    }
    public static void DestroyAllBodies()
    {
        if (Body.bodies != null)
        {
            List<GameObject> gos = Body.bodies;

            foreach (GameObject g in gos)
            {

                Body b = g.GetComponent<Body>();
                Destroy(b.planeShadow);
                Destroy(b.planeRay);
                Destroy(b.displaySOI);
                Destroy(b.nameText);
                Destroy(g);
            }
        }
    }
    /*public static void DestroyAllBodies(List<GameObject> GameObjectBodies)
    {
        if (GameObjectBodies != null)
        {
            foreach (GameObject g in GameObjectBodies)
            {
                Destroy(g.GetComponent<Body>().planeShadow);
                Destroy(g.GetComponent<Body>().planeRay);
                Destroy(g);
            }
        }
    }*/

}

public enum SpectralClasses
{
    M,
    K,
    G,
    F,
    A,
    B,
    O,
    X,
    Unknown
}
