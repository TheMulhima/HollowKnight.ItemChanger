﻿using System.Collections;
using UnityEngine;

namespace ItemChanger.Components
{
    internal class ObjectDestroyer : MonoBehaviour
    {
        private string _objectName;
        private string _sceneName;

        public static void Destroy(string sceneName, string destroyName)
        {
            GameObject obj = new GameObject();
            ObjectDestroyer destroyer = obj.AddComponent<ObjectDestroyer>();
            destroyer._objectName = destroyName;
            destroyer._sceneName = sceneName;
        }

        public void Start()
        {
            StartCoroutine(CheckDestroy());
        }

        public IEnumerator CheckDestroy()
        {
            while (GameObject.Find(_objectName) == null && GameManager.instance.GetSceneNameString() == _sceneName)
            {
                yield return new WaitForEndOfFrame();
            }

            if (GameManager.instance.GetSceneNameString() != _sceneName)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(GameObject.Find(_objectName));
                Destroy(gameObject);
            }
        }
    }
}