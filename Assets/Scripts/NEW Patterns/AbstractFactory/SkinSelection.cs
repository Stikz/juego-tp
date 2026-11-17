using UnityEngine;

public enum SkinType { Normal, Roja, Azul }

public static class SkinSelection
{
    private const string SkinKey = "PlayerSkin";
    private static SkinLibrary library;

    public static SkinType Current
    {
        get => (SkinType)PlayerPrefs.GetInt(SkinKey, 0);
        set
        {
            PlayerPrefs.SetInt(SkinKey, (int)value);
            PlayerPrefs.Save();
        }
    }

    public static ISkinFactory GetFactory()
    {
        return GetFactory(Current);
    }

    public static ISkinFactory GetFactory(SkinType type)
    {
        if (library == null)
            library = Resources.Load<SkinLibrary>("SkinLibrary");

        switch (type)
        {
            case SkinType.Roja: return new RedSkinFactory(library);
            case SkinType.Azul: return new BlueSkinFactory(library);
            default: return new DefaultSkinFactory(library);
        }
    }
}
