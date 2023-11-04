using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingAnimView : MonoBehaviour
{
    [SerializeField] float _animSpeed = 1f;
    [Space(10)]
    [SerializeField] Image[] _whiteDot;

    public IEnumerator Play()
    {
        int index = 0;

        while (true)
        {
            yield return new WaitForSeconds(_animSpeed);

            if (index < _whiteDot.Length)
            {
                _whiteDot[index].enabled = true;
                index++;
            }
            else
            {
                foreach(var dot in _whiteDot)
                {
                    dot.enabled = false;
                }

                index = 0;
            }
        }
    }
}
