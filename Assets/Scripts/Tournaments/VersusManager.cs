using UnityEngine;
using System.Collections;

public class VersusManager : MonoBehaviour
{
    private static VersusManager _instance;
    public static VersusManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(VersusManager)) as VersusManager;
                DontDestroyOnLoad(_instance.gameObject);

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    
    [Header("VictoryCount")]
    public int victoryCountPlayer1 = 0;
    public int victoryCountPlayer2 = 0;
    
    [Header("Animation")]
    private RectTransform leftPlayerRectTransform;
    private RectTransform rightPlayerRectTransform;
    private Vector2 originalLeftPosition;
    private Vector2 originalRightPosition;
    public GameObject victoryContainerRed;
    public GameObject victoryContainerBlue;
    
    private void Start()
    {
        leftPlayerRectTransform = GameObject.Find("NinjaRed").GetComponent<RectTransform>();
        rightPlayerRectTransform = GameObject.Find("NinjaBlue").GetComponent<RectTransform>();
        originalLeftPosition = leftPlayerRectTransform.anchoredPosition;
        originalRightPosition = rightPlayerRectTransform.anchoredPosition;
        
        StartCoroutine(AnimatePlayers());
    }

    private IEnumerator AnimatePlayers()
    {
        while (true)
        {
            Vector2 leftTargetPosition = originalRightPosition + (originalLeftPosition - originalRightPosition) * 0.8f;
            Vector2 rightTargetPosition = originalLeftPosition + (originalRightPosition - originalLeftPosition) * 0.8f;
            
            LeanTween.move(leftPlayerRectTransform, leftTargetPosition, 0.3f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.move(rightPlayerRectTransform, rightTargetPosition, 0.3f).setEase(LeanTweenType.easeOutQuad);

            yield return new WaitForSeconds(.2f);
            //Create Particle
            GameObject fryingParticle = Resources.Load<GameObject>("Prefabs/Particles/UIFryingParticle");
            Instantiate(fryingParticle, new Vector3(0f, 0f, 0f), Quaternion.identity);
            
            yield return new WaitForSeconds(0.1f);
            
            
            LeanTween.move(leftPlayerRectTransform, originalLeftPosition, 0.3f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.move(rightPlayerRectTransform, originalRightPosition, 0.3f).setEase(LeanTweenType.easeOutQuad);

            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    public void ApplyVictoryCount()
    {
        for (int i = 1; i <= victoryCountPlayer1; i++)
        {
            victoryContainerRed.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        for (int i = 1; i <= victoryCountPlayer2; i++)
        {
            victoryContainerBlue.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}