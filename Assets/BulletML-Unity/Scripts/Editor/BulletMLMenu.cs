#if UNITY_EDITOR
// Copyright © 2014 FoodChain Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEditor;
using UnityEngine;
using FoodChain;

namespace FoodChain.BulletML
{
  public class BulletBankMenu : MonoBehaviour
  {
    [MenuItem("Assets/Create/BulletML/Bullet Bank")]
    [AddComponentMenu("BulletML/New bullet bank")]
    public static void CreateBankOtherMenu()
    {
      ScriptableObjectUtility.CreateAsset<BulletBank>();
    }

  }
}
#endif
