using System;

public static class InteractableMessageHub
{
    public static Action<Interactable> OnInteractableFoundPlayer;
    public static Action<Interactable> OnInteractableLostPlayer;
}