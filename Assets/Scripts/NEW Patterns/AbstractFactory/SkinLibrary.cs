using UnityEngine;

[CreateAssetMenu(menuName = "Skins/Skin Library")]
public class SkinLibrary : ScriptableObject
{
    public Sprite defaultPlayer;
    public Sprite redPlayer;
    public Sprite bluePlayer;

    public RuntimeAnimatorController defaultBullet;
    public RuntimeAnimatorController redBullet;
    public RuntimeAnimatorController blueBullet;
}
