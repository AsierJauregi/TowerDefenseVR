using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Transform selectionTransform;
    [SerializeField] private Transform cursorTransform;

    [Header("Events")]
    [SerializeField] private RadialSection top;
    [SerializeField] private RadialSection right;
    [SerializeField] private RadialSection bottom;
    [SerializeField] private RadialSection left;

    private Vector2 touchPosition = Vector2.zero;
    private List<RadialSection> radialSections;
    private RadialSection highlightedsection;

    private readonly float degreeIncrement = 90.0f;

    private void Awake()
    {
        Show(false);
        SetUpSections();
    }

    private void SetUpSections()
    {
        radialSections = new List<RadialSection>() { top, right, bottom, left };

        foreach (RadialSection section in radialSections)
        {
            section.iconRenderer.sprite = section.icon;
        }
    }

    private void Update()
    {
        Vector2 direction = Vector2.zero + touchPosition;
        float rotation = GetDegrees(direction);
        SetCursorPosition();
        if (rotation != 0)
        {
            SetSelectedRotation(rotation);
            SetSelectedEvent(rotation);
        }

    }
    public void Show(bool value)
    {
        gameObject.SetActive(value);
    }

    private float GetDegrees(Vector2 direction)
    {
        float result = Mathf.Atan2(direction.x, direction.y);
        result *= Mathf.Rad2Deg;

        if (result < 0) result += 360;

        return result;
    }
    private void SetCursorPosition()
    {
        cursorTransform.localPosition = touchPosition;
    }

    private void SetSelectedRotation(float newRotation)
    {
        float snappedRotation = SnapRotation(newRotation);
        selectionTransform.localEulerAngles = new Vector3(0, 0, -snappedRotation);
    }
    private float SnapRotation(float rotation)
    {
        return GetNearestIncrement(rotation) * degreeIncrement;
    }
    private int GetNearestIncrement(float rotation)
    {
        return Mathf.RoundToInt(rotation / degreeIncrement);
    }
    private void SetSelectedEvent(float currentRotation)
    {
        int index = GetNearestIncrement(currentRotation);

        if (index == 4) index = 0;

        highlightedsection = radialSections[index];
        highlightedsection.onPress.Invoke();
    }

    public Vector2 TouchPosition
    {
        get
        {
            return touchPosition;
        }
        set
        {
            touchPosition = value;
        }
    }
}
