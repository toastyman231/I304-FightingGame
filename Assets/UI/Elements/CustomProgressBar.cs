using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomProgressBar : VisualElement, INotifyValueChanged<float>
{
    public int width { get; set; }
    public int height { get; set; }

    public void SetValueWithoutNotify(float newValue)
    {
        m_value = newValue;
    }

    private float m_value;

    public float value
    {
        get => Mathf.Clamp01(m_value);
        set
        {
            if (EqualityComparer<float>.Default.Equals(m_value, value)) return;
            if (this.panel != null)
            {
                using (ChangeEvent<float> pooled = ChangeEvent<float>.GetPooled(this.m_value, value))
                {
                    pooled.target = (IEventHandler)this;
                    this.SetValueWithoutNotify(value);
                    this.SendEvent((EventBase)pooled);
                } 
            }
            else
            {
                SetValueWithoutNotify(value);
            }
        }
    }

    public enum FillType
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    public Color fillColor;
    public Color backgroundColor;
    public Color borderColor;

    public float borderRadius;

    public FillType fillType;

    private VisualElement progressBarParent;
    private VisualElement progressBarBackground;
    private VisualElement progressBarForeground;

    public new class UxmlFactory: UxmlFactory<CustomProgressBar, UxmlTraits> {}

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        private UxmlIntAttributeDescription m_width = new UxmlIntAttributeDescription() { name = "width", defaultValue = 300};
        private UxmlIntAttributeDescription m_height = new UxmlIntAttributeDescription() { name = "height", defaultValue = 50 };
        private UxmlFloatAttributeDescription m_value = new UxmlFloatAttributeDescription() { name = "value", defaultValue = 1 };
        private UxmlFloatAttributeDescription m_borderRadius = new UxmlFloatAttributeDescription()
            { name = "border-radius", defaultValue = 5 };
        private UxmlEnumAttributeDescription<FillType> m_fillType = new UxmlEnumAttributeDescription<FillType>()
            { name = "fill-type", defaultValue = 0 };
        private UxmlColorAttributeDescription m_fillColor = new UxmlColorAttributeDescription()
            { name = "fill-color", defaultValue = Color.red };
        private UxmlColorAttributeDescription m_backgroundColor = new UxmlColorAttributeDescription()
            { name = "background-color", defaultValue = Color.black };
        private UxmlColorAttributeDescription m_borderColor = new UxmlColorAttributeDescription()
            { name = "border-color", defaultValue = Color.black };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as CustomProgressBar;
            ate.width = m_width.GetValueFromBag(bag, cc);
            ate.height = m_height.GetValueFromBag(bag, cc);
            ate.value = m_value.GetValueFromBag(bag, cc);
            ate.borderRadius = m_borderRadius.GetValueFromBag(bag, cc);
            ate.fillType = m_fillType.GetValueFromBag(bag, cc);
            ate.fillColor = m_fillColor.GetValueFromBag(bag, cc);
            ate.backgroundColor = m_backgroundColor.GetValueFromBag(bag, cc);
            ate.borderColor = m_borderColor.GetValueFromBag(bag, cc);

            ate.Clear();
            VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("UIDocuments/CustomProgressBar");
            VisualElement progressBar = vt.Instantiate();
            ate.progressBarParent = progressBar.Q<VisualElement>("ProgressBar");
            ate.progressBarBackground = progressBar.Q<VisualElement>("Background");
            ate.progressBarForeground = progressBar.Q<VisualElement>("Foreground");
            ate.Add(progressBar);

            //ate.progressBarParent.style.width = ate.width;
            ate.progressBarParent.style.height = ate.height;
            //ate.style.width = ate.width;
            ate.style.height = ate.height;
            ate.progressBarForeground.style.backgroundColor = ate.fillColor;
            ate.progressBarBackground.style.backgroundColor = ate.backgroundColor;
            ate.SetBorderInfo();

            ate.SetFillDirection();

            ate.RegisterValueChangedCallback(ate.UpdateProgress);
            ate.FillProgress();
        }
    }

    public void UpdateProgress(ChangeEvent<float> evt)
    {
        FillProgress();
    }

    private void SetBorderInfo()
    {
        progressBarParent.style.borderBottomColor = borderColor;
        progressBarParent.style.borderTopColor = borderColor;
        progressBarParent.style.borderLeftColor = borderColor;
        progressBarParent.style.borderRightColor = borderColor;

        progressBarParent.style.borderBottomLeftRadius = borderRadius;
        progressBarParent.style.borderBottomRightRadius = borderRadius;
        progressBarParent.style.borderTopLeftRadius = borderRadius;
        progressBarParent.style.borderTopRightRadius = borderRadius;
    }

    private void SetFillDirection()
    {
        switch (fillType)
        {
            case FillType.LeftToRight:
                progressBarForeground.style.transformOrigin = new StyleTransformOrigin(
                    new TransformOrigin(Length.Percent(0), Length.Percent(100)));
                break;
            case FillType.RightToLeft:
                progressBarForeground.style.transformOrigin = new StyleTransformOrigin(
                    new TransformOrigin(Length.Percent(100), Length.Percent(100)));
                break;
            case FillType.TopToBottom:
                progressBarForeground.style.transformOrigin = new StyleTransformOrigin(
                    new TransformOrigin(Length.Percent(100), Length.Percent(0)));
                break;
            case FillType.BottomToTop:
                progressBarForeground.style.transformOrigin = new StyleTransformOrigin(
                    new TransformOrigin(Length.Percent(0), Length.Percent(100)));
                break;
            default:
                break;
        }
    }

    private void FillProgress()
    {
        if (fillType == FillType.LeftToRight || fillType == FillType.RightToLeft)
        {
            progressBarForeground.style.scale = new Scale(new Vector3(value, 1, 1));
        }
        else
        {
            progressBarForeground.style.scale = new Scale(new Vector3(1, value, 1));
        }
    }
}
