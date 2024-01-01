using MoreCompany.Utils;
using UnityEngine;

namespace MoreCompany.Cosmetics
{
    public class CosmeticInstance : MonoBehaviour
    {
        public string cosmeticId;
        public string boneName;
        public CosmeticType cosmeticType;
        public Texture2D icon;
        public bool scaledToPlayerPrefab;
    }

    public enum CosmeticType
    {
        HAT,
        WRIST,
        CHEST,
        R_LOWER_ARM,
        HIP,
        L_SHIN,
        R_SHIN
    }
}