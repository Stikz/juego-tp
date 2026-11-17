// PATRON MEMENTO
public class SettingsMemento
{
    public float Master { get; }
    public float Sfx { get; }
    public float Music { get; }
    public bool Fullscreen { get; }

    public SettingsMemento(float master, float sfx, float music, bool fullscreen)
    {
        Master = master;
        Sfx = sfx;
        Music = music;
        Fullscreen = fullscreen;
    }
}
