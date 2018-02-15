using UnityEngine;
using Okomotive.AnimationSystem;

public class AnimationMultiTrigger : MonoBehaviour
{

    [SerializeField]
    private AnimationBase[] Animations;

    public void PlayState(int State)
    {
        for (int i = 0; i < Animations.Length; i++)
        {
            Animations[i].PlayState(State);
        }
    }

    public void Stop()
    {
        for (int i = 0; i < Animations.Length; i++)
        {
            Animations[i].Stop();
        }
    }
}
