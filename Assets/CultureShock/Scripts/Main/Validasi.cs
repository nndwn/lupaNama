using UnityEngine;
using UnityEngine.AddressableAssets;


namespace CultureShock.Scripts.Main
{
    public static class StringExtensions
    {
        public static string to_s(this object value)
        {
            return value.ToString();
        }
    }

    public enum E
    {
        NewInstall,
        NewOldPlayerDone,
        DownSaveDone
    }


    public class Validasi : MonoBehaviour, IFinishZoomInHandler
    {
        [SerializeField] private bool newInstall;
        [SerializeField] private bool newOldPlayerDone;

        public int newinstall;
        public int newOldPlayer;
        public int downloadDown;

        public AssetReferenceT<AudioClip> bgAudio;
        public AudioSource audioSource;
        [HideInInspector]public Transform round;
        [HideInInspector]public Transform textObject;

        public Settings settings;
        
        private int _done;

        private void Start()
        {
            Toolkit.LoadAudio(bgAudio, audioSource);
            InitialMain();
        }
        

        private void InitialMain()
        {
            //Toolkit.LoadAudio();
            round.localScale = Vector3.zero;
            textObject.gameObject.SetActive(false);
            StartCoroutine(Toolkit.AnimateZoomIn(round,Vector3.zero,2,Vector3.one, this));
       
        }
        public void FinishZoomIn()
        {
            if (round.transform.localScale == Vector3.one)
            {
                textObject.gameObject.SetActive(true);
                CheckNewInstall();
                CheckUser();
                CheckSave();
                CheckBilling();
                GetAlbum();
            }
        }



        private void GetAlbum()
        {
            if (settings is null) return;
            for (var i = 0; i < settings.countAlbum; i++)
            {
                if (i != 0) continue;
                if (!PlayerPrefs.HasKey(settings.nameAlbum[i]))
                    PlayerPrefs.SetString(settings.nameAlbum[i], "done");
            }
        }


        private void CheckSave()
        {
            //untuk cek save cek terlebih dahulu data user lama 
            if (PlayerPrefs.HasKey(E.NewOldPlayerDone.to_s()))
                if (newOldPlayer == 1 && !PlayerPrefs.HasKey(E.DownSaveDone.to_s()))
                {
                    //download data save
                    PlayerPrefs.SetInt(E.DownSaveDone.to_s(), 1);
                    downloadDown = PlayerPrefs.GetInt(E.DownSaveDone.to_s());
                }

            _done++;
        }

        //buat kondisi jika user online atau tidak
        //check akun player dari google play game
        public static bool CheckOnline()
        {
            return true;
        }

        //Indifikasi user sebagai player lama atau player baru 
        //function ini hanya berjalan hanya pertama kali ketika player melakukan installasi
        private void CheckUser()
        {
            if (CheckOnline() && PlayerPrefs.HasKey(E.NewInstall.to_s()))
                if (newinstall == 1 && !PlayerPrefs.HasKey(E.NewOldPlayerDone.to_s()))
                {
                    //indification data user terlebih dahulu
                    //aturan old user
                    //memiliki akun user jika tidak memiliki akun user ada di kembalikan sebagai new
                    //jadi kapan newOldPlayerDone menjadi 1 ? 
                    // ketika player melakukan save data secara online untuk pertama kali.
                    PlayerPrefs.SetInt(E.NewOldPlayerDone.to_s(), 0);
                    newOldPlayer = PlayerPrefs.GetInt(E.NewOldPlayerDone.to_s());
                }
        }

        //untuk mengindifikasi user baru melakukan installasi atau tidak
        // dengan menyimpan data bool ke local
        private void CheckNewInstall()
        {
            if (!PlayerPrefs.HasKey(E.NewInstall.to_s()))
            {
                PlayerPrefs.SetInt(E.NewInstall.to_s(), 1);
                newinstall = PlayerPrefs.GetInt(E.NewInstall.to_s());
            }
        }

        private void CheckBilling()
        {
            /*if (PlayerPrefs.HasKey(Data.Instance.listAlbum[1].name))
            {
                if (PlayerPrefs.GetString(Data.Instance.listAlbum[1].name) == "done")
                    Debug.Log("No Payment");
                else
                    Debug.Log("Already Payment");
            }*/
        }

        public void GetObject()
        {
            round = gameObject.transform.Find("PanelLoadValidation/initial/round");
            textObject = gameObject.transform.Find("PanelLoadValidation/initial/text");

        }
    }
}