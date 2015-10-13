﻿//bl_UtilityHelper.cs - Helper script that contains multiple useful functions
using UnityEngine;
using System.Collections;

public class bl_UtilityHelper {
    /// Call this to capture a custom, screenshot
    public static Texture2D CaptureCustomScreenshot(int width, int height) {
        UnityEngine.Texture2D textured = new UnityEngine.Texture2D(width, height, UnityEngine.TextureFormat.RGB24, true, false);
        textured.ReadPixels(new UnityEngine.Rect(0f, 0f, (float)width, (float)height), 0, 0);
        int miplevel = UnityEngine.Screen.width / 800;
        UnityEngine.Texture2D textured2 = new UnityEngine.Texture2D(width >> miplevel, height >> miplevel, UnityEngine.TextureFormat.RGB24, false, false);
        textured2.SetPixels32(textured.GetPixels32(miplevel));
        textured2.Apply();
        return textured2;
    }

    /// Call this to capture a screenshot Automatic size
    public static byte[] CaptureScreenshot() {
        UnityEngine.Texture2D textured = new UnityEngine.Texture2D(UnityEngine.Screen.width, UnityEngine.Screen.height, UnityEngine.TextureFormat.RGB24, false, false);
        textured.ReadPixels(new UnityEngine.Rect(0f, 0f, (float)UnityEngine.Screen.width, (float)UnityEngine.Screen.height), 0, 0);
        return textured.EncodeToPNG();
    }

    /// Call this to capture a custom size screenshot
    public static byte[] CaptureScreenshot(int width, int height) {
        Texture2D textured = new Texture2D(width, height, UnityEngine.TextureFormat.RGB24, false, false);
        textured.ReadPixels(new UnityEngine.Rect(0f, 0f, (float)width, (float)height), 0, 0);
        return textured.EncodeToPNG();
    }

    /// change the parent of an object to another
    public static void ChangeParent(UnityEngine.Transform target, UnityEngine.Transform parent) {
        Vector3 localPosition = target.localPosition;
        Quaternion localRotation = target.localRotation;
        Vector3 localScale = target.localScale;
        target.parent = parent;
        target.localPosition = localPosition;
        target.localRotation = localRotation;
        target.localScale = localScale;
    }

    /// change the parent of an object to another in word
    public static void ChangeParentWorld(UnityEngine.Transform target, UnityEngine.Transform parent) {
        Vector3 position = target.position;
        Quaternion rotation = target.rotation;
        target.parent = parent;
        target.position = position;
        target.rotation = rotation;
    }

    /// Get ClampAngle
    public static float ClampAngle(float ang, float min, float max) {
        if (ang < -360f)
            ang += 360f;
        if (ang > 360f)
            ang -= 360f;
        return UnityEngine.Mathf.Clamp(ang, min, max);
    }

    /// obtained, the magnitude of a certain position and another
    public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
        return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
    }

    /// locate an object hierarchy by name
    public static Transform FindHierarchy(Transform current, string name) {
        if (current.name == name)
            return current;
        for (int i = 0; i < current.childCount; i++) {
            UnityEngine.Transform transform = FindHierarchy(current.GetChild(i), name);
            if (transform != null)
                return transform;
        }
        return null;
    }

    /// obtained the period of a value
    public static float Period(float period, float k = 1f) {
        float t = Time.realtimeSinceStartup * k;
        return (t - (((int)(t / period)) * period));
    }

    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
        UnityEngine.Vector3 rhs = point - lineStart;
        UnityEngine.Vector3 vector2 = lineEnd - lineStart;
        float magnitude = vector2.magnitude;
        UnityEngine.Vector3 lhs = vector2;
        if (magnitude > 1E-06f)
            lhs = (UnityEngine.Vector3)(lhs / magnitude);
        float num2 = UnityEngine.Mathf.Clamp(UnityEngine.Vector3.Dot(lhs, rhs), 0f, magnitude);
        return (lineStart + ((UnityEngine.Vector3)(lhs * num2)));
    }

    /// Obtained distance between two positions.
    public static float GetDistance(Vector3 posA, Vector3 posB) {
        return Vector3.Distance(posA, posB);
    }

    public static GameObject GetGameObjectView(PhotonView m_view) {
        GameObject go = PhotonView.Find(m_view.viewID).gameObject;
        return go;
    }

    /// obtain only the first two values
    public static string GetDoubleChar(float f) {
        return f.ToString("00");
    }

    /// obtain only the first three values
    public static string GetThreefoldChar(float f) {
        return f.ToString("000");
    }

    public static string GetTimeFormat(float m, float s) {
        return string.Format("{0:00}:{1:00}", m, s);
    }

    public static float ComputeVolume(UnityEngine.Collider coll) {
        if (coll != null) {
            UnityEngine.CapsuleCollider collider = coll as UnityEngine.CapsuleCollider;
            if (collider != null) {
                UnityEngine.Vector3 lossyScale = collider.transform.lossyScale;
                float num = collider.height * lossyScale.y;
                float num2 = collider.radius * UnityEngine.Mathf.Max(lossyScale.x, lossyScale.z);
                float num3 = (3.141593f * num2) * num2;
                float num4 = num3 * num;
                float num5 = (num3 * num2) * 1.333333f;
                return (num4 + num5);
            }
            UnityEngine.SphereCollider collider2 = coll as UnityEngine.SphereCollider;
            if (collider2 != null) {
                UnityEngine.Vector3 vector2 = collider2.transform.lossyScale;
                float[] values = new float[] { vector2.x, vector2.y, vector2.z };
                float num6 = collider2.radius * UnityEngine.Mathf.Max(values);
                return (4.18879f * ((num6 * num6) * num6));
            }
            UnityEngine.BoxCollider collider3 = coll as UnityEngine.BoxCollider;
            if (collider3 != null) {
                UnityEngine.Vector3 vector3 = collider3.transform.lossyScale;
                UnityEngine.Vector3 size = collider3.size;
                return (((size.x * vector3.x) * (size.y * vector3.y)) * (size.z * vector3.z));
            }
            UnityEngine.Debug.LogWarning("Not implemented for " + coll.GetType() + " type!");
        }
        return 0f;
    }

    public static Vector3 CorrectForceSize(UnityEngine.Vector3 force) {
        float num = (1.2f / Time.timeScale) - 0.2f;
        force = (UnityEngine.Vector3)(force * num);
        return force;
    }

    public static void ShadowLabel(string text, params GUILayoutOption[] option) {
        Color color = GUI.color;
        Color black = Color.black;
        black.a = color.a;
        GUI.color = black;
        GUILayout.Label(text, option);
        Rect lastRect = GUILayoutUtility.GetLastRect();
        lastRect.x--;
        lastRect.y--;
        GUI.color = color;
        GUI.Label(lastRect, text);
    }
    public static void ShadowLabel(UnityEngine.Rect rect, string text) {
        ShadowLabel(rect, text, null);
    }

    public static void ShadowLabel(string text, UnityEngine.GUIStyle style, params UnityEngine.GUILayoutOption[] option) {
        UnityEngine.Color color = UnityEngine.GUI.color;
        UnityEngine.Color black = UnityEngine.Color.black;
        black.a = color.a;
        UnityEngine.GUI.color = black;
        UnityEngine.GUILayout.Label(text, style, option);
        UnityEngine.Rect lastRect = UnityEngine.GUILayoutUtility.GetLastRect();
        lastRect.x--;
        lastRect.y--;
        UnityEngine.GUI.color = color;
        UnityEngine.GUI.Label(lastRect, text, style);
    }

    public static void ShadowLabel(UnityEngine.Rect rect, string text, UnityEngine.GUIStyle style) {
        UnityEngine.Rect position = new UnityEngine.Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height);
        UnityEngine.Color color = UnityEngine.GUI.color;
        UnityEngine.Color color2 = !(color == UnityEngine.Color.black) ? UnityEngine.Color.black : UnityEngine.Color.white;
        color2.a = color.a;
        UnityEngine.GUI.color = color2;
        if (style != null)
            UnityEngine.GUI.Label(position, text, style);
        else
            UnityEngine.GUI.Label(position, text);
        UnityEngine.GUI.color = color;
        if (style != null)
            UnityEngine.GUI.Label(rect, text, style);
        else
            UnityEngine.GUI.Label(rect, text);
    }

    /// Helper for Cursor locked in Unity 5
    /// <param name="mLock">cursor state</param>
    public static void LockCursor(bool mLock) {
        if (mLock == true) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static bool GetCursorState
    {
        get
        {
            if (Cursor.visible && Cursor.lockState != CursorLockMode.Locked)
                return false;
            else
                return true;
        }
    }

    // The angle between dirA and dirB around axis
    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis) {
        // Project A and B onto the plane orthogonal target axis
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        // Find (positive) angle between A and B
        float angle = Vector3.Angle(dirA, dirB);

        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }

    public static bl_GameManager GetGameManager {
        get {
            bl_GameManager g = GameObject.FindWithTag("GameManager").GetComponent<bl_GameManager>();
            return g;
        }
    }

    public static void PlayClipAtPoint(AudioClip clip,Vector3 position,float volume,AudioSource sourc) {
        GameObject obj2 = new GameObject("One shot audio")
        {
            transform = { position = position }
        };
        AudioSource source = (AudioSource)obj2.AddComponent(typeof(AudioSource));
        source.minDistance = sourc.minDistance;
        source.maxDistance = sourc.maxDistance;
        source.panStereo = sourc.panStereo;
        source.spatialBlend = sourc.spatialBlend;
        source.rolloffMode = sourc.rolloffMode;
        source.priority = sourc.priority;
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Object.Destroy(obj2, clip.length * Time.timeScale);
    }
}
