using UnityEngine;

namespace Zappar.Additional.SNS
{
    public abstract class ZWebListener : MonoBehaviour
    {
        public const string UnityObjectName = "_zWebglSNSListener";

        public abstract void MessageCallback(string message);
    }
}