using System;
using System.Collections.Generic;
using UnityEngine;

  
public enum AnimState
{
    Idle = 0,
    Death = 1,
}

[CreateAssetMenu(fileName ="SpriteAnimatorCfg", menuName = "Configs / Animation Config", order = 4)]
public class SpriteAnimatorConfig : ScriptableObject
{   
    [Serializable]
    public sealed class SpriteSequence 
    {
        public AnimState Track;
        public List<Sprite> Sprites = new List<Sprite>();
        public float AnimationSpeed;
    }

    public List<SpriteSequence> Sequence = new List<SpriteSequence>();
}
