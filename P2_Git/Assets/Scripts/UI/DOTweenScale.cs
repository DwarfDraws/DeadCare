using UnityEngine;
using DG.Tweening;

public class DOTweenScale : MonoBehaviour
{

    private Vector3 stars_initScale;
    private Vector3 stars_scaleTo;

    [SerializeField] int starID;
    static float stars_tweeningFactor;
    static float stars_tweeningDuration_seconds;
    static float tweeningOffset_seconds;
    static float waitTime_atMaxScale_seconds;
    float waitTime;

    void Start()
    {
        stars_initScale = transform.localScale;
        stars_scaleTo = stars_initScale * stars_tweeningFactor;
    }

    public void StartTweening()
    {
        waitTime = tweeningOffset_seconds * starID;
        Invoke("StartStarTween", waitTime);
    }

    void StartStarTween()
    {
        transform.DOScale(stars_scaleTo, stars_tweeningDuration_seconds)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DOScale(stars_initScale, stars_tweeningDuration_seconds)
                    .SetEase(Ease.OutBounce)
                    .SetDelay(waitTime_atMaxScale_seconds)
                    .OnComplete(StartStarTween);
            });
    }

    public void SetupTweening(float factor, float duration_sec, float offset_sec, float waitTime_atMax_sec) 
    {
        stars_tweeningFactor = factor;
        stars_tweeningDuration_seconds = duration_sec;
        tweeningOffset_seconds = offset_sec;
        waitTime_atMaxScale_seconds = waitTime_atMax_sec;
    }


}
