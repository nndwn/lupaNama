using UnityEngine;

namespace CultureShock.Scripts.Main
{
    [CreateAssetMenu(fileName = "CreateAlbum", menuName = "CultureShock/Create Album", order = 0)]
    public class CreateAlbum : ScriptableObject
    {
        // [MenuItem("CultureShock/Create Album")]


        [HideInInspector] public Sprite cover;
        public Sprite[] animateCover;
        public string author;
        public CreateClip[] musics;
    }
}