using System;

namespace GameFrameX.Advertisement.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AdvertisementConfigAttribute : Attribute
    {
        public Type ConfigType { get; }

        public AdvertisementConfigAttribute(Type configType)
        {
            ConfigType = configType;
        }
    }
}
