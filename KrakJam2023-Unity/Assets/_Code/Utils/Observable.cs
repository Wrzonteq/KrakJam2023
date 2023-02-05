using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023.Utils {
    public class Observable <T> {
        public event Action<T> ChangedValue;
        public event Action Changed;

        T value;
        readonly bool isValueType;

        public T Value {
            get => value;
            set => Set(value);
        }

        public Observable(T value = default) {
            this.value = value;
            isValueType = typeof(T).IsValueType;
        }

        public void Set(T newValue, bool forceUpdate = false) {
            var equals = isValueType ? value.Equals(newValue) : ReferenceEquals(value, newValue);
            if (equals && !forceUpdate)
                return;
            value = newValue;
            Changed?.Invoke();
            ChangedValue?.Invoke(value);
        }

        public void SilentSet(T newValue) {
            value = newValue;
        }
    }

    public static class Extensions {
        public static void SetLocalScaleX(this Transform transform, float scaleX) {
            var scale = transform.localScale;
            scale.x = scaleX;
            transform.localScale = scale;
        }

        public static void SetLocalScaleY(this Transform transform, float scaleY) {
            var scale = transform.localScale;
            scale.y = scaleY;
            transform.localScale = scale;
        }

        public static void SetLocalScaleZ(this Transform transform, float scaleZ) {
            var scale = transform.localScale;
            scale.z = scaleZ;
            transform.localScale = scale;
        }
    }

    public static class Utilities {
        public static int SecondsToMiliseconds(float seconds) {
            return (int)(seconds * 1000);
        }
    }
}
