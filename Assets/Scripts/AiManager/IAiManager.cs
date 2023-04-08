using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface IAiManager 
{
    AiEntity agent { get; set; }
   
    void Enter();

    void Exit();

    void Tick();
}
