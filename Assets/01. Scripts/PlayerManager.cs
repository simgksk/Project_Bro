using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    
    public static PlayerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<PlayerManager>();
            }
            return _instance;
        }

    }

    public GameObject CurrentCharacter { get; set; }
}
