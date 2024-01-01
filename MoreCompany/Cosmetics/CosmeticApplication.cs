using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreCompany.Cosmetics
{
    public class CosmeticApplication : MonoBehaviour
    {
        public List<CosmeticInstance> spawnedCosmetics = new List<CosmeticInstance>();
        
        private Dictionary<string, Transform> bones = new Dictionary<string, Transform>();

        public void Awake()
        {
            RecursiveCacheBones(transform.Find("spine"), new string[]{"spine", "shoulder", "arm", "hand", "finger", "thigh", "shin", "foot", "heel", "toe"});

            RefreshAllCosmeticPositions();
        }

        private void OnDisable()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                spawnedCosmetic.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                spawnedCosmetic.gameObject.SetActive(true);
            }
        }

        public void ClearCosmetics()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                GameObject.Destroy(spawnedCosmetic.gameObject);
            }
            spawnedCosmetics.Clear();
        }
        
        public void ApplyCosmetic(string cosmeticId, bool startEnabled)
        {
            if (CosmeticRegistry.cosmeticInstances.ContainsKey(cosmeticId))
            {
                CosmeticInstance cosmeticInstance = CosmeticRegistry.cosmeticInstances[cosmeticId];
                GameObject cosmeticInstanceGameObject = GameObject.Instantiate(cosmeticInstance.gameObject);
                cosmeticInstanceGameObject.SetActive(startEnabled);
                CosmeticInstance cosmeticInstanceBehavior = cosmeticInstanceGameObject.GetComponent<CosmeticInstance>();
                spawnedCosmetics.Add(cosmeticInstanceBehavior);
                if (cosmeticInstanceBehavior.scaledToPlayerPrefab)
                    cosmeticInstanceGameObject.transform.localScale /= CosmeticRegistry.COSMETIC_PLAYER_SCALE_MULT; // because of scale difference between display guy and player prefab
                if (startEnabled)
                {
                    ParentCosmetic(cosmeticInstanceBehavior);
                }
            }
        }
        
        public void RefreshAllCosmeticPositions()
        {
            foreach (var spawnedCosmetic in spawnedCosmetics)
            {
                ParentCosmetic(spawnedCosmetic);
            }
        }

        private void ParentCosmetic(CosmeticInstance cosmeticInstance)
        {
            Transform targetTransform = null;
            if (string.IsNullOrEmpty(cosmeticInstance.boneName) == false)
                targetTransform = bones[cosmeticInstance.boneName];
            else {
                switch (cosmeticInstance.cosmeticType)
                {
                    case CosmeticType.HAT:
                        targetTransform = bones["spine.004"];
                        break;
                    case CosmeticType.R_LOWER_ARM:
                        targetTransform = bones["arm.R_lower"];
                        break;
                    case CosmeticType.HIP:
                        targetTransform = bones["spine"];
                        break;
                    case CosmeticType.L_SHIN:
                        targetTransform = bones["shin.L"];
                        break;
                    case CosmeticType.R_SHIN:
                        targetTransform = bones["shin.R"];
                        break;
                    case CosmeticType.CHEST:
                        targetTransform = bones["spine.003"];
                        break;
                }
            }

            cosmeticInstance.transform.position = targetTransform.position;
            cosmeticInstance.transform.rotation = targetTransform.rotation;
            cosmeticInstance.transform.parent = targetTransform;
        }

        void RecursiveCacheBones(Transform currentTf, string[] nameFilter) {
            bool filtersPassed = false;
            foreach (string f in nameFilter) {
                if (currentTf.name.StartsWith(f) && currentTf.name.EndsWith("_end") == false) {
                    filtersPassed = true;
                    break;
                }
            }
            if (filtersPassed == false)
                return;

            bones[currentTf.name] = currentTf;
            foreach (Transform child in currentTf) {
                RecursiveCacheBones(child, nameFilter);
            }
        }
    }
}