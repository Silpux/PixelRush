using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>{

    public static T Instance{get; private set;}

    protected virtual void Awake(){
        if(Instance == null){
            Instance = this as T;
            if(Instance is null){
                throw new InvalidOperationException($"{nameof(T)} is null");
            }
            return;
        }
        Debug.LogError($"{typeof(T)} already exists!");
        Destroy(gameObject);
    }

}
