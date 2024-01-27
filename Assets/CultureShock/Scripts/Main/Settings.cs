using UnityEngine;
using UnityEngine.Tilemaps;

namespace CultureShock.Scripts.Main
{
    [CreateAssetMenu(fileName = "Settings", menuName = "CultureShock/Settings", order = 3)]
    public class Settings : ScriptableObject
    {
        [HideInInspector] public int tileBaseCount;
        [HideInInspector] public TileBase[] tileBase;
        [HideInInspector] public int countButton;
        [HideInInspector] public int[] posXButton;
        [HideInInspector] public int countAlbum;
        [HideInInspector] public string[] nameAlbum;
        [HideInInspector] public int countMode;
        [HideInInspector] public string[] nameMode;
        [HideInInspector] public int level;
        [HideInInspector] public string[] nameLevel;
        [HideInInspector] public int rate;
        [HideInInspector] public string[] nameRate;
        [HideInInspector] public int[] limitRate;
        [HideInInspector] public int[] ratePoint;
    }
}