using UnityEngine;

namespace CultureShock.Scripts.Main
{
    
    [ExecuteInEditMode]
    public class SingletonData : MonoBehaviour
    {
        public static SingletonData Instance { get; private set; }

        private void Awake()
        {
            CheckInstance();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) CheckInstance();
#endif
        }

        private void CheckInstance()
        {
            Instance = this;
        }

        public bool colorChange;
        

        // private void GetBilling(bool aktive)
        // {
        //     if (!PlayerPrefs.HasKey(Instance.listAlbum[1].name) && aktive)
        //         PlayerPrefs.SetString(Instance.listAlbum[1].name, "done");
        // }

        //check player membeli atau tidak 
        //jika membeli buat kondisi diletakan save game secara local
        //check kembali setiap player membuka game hanya berlaku di kondisi online
        //selalu di jalankan ketika saat start main menu.
   

        // public void GetAlbum()
        // {
        //     
        // }
    }
}