using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BenchSit : MonoBehaviour
{
    [Header("Posición de sentado")]
    [SerializeField] private Transform sitPosition;
    [SerializeField] private Transform xrOrigin;

    [Header("Locomotion")]
    [SerializeField] private GameObject locomotion;

    [Header("UI por mando")]
    [SerializeField] private GameObject leftHandUI;
    [SerializeField] private GameObject rightHandUI;
    [SerializeField] private TextMeshProUGUI leftHandText;
    [SerializeField] private TextMeshProUGUI rightHandText;

    [Header("Outline")]
    [SerializeField] private Renderer[] chairRenderers;
    [SerializeField] private Material highlightMaterial;

    private bool isSeated = false;
    private Vector3 standPosition;
    private Quaternion standRotation;
    private readonly List<Material[]> originalMaterials = new();

    void Start()
    {
        standPosition = xrOrigin.position;
        standRotation = xrOrigin.rotation;

        foreach (var r in chairRenderers)
            originalMaterials.Add(r.sharedMaterials);

        if (leftHandUI) leftHandUI.SetActive(false);
        if (rightHandUI) rightHandUI.SetActive(false);

        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
        interactable.selectEntered.AddListener(OnChairSelected);

        UpdateUIText();
    }

    void OnDestroy()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        if (interactable == null) return;
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
        interactable.selectEntered.RemoveListener(OnChairSelected);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        SetHighlight(true);
        SetHandUI(args.interactorObject, true);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        SetHighlight(false);
        SetHandUI(args.interactorObject, false);
    }

    private void OnChairSelected(SelectEnterEventArgs args)
    {
        ToggleSit();
    }

    private void SetHandUI(IXRInteractor interactor, bool show)
    {
        if (interactor == null) return;

        bool isLeft = false;
        if (interactor is XRBaseInteractor baseInteractor)
            isLeft = baseInteractor.handedness == InteractorHandedness.Left;
        else
            isLeft = interactor.transform.name.ToLower().Contains("left");

        if (isLeft)
            leftHandUI?.SetActive(show);
        else
            rightHandUI?.SetActive(show);
    }

    private void ToggleSit()
    {
        if (isSeated) StandUp();
        else SitDown();
    }

    private void SitDown()
    {
        if (xrOrigin == null || sitPosition == null) return;

        standPosition = xrOrigin.position;
        standRotation = xrOrigin.rotation;
        xrOrigin.position = sitPosition.position;
        xrOrigin.rotation = sitPosition.rotation;

        if (locomotion != null) locomotion.SetActive(false);

        isSeated = true;
        UpdateUIText();
    }

    private void StandUp()
    {
        if (xrOrigin == null) return;

        xrOrigin.position = standPosition;
        xrOrigin.rotation = standRotation;

        if (locomotion != null) locomotion.SetActive(true);

        isSeated = false;
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        string text = isSeated ? "Levantarse" : "Sentarse";
        if (leftHandText != null) leftHandText.text = text;
        if (rightHandText != null) rightHandText.text = text;
    }

    private void SetHighlight(bool show)
    {
        if (highlightMaterial == null) return;

        for (int i = 0; i < chairRenderers.Length; i++)
        {
            var mats = new List<Material>(originalMaterials[i]);
            if (show) mats.Add(highlightMaterial);
            chairRenderers[i].materials = mats.ToArray();
        }
    }
}