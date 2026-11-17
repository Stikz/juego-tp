using UnityEngine;

// PATRON ABSTRACT FACTORY
public class DefaultSkinFactory : ISkinFactory
{
    private readonly SkinLibrary library;

    public DefaultSkinFactory(SkinLibrary library) { this.library = library; }

    public string SkinName => "Normal";
    public Sprite CreatePlayerSprite() => library.defaultPlayer;
    public RuntimeAnimatorController CreateBulletAnimator() => library.defaultBullet;
}

// PATRON ABSTRACT FACTORY
public class RedSkinFactory : ISkinFactory
{
    private readonly SkinLibrary library;

    public RedSkinFactory(SkinLibrary library) { this.library = library; }

    public string SkinName => "Roja";
    public Sprite CreatePlayerSprite() => library.redPlayer;
    public RuntimeAnimatorController CreateBulletAnimator() => library.redBullet;
}

// PATRON ABSTRACT FACTORY
public class BlueSkinFactory : ISkinFactory
{
    private readonly SkinLibrary library;

    public BlueSkinFactory(SkinLibrary library) { this.library = library; }

    public string SkinName => "Azul";
    public Sprite CreatePlayerSprite() => library.bluePlayer;
    public RuntimeAnimatorController CreateBulletAnimator() => library.blueBullet;
}
