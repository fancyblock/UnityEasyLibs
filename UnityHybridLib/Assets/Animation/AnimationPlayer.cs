using UnityEngine;


public class AnimationPlayer : MonoBehaviour
{
    public SpriteRenderer sprite;
    public AnimClip animClip;

    int m_curIndex;
    float m_timer;


    // Use this for initialization
    void Start ()
    {
        m_curIndex = 0;
        m_timer = 0;

        sprite.sprite = animClip.m_frames[0];
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= animClip.m_interval)
        {
            m_curIndex++;
            m_timer -= animClip.m_interval;

            if (m_curIndex >= animClip.m_frames.Count)
            {
                m_curIndex = 0;
            }

            sprite.sprite = animClip.m_frames[m_curIndex];
        }
    }
}
