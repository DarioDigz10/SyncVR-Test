using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private Camera m_MainCamera;

    private Quaternion _lastRot = Quaternion.identity;

    private void Awake() {
        m_MainCamera = Camera.main;
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        _lastRot = m_MainCamera.transform.rotation;
    }

    private void LateUpdate() {
        if (!m_MainCamera.transform.rotation.Equals(_lastRot)) {
            _lastRot = m_MainCamera.transform.rotation;
            transform.LookAt(transform.position + _lastRot * Vector3.forward, _lastRot * Vector3.up);
        }
    }
}
