using UnityEngine;

// PATRON ABSTRACT FACTORY
public interface ISkinFactory
{
    string SkinName { get; }
    Sprite CreatePlayerSprite();
    RuntimeAnimatorController CreateBulletAnimator();
}
