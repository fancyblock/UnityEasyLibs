using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class UIAnimatorPlayer : MonoBehaviour
{
    public Image m_image;
    public AnimClip m_anim;
    public float m_intervalMin = 0;
    public float m_intervalMax = 0;

    public bool autoPlay = true;
    public bool loop = true;
    public Sprite m_defaultSpr;

    bool m_playing = false;
    bool m_cooldown;
    int m_curIndex;
    float m_timer;


    public void PlayClip(AnimClip clip = null)
    {
        if(clip != null)
            m_anim = clip;

        reset();
    }

    void Awake()
    {
        if(autoPlay)
            reset();
    }

    void Update()
    {
        if (!m_playing)
            return;

        if(m_cooldown)
        {
            m_timer -= Time.deltaTime;

            if(m_timer <= 0.0f)
            {
                m_cooldown = false;
                m_timer = 0.0f;
            }
        }
        else 
        {
            m_timer += Time.deltaTime;

            if (m_timer >= m_anim.m_interval)
            {
                m_curIndex++;
                m_timer -= m_anim.m_interval;

                if (m_curIndex >= m_anim.m_frames.Count)
                {
                    m_curIndex = 0;
                    m_cooldown = true;
                    m_timer = Random.Range(m_intervalMin, m_intervalMax);

                    if (!loop)
                    {
                        m_image.sprite = m_defaultSpr;
                        m_playing = false;
                    }
                }
                else
                {
                    m_image.sprite = m_anim.m_frames[m_curIndex];
                }
            }
        }
    }

    void reset()
    {
        m_curIndex = 0;
        m_timer = 0;
        m_cooldown = false;

        m_image.sprite = m_anim.m_frames[0];

        m_playing = true;
    }


    [ContextMenu("TestPlay")]
    void TestPlay()
    {
        PlayClip();
    }
}
