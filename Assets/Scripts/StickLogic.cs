using System;
using System.Collections;
using UnityEngine;

public class StickLogic : MonoBehaviour
{
    #region Fields / Events / Properties 

    const float STICK_DOWN_SPEED = 400f;
    const float STICK_GROW_SPEED = 13f;
    const float ROTATE_AMOUNT = -90f;

    public event Action OnStickFallDown;

    RectTransform stickRect;
    bool isStickFallDown = false;
    bool hasStickGrew = false;

    AudioSource sound;

    public RectTransform GetRect
    {
        get
        {
            return stickRect;
        }
        private set
        {
            stickRect = value;
        }
    }

    #endregion

    #region Unity LifeCycle

    void Start()
    {
        stickRect = GetComponent<RectTransform>();
        sound = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (!hasStickGrew)
        {
            if (Input.GetMouseButton(0))
            {
                float stickHeight = transform.localPosition.y + stickRect.sizeDelta.y;

                if (stickHeight <= Screen.height)
                    stickRect.sizeDelta += new Vector2(0, STICK_GROW_SPEED);

                if (!sound.isPlaying)
                    sound.Play();
            }

            if (Input.GetMouseButtonUp(0))
            {
                sound.Stop();

                StopAllCoroutines();
                StartCoroutine(RotateStick());

                hasStickGrew = true;
            }
        }

        if (isStickFallDown)
        {
            if (OnStickFallDown != null)
            {
                OnStickFallDown();
            }
            isStickFallDown = false;
        }
    }

    #endregion

    #region Stick Interact

    /// <summary>
    /// Поворот на констное количество градусов
    /// </summary>
    /// <returns></returns>
    public IEnumerator RotateStick()
    {
        Quaternion finalRotation = Quaternion.Euler(0, 0, ROTATE_AMOUNT) * 
            transform.rotation;

        while (transform.rotation != finalRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                finalRotation, Time.deltaTime * STICK_DOWN_SPEED);
            yield return 0;
        }
        isStickFallDown = true;
    }


    /// <summary>
    /// Сброс текущих параметров палки
    /// </summary>
    public void ResetStick()
    {
        stickRect.localRotation = new Quaternion(0, 0, 0, 0);
        stickRect.sizeDelta = new Vector2(stickRect.sizeDelta.x, 0);
        hasStickGrew = false;
    }

    #endregion
}