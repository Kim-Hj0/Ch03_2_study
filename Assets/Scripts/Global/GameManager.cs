using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player {  get; private set; }
    [SerializeField] private string playerTag = "Player";

    private void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform; //FindGameObjectWithTag 딱 한번만 사용하기.
    }
}
