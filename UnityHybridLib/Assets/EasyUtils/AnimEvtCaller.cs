using UnityEngine;
using UnityEngine.Events;


public class AnimEvtCaller : MonoBehaviour
{
    [SerializeField] private UnityEvent m_anim01Evt;
    [SerializeField] private UnityEvent m_anim02Evt;
    [SerializeField] private UnityEvent m_anim03Evt;
    [SerializeField] private UnityEvent m_anim04Evt;



    public void OnAnimEvt01()
    {
        m_anim01Evt.Invoke();
    }

    public void OnAnimEvt02()
    {
        m_anim02Evt.Invoke();
    }

    public void OnAnimEvt03()
    {
        m_anim03Evt.Invoke();
    }

    public void OnAnimEvt04()
    {
        m_anim04Evt.Invoke();
    }

}
