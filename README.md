
---

# Unity Procedural Island Generator

This tool is designed to help you quickly populate a Unity terrain with objects like trees, rocks, and buildings based on a set of creative rules you define.

Instead of placing every object by hand, this generator allows you to act as a director. You define the logic for where objects should go, and the tool handles the placement, allowing for rapid iteration and natural-looking results.

### Core Features

*   **Rule-Based Placement:** Control where objects spawn using rules for altitude, slope, and distance from water.
*   **Object Grouping:** Organize your prefabs into logical groups (e.g., "Forest Trees," "Boulders," "Village Huts"), each with its own unique set of placement rules.
*   **One-Click Generation:** A simple "Generate Island" button in the Unity Inspector executes all your rules and populates the scene instantly.
*   **Beginner-Friendly:** The entire system is controlled from the Unity Inspector. No coding is required to use or modify how your island generates.

---

## 1. Setup Guide

Follow these steps to integrate the generator into your Unity project.

### Step 1: Add Script Files

All three provided C# scripts must be placed together inside a `Scripts` folder in your project.

1.  In the Unity "Project" window, right-click on the `Assets` folder.
2.  Choose **Create > Folder**.
3.  Name the new folder **`Scripts`**.
4.  Drag and drop the following three files into this new `Scripts` folder:
    *   `ProceduralIslandGenerator.cs`
    *   `PlacementRule.cs`
    *   `ProceduralIslandGeneratorEditor.cs`

Your project structure should look like this:```
Your Unity Project/
└── Assets/
    └── Scripts/
        ├── ProceduralIslandGenerator.cs
        ├── PlacementRule.cs
        └── ProceduralIslandGeneratorEditor.cs
```

### Step 2: Configure Physics Layers

The generator uses Unity's physics engine for efficient object detection. This requires setting up dedicated layers.

1.  In the Unity top menu, navigate to **Edit > Project Settings...**
2.  In the settings window, select the **Tags and Layers** tab.
3.  Under the **Layers** section, add new layers for each category of object you want to place. For example:
    *   User Layer 8: `Trees`
    *   User Layer 9: `Rocks`
    *   User Layer 10: `Buildings`

### Step 3: Create the Generator in Your Scene

1.  In your main scene, create a new Empty Game Object by right-clicking in the Hierarchy panel and selecting **Create Empty**.
2.  Rename this object to something clear, like **"Island Generation Manager"**.
3.  With the "Island Generation Manager" object selected, drag the `ProceduralIslandGenerator.cs` script from your `Scripts` folder onto its Inspector panel.

---

## 2. How to Use the Generator

Select your "Island Generation Manager" object in the scene to access its control panel in the Inspector.

1.  **Assign Terrain:** Drag your terrain object from the Hierarchy into the `Target Terrain` slot.
2.  **Set Water Level:** Adjust the `Water Height` to match the water level in your scene.
3.  **Create Object Groups:**
    *   Expand the **Object Groups** list. Set its **Size** to the number of different categories you want (e.g., 2 for trees and rocks).
4.  **Configure Each Group:**
    *   **Group Name:** Give it a descriptive name (e.g., "Forest Pines").
    *   **Rule:** Define the placement logic for this group (details in the next section).
    *   **Prefabs:** Set the **Size** and drag your prefabs from the Project window into the element slots.
    *   **Placement Attempts:** Controls the density. A higher number means more objects will be attempted. Start with `1000`.
    *   **Object Layer:** Select the corresponding physics layer you created during setup (e.g., `Trees`).
5.  **Generate Your Island:**
    *   Once configured, scroll to the bottom of the script's Inspector panel.
    *   Click the large **[ Generate Island ]** button to populate your world.
    *   If you want to re-run the generator, it will automatically clear the previously generated objects.

---

## 3. Understanding the Rules

Each object group has its own set of rules that you can tweak in the Inspector.

| Setting | What It Does |
| :--- | :--- |
| **Min / Max Altitude** | Defines the lowest and highest points on the terrain where this object can appear. |
| **Max Slope** | The steepest angle (in degrees) the ground can have. This prevents objects from spawning on cliffsides. |
| **Min Water Distance** | How far away from the water's edge an object must be. |
| **Min Distance Between Objects** | The minimum required space between objects *in the same group*. This is key to preventing overlap and creating natural spacing. |

---

## 4. Important Tips

*   **Prefabs Need Colliders:** For the `Min Distance Between Objects` rule to work, your prefabs **must** have a Collider component attached (e.g., Box Collider, Capsule Collider).
*   **Assign Layers to Prefabs:** You must also assign the correct layer to your actual prefabs. Select your prefab in the `Assets` folder and, at the top of its Inspector, change its **Layer** from "Default" to the appropriate custom layer (e.g., "Trees").