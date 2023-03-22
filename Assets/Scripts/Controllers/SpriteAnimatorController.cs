using System;
using System.Collections.Generic;
using UnityEngine;


internal class SpriteAnimatorController : IDisposable
{
    private sealed class Animation
    {
        public AnimState Track;
        public List<Sprite> Sprites;
        public bool Loop;
        public float Speed;
        public float FrameCounter = 0;
        public bool Sleep;

        public void Execute()
        {
            if (Sleep) return;
            FrameCounter += Time.deltaTime * Speed;
            if(Loop)
            {
                while(FrameCounter > Sprites.Count)
                {
                    FrameCounter -= Sprites.Count;
                }
            }
            else if(FrameCounter > Sprites.Count)
            {
                FrameCounter = Sprites.Count;
                Sleep = true;
            }
        }
    }

    private SpriteAnimatorConfig _config;
    private Dictionary<SpriteRenderer, Animation> _activeAnimation = new Dictionary<SpriteRenderer, Animation>();

    public SpriteAnimatorController(SpriteAnimatorConfig config)
    {
        _config = config;
    }
    public void StartAnimation(SpriteRenderer spriteRenderer, AnimState track, bool loop)
    {
        if(_activeAnimation.TryGetValue(spriteRenderer, out var animation))
        {
            animation.Sleep = false;
            animation.Loop = loop;
            if(animation.Track != track)
            {
                animation.Track = track;
                animation.Sprites = _config.Sequence.Find(sequence => sequence.Track == track).Sprites;
                animation.Speed = _config.Sequence.Find(sequence => sequence.Track == track).AnimationSpeed;
                animation.FrameCounter = 0;
            }
        }
        else
        {
            _activeAnimation.Add(spriteRenderer, 
            new Animation()
            {
                Loop = loop,
                Track = track,
                Sprites = _config.Sequence.Find(sequence => sequence.Track == track).Sprites,
                Speed = _config.Sequence.Find(sequence => sequence.Track == track).AnimationSpeed
        });
        }               
    }

    public void StopAnimation(SpriteRenderer spriteRenderer)
    {
        if(_activeAnimation.ContainsKey(spriteRenderer))
        {
            _activeAnimation.Remove(spriteRenderer);
        }
    }

    public void Execute()
    {
        foreach(var animation in _activeAnimation)
        {
            animation.Value.Execute();

            if(animation.Value.FrameCounter < animation.Value.Sprites.Count)
            {
                animation.Key.sprite = animation.Value.Sprites[(int)animation.Value.FrameCounter];
            }
        }
    }

    public void Dispose()
    {
        _activeAnimation.Clear();
    }
}